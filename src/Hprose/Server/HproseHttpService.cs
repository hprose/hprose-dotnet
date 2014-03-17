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
 * HproseHttpService.cs                                   *
 *                                                        *
 * hprose http service class for C#.                      *
 *                                                        *
 * LastModified: Mar 17, 2014                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !(ClientOnly || ClientProfile)
using System;
using System.IO;
using System.IO.Compression;
using System.Web;
using System.Web.SessionState;
using Hprose.Common;

namespace Hprose.Server {

    public class HproseHttpService : HproseService {

        private bool crossDomainEnabled = false;
        private bool p3pEnabled = false;
        private bool getEnabled = true;
        private bool compressionEnabled = false;
        public event SendHeaderEvent OnSendHeader = null;
        [ThreadStatic]
        private static HttpContext currentContext;

        protected override object[] FixArguments(Type[] argumentTypes, object[] arguments, int count, object context) {
            HttpContext currentContext = (HttpContext)context;
            if (argumentTypes.Length != count) {
                object[] args = new object[argumentTypes.Length];
                System.Array.Copy(arguments, args, count);
                Type argType = argumentTypes[count];
                if (argType == typeof(HttpContext)) {
                    args[count] = currentContext;
                }
                else if (argType == typeof(HttpRequest)) {
                    args[count] = currentContext.Request;
                }
                else if (argType == typeof(HttpResponse)) {
                    args[count] = currentContext.Response;
                }
                else if (argType == typeof(HttpServerUtility)) {
                    args[count] = currentContext.Server;
                }
                else if (argType == typeof(HttpApplicationState)) {
                    args[count] = currentContext.Application;
                }
                else if (argType == typeof(HttpSessionState)) {
                    args[count] = currentContext.Session;
                }
                return args;
            }
            return arguments;
        }

        public override HproseMethods GlobalMethods {
            get {
                if (gMethods == null) {
                    gMethods = new HproseHttpMethods();
                }
                return gMethods;
            }
        }

        public static HttpContext CurrentContext {
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

        private Stream GetOutputStream(HttpContext context) {
            Stream ostream = new BufferedStream(context.Response.OutputStream);
            if (compressionEnabled) {
                string acceptEncoding = context.Request.Headers["Accept-Encoding"];
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

        private MemoryStream GetInputStream(HttpContext context) {
            Stream istream = context.Request.InputStream;
            int len = (int)istream.Length;
            int off = 0;
            byte[] data = new byte[len];
            while (len > 0) {
                int size = istream.Read(data, off, len);
                off += size;
                len -= size;
            }
            istream.Close();
            return new MemoryStream(data);
        }

        private void SendHeader(HttpContext context) {
            if (OnSendHeader != null) {
                OnSendHeader(context);
            }
            context.Response.ContentType = "text/plain";
            if (p3pEnabled) {
                context.Response.AddHeader("P3P", "CP=\"CAO DSP COR CUR ADM DEV TAI PSA PSD " +
                                                         "IVAi IVDi CONi TELo OTPi OUR DELi SAMi " +
                                                         "OTRi UNRi PUBi IND PHY ONL UNI PUR FIN " +
                                                         "COM NAV INT DEM CNT STA POL HEA PRE GOV\"");
            }
            if (crossDomainEnabled) {
                string origin = context.Request.Headers["Origin"];
                if (origin != null && origin != "" && origin != "null") {
                    context.Response.AddHeader("Access-Control-Allow-Origin", origin);
                    context.Response.AddHeader("Access-Control-Allow-Credentials", "true");
                }
                else {
                    context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                }
            }
            if (compressionEnabled) {
                string acceptEncoding = context.Request.Headers["Accept-Encoding"];
                if (acceptEncoding != null) {
                    acceptEncoding = acceptEncoding.ToLower();
                    if (acceptEncoding.IndexOf("deflate") > -1) {
                        context.Response.AddHeader("Content-Encoding", "deflate");
                    }
                    else if (acceptEncoding.IndexOf("gzip") > -1) {
                        context.Response.AddHeader("Content-Encoding", "gzip");
                    }
                }
            }
        }

        public void Handle() {
            Handle(HttpContext.Current, null);
        }

        public void Handle(HproseMethods methods) {
            Handle(HttpContext.Current, (HproseHttpMethods) methods);
        }

        public void Handle(HttpContext context) {
            Handle(context, null);
        }

        public void Handle(HttpContext context, HproseHttpMethods methods) {
            currentContext = context;
            try {
                SendHeader(context);
                string method = context.Request.HttpMethod;
                Stream ostream = GetOutputStream(context);
                if ((method == "GET") && getEnabled) {
                    DoFunctionList(methods, context).WriteTo(ostream);
                }
                else if (method == "POST") {
                    Handle(GetInputStream(context), methods, context).WriteTo(ostream);
                }
                else {
                    context.Response.StatusCode = 403;
                }
                ostream.Close();
                context.Response.Flush();
            }
            finally {
                currentContext = null;
            }
        }
    }
}
#endif