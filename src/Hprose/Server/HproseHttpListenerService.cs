/**********************************************************\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: http://www.hprose.com/                 |
|                   http://www.hprose.net/                 |
|                   http://www.hprose.org/                 |
|                                                          |
\**********************************************************/
/**********************************************************\
 *                                                        *
 * HproseHttpListenerService.cs                           *
 *                                                        *
 * hprose http listener service class for C#.             *
 *                                                        *
 * LastModified: Mar 4, 2014                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !(dotNET10 || dotNET11 || ClientOnly)
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Principal;
using Hprose.Common;

namespace Hprose.Server {

    public class HproseHttpListenerService : HproseService {

        private bool crossDomainEnabled = false;
        private bool p3pEnabled = false;
        private bool getEnabled = true;
        private bool compressionEnabled = false;
        public event SendHeaderEvent OnSendHeader = null;

        [ThreadStatic]
        private static HttpListenerContext currentContext;

        protected override object[] FixArguments(Type[] argumentTypes, object[] arguments, int count, object context) {
            HttpListenerContext currentContext = (HttpListenerContext)context;
            if (argumentTypes.Length != count) {
                object[] args = new object[argumentTypes.Length];
                System.Array.Copy(arguments, args, count);
                Type argType = argumentTypes[count];
                if (argType == typeof(HttpListenerContext)) {
                    args[count] = currentContext;
                }
                else if (argType == typeof(HttpListenerRequest)) {
                    args[count] = currentContext.Request;
                }
                else if (argType == typeof(HttpListenerResponse)) {
                    args[count] = currentContext.Response;
                }
                else if (argType == typeof(IPrincipal)) {
                    args[count] = currentContext.User;
                }
                return args;
            }
            return arguments;
        }

        public override HproseMethods GlobalMethods {
            get {
                if (gMethods == null) {
                    gMethods = new HproseHttpListenerMethods();
                }
                return gMethods;
            }
        }

        public static HttpListenerContext CurrentContext {
            get {
                return currentContext;
            }
        }

        public bool IsCrossDomainEnabled {
            get {
                return crossDomainEnabled;
            }
            set {
                crossDomainEnabled = value;
            }
        }

        public bool IsP3pEnabled {
            get {
                return p3pEnabled;
            }
            set {
                p3pEnabled = value;
            }
        }

        public bool IsGetEnabled {
            get {
                return getEnabled;
            }
            set {
                getEnabled = value;
            }
        }

        public bool IsCompressionEnabled {
            get {
                return compressionEnabled;
            }
            set {
                compressionEnabled = value;
            }
        }

        private Stream GetOutputStream(HttpListenerContext currentContext) {
            Stream ostream = currentContext.Response.OutputStream;
            if (compressionEnabled) {
                string acceptEncoding = currentContext.Request.Headers["Accept-Encoding"];
                if (acceptEncoding != null) {
                    acceptEncoding = acceptEncoding.ToLower();
                    if (acceptEncoding.IndexOf("deflate") > -1) {
                        ostream = new DeflateStream(ostream, CompressionMode.Compress);
                    }
                    else if (acceptEncoding.IndexOf("gzip") > -1) {
                        ostream = new GZipStream(ostream, CompressionMode.Compress);
                    }
                }
            }
            return ostream;
        }

        private class NonBlockingWriteContext {
            public HttpListenerContext currentContext;
            public Stream ostream;
        }

        private void NonBlockingWriteCallback(IAsyncResult asyncResult) {
            NonBlockingWriteContext context = (NonBlockingWriteContext)asyncResult.AsyncState;
            try {
                context.ostream.EndWrite(asyncResult);
            }
            catch (Exception e) {
                FireErrorEvent(e);
            }
            finally {
                if (context.ostream.CanWrite) {
                    context.ostream.Close();
                    context.currentContext.Response.Close();
                }
            }
        }

        private class NonBlockingReadContext {
            public HttpListenerContext currentContext;
            public HproseHttpListenerMethods methods;
            public Stream istream;
            public MemoryStream data;
            public int bufferlength;
            public byte[] buffer;
        }

        private void NonBlockingWrite(NonBlockingReadContext context) {
            currentContext = context.currentContext;
            NonBlockingWriteContext writeContext = new NonBlockingWriteContext();
            writeContext.currentContext = context.currentContext;
            try {
                MemoryStream data = Handle(context.data, context.methods, context.currentContext);
                writeContext.ostream = GetOutputStream(context.currentContext);
                writeContext.ostream.BeginWrite(data.GetBuffer(), 0, (int)data.Length,
                        new AsyncCallback(NonBlockingWriteCallback), writeContext);
            }
            catch (Exception e) {
                FireErrorEvent(e);
                if (writeContext.ostream != null) {
                    writeContext.ostream.Close();
                }
                context.currentContext.Response.Close();
                return;
            }
            finally {
                currentContext = null;
            }
        }

        private void NonBlockingReadCallback(IAsyncResult asyncResult) {
            NonBlockingReadContext context = (NonBlockingReadContext)asyncResult.AsyncState;
            Stream istream = context.istream;
            try {
                if (istream.CanRead) {
                    int n = istream.EndRead(asyncResult);
                    if (n > 0) {
                        context.data.Write(context.buffer, 0, n);
                        istream.BeginRead(context.buffer, 0, context.bufferlength,
                                new AsyncCallback(NonBlockingReadCallback), context);
                        return;
                    }
                }
                else {
                    return;
                }
            }
            catch (Exception e) {
                FireErrorEvent(e);
                istream.Close();
                context.currentContext.Response.Close();
                return;
            }
            istream.Close();
            NonBlockingWrite(context);
        }

        private void NonBlockingHandle(HttpListenerContext currentContext, HproseHttpListenerMethods methods) {
            NonBlockingReadContext context = new NonBlockingReadContext();
            context.currentContext = currentContext;
            context.methods = methods;
            context.istream = currentContext.Request.InputStream;
            int len = (int)currentContext.Request.ContentLength64;
            context.data = (len > 0) ? new MemoryStream(len) : new MemoryStream();
            context.bufferlength = (len > 81920 || len < 0) ? 81920 : len;
            context.buffer = new byte[context.bufferlength];
            context.istream.BeginRead(context.buffer, 0, context.bufferlength, new AsyncCallback(NonBlockingReadCallback), context);
        }

        private void SendHeader(HttpListenerContext currentContext) {
            if (OnSendHeader != null) {
                OnSendHeader(currentContext);
            }
            currentContext.Response.ContentType = "text/plain";
            if (p3pEnabled) {
                currentContext.Response.AddHeader("P3P", "CP=\"CAO DSP COR CUR ADM DEV TAI PSA PSD " +
                                                         "IVAi IVDi CONi TELo OTPi OUR DELi SAMi " +
                                                         "OTRi UNRi PUBi IND PHY ONL UNI PUR FIN " +
                                                         "COM NAV INT DEM CNT STA POL HEA PRE GOV\"");
            }
            if (crossDomainEnabled) {
                string origin = currentContext.Request.Headers["Origin"];
                if (origin != null && origin != "" && origin != "null") {
                    currentContext.Response.AddHeader("Access-Control-Allow-Origin", origin);
                    currentContext.Response.AddHeader("Access-Control-Allow-Credentials", "true");
                }
                else {
                    currentContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
                }
            }
            if (compressionEnabled) {
                string acceptEncoding = currentContext.Request.Headers["Accept-Encoding"];
                if (acceptEncoding != null) {
                    acceptEncoding = acceptEncoding.ToLower();
                    if (acceptEncoding.IndexOf("deflate") > -1) {
                        currentContext.Response.AddHeader("Content-Encoding", "deflate");
                    }
                    else if (acceptEncoding.IndexOf("gzip") > -1) {
                        currentContext.Response.AddHeader("Content-Encoding", "gzip");
                    }
                }
            }
        }

        protected void Handle(HttpListenerContext context) {
            Handle(context, null);
        }

        protected void Handle(HttpListenerContext context, HproseHttpListenerMethods methods) {
            SendHeader(context);
            string method = context.Request.HttpMethod;
            if ((method == "GET") && getEnabled) {
                MemoryStream data = DoFunctionList(methods);
                NonBlockingWriteContext writeContext = new NonBlockingWriteContext();
                writeContext.currentContext = context;
                writeContext.ostream = GetOutputStream(context);
                writeContext.ostream.BeginWrite(data.GetBuffer(), 0, (int)data.Length,
                    new AsyncCallback(NonBlockingWriteCallback), writeContext);
            }
            else if (method == "POST") {
                NonBlockingHandle(context, methods);
            }
            else {
                context.Response.StatusCode = 403;
                context.Response.Close();
            }
        }
    }
}
#endif