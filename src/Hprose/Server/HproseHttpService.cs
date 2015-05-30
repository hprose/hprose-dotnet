/**********************************************************\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: http://www.hprose.com/                 |
|                   http://www.hprose.org/                 |
|                                                          |
\**********************************************************/
/**********************************************************\
 *                                                        *
 * HproseHttpService.cs                                   *
 *                                                        *
 * hprose http service class for C#.                      *
 *                                                        *
 * LastModified: May 30, 2015                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !(ClientOnly || ClientProfile || Smartphone)
using System;
using System.Collections;
#if !(dotNET10 || dotNET11)
using System.Collections.Generic;
#endif
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
#if !(dotNET10 || dotNET11)
        private Dictionary<string, bool> origins = new Dictionary<string, bool>();
#else
        private Hashtable origins = new Hashtable();
#endif
        public event SendHeaderEvent OnSendHeader = null;
        [ThreadStatic]
        private static HttpContext currentContext;

        protected override object[] FixArguments(Type[] argumentTypes, object[] arguments, int count, HproseContext context) {
            HproseHttpContext currentContext = (HproseHttpContext)context;
            if (argumentTypes.Length != count) {
                object[] args = new object[argumentTypes.Length];
                System.Array.Copy(arguments, 0, args, 0, count);
                Type argType = argumentTypes[count];
                if (argType == typeof(HproseContext) ||
                    argType == typeof(HproseHttpContext)) {
                    args[count] = currentContext;
                }
                else if (argType == typeof(HttpContext)) {
                    args[count] = currentContext.Context;
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

        public static new HttpContext CurrentContext {
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

        public void AddAccessControlAllowOrigin(string origin) {
            origins[origin] = true;
        }

        public void RemoveAccessControlAllowOrigin(string origin) {
            origins.Remove(origin);
        }

        private Stream GetOutputStream(HproseHttpContext context) {
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

        private MemoryStream GetInputStream(HproseHttpContext context) {
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

        private void SendHeader(HproseHttpContext context) {
            if (OnSendHeader != null) {
                OnSendHeader(context);
            }
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            response.ContentType = "text/plain";
            if (p3pEnabled) {
                response.AddHeader("P3P",
                    "CP=\"CAO DSP COR CUR ADM DEV TAI PSA PSD " +
                    "IVAi IVDi CONi TELo OTPi OUR DELi SAMi " +
                    "OTRi UNRi PUBi IND PHY ONL UNI PUR FIN " +
                    "COM NAV INT DEM CNT STA POL HEA PRE GOV\"");
            }
            if (crossDomainEnabled) {
                string origin = request.Headers["Origin"];
                if (origin != null && origin != "" && origin != "null") {
                    if (origins.Count == 0 || origins.ContainsKey(origin)) {
                        response.AddHeader("Access-Control-Allow-Origin", origin);
                        response.AddHeader("Access-Control-Allow-Credentials", "true");
                    }
                }
                else {
                    response.AddHeader("Access-Control-Allow-Origin", "*");
                }
            }
            if (compressionEnabled) {
                string acceptEncoding = request.Headers["Accept-Encoding"];
                if (acceptEncoding != null) {
                    acceptEncoding = acceptEncoding.ToLower();
                    if (acceptEncoding.IndexOf("deflate") > -1) {
                        response.AddHeader("Content-Encoding", "deflate");
                    }
                    else if (acceptEncoding.IndexOf("gzip") > -1) {
                        response.AddHeader("Content-Encoding", "gzip");
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

        public void Handle(HttpContext httpContext) {
            Handle(httpContext, null);
        }

        public void Handle(HttpContext httpContext, HproseHttpMethods methods) {
            currentContext = httpContext;
            try {
                HproseHttpContext context = new HproseHttpContext(httpContext);
                SendHeader(context);
                string method = context.Request.HttpMethod;
                Stream ostream = GetOutputStream(context);
                if ((method == "GET") && getEnabled) {
                    if (getEnabled) {
                        DoFunctionList(methods, context)
                        .WriteTo(ostream);
                    }
                    else {
                        context.Response.StatusCode = 403;
                    }
                }
                else if (method == "POST") {
                    Handle(GetInputStream(context), methods, context)
                    .WriteTo(ostream);
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
