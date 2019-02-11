/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  AspNetHttpHandler.cs                                    |
|                                                          |
|  AspNetHttpHandler class for C#.                         |
|                                                          |
|  LastModified: Feb 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Text;

namespace Hprose.RPC.AspNet {
    public class AspNetHttpHandler : IHandler<HttpContext> {
        public bool CrossDomain { get; set; } = true;
        public bool P3P { get; set; } = true;
        public bool Get { get; set; } = true;
        public bool Compress { get; set; } = false;
        public string CrossDomainXmlFile { get; set; } = null;
        public string ClientAccessPolicyXmlFile { get; set; } = null;
        private readonly string lastModified;
        private readonly string etag;
        private readonly Dictionary<string, bool> origins = new Dictionary<string, bool>();
        public Action<Exception> OnError { get; set; } = null;
        public Service Service { get; private set; }
        public AspNetHttpHandler(Service service) {
            Service = service;
            var rand = new Random();
            lastModified = DateTime.Now.ToString("R", CultureInfo.InvariantCulture);
            etag = '"' + rand.Next().ToString("x", CultureInfo.InvariantCulture) + ":" + rand.Next().ToString("x", CultureInfo.InvariantCulture) + '"';
        }
        public Task Bind(HttpContext server) {
            return Handler(server);
        }
        public void AddAccessControlAllowOrigin(string origin) {
            origins[origin] = true;
        }
        public void RemoveAccessControlAllowOrigin(string origin) {
            origins.Remove(origin);
        }
        private Stream GetOutputStream(HttpRequest request, HttpResponse response) {
            Stream ostream = new BufferedStream(response.OutputStream);
            if (Compress) {
                string acceptEncoding = request.Headers["Accept-Encoding"];
                if (acceptEncoding != null) {
                    acceptEncoding = acceptEncoding.ToLowerInvariant();
                    if (acceptEncoding.Contains("gzip")) {
                        response.AppendHeader("Content-Encoding", "gzip");
                        ostream = new GZipStream(ostream, CompressionMode.Compress);
                    }
                    else if (acceptEncoding.Contains("deflate")) {
                        response.AppendHeader("Content-Encoding", "deflate");
                        ostream = new DeflateStream(ostream, CompressionMode.Compress);
                    }
                }
            }
            return ostream;
        }
        private void SendHeader(HttpRequest request, HttpResponse response) {
            response.ContentType = "text/plain";
            if (P3P) {
                response.AppendHeader("P3P",
                    "CP=\"CAO DSP COR CUR ADM DEV TAI PSA PSD IVAi IVDi " +
                    "CONi TELo OTPi OUR DELi SAMi OTRi UNRi PUBi IND PHY ONL " +
                    "UNI PUR FIN COM NAV INT DEM CNT STA POL HEA PRE GOV\"");
            }
            if (CrossDomain) {
                string origin = request.Headers["Origin"];
                if (string.IsNullOrEmpty(origin) || origin == "null") {
                    response.AppendHeader("Access-Control-Allow-Origin", "*");
                }
                else if (origins.Count == 0 || origins.ContainsKey(origin)) {
                    response.AppendHeader("Access-Control-Allow-Origin", origin);
                    response.AppendHeader("Access-Control-Allow-Credentials", "true");
                }
            }
        }
        private async Task<bool> CrossDomainXmlHandler(HttpRequest request, HttpResponse response) {
            if (request.Url.AbsolutePath.ToLowerInvariant() == "/crossdomain.xml") {
                if (request.Headers["If-Modified-Since"] == lastModified &&
                    request.Headers["If-None-Match"] == etag) {
                    response.StatusCode = 304;
                }
                else if (CrossDomainXmlFile != null) {
                    response.AppendHeader("Last-Modified", lastModified);
                    response.AppendHeader("Etag", etag);
                    response.ContentType = "text/xml";
                    using (var fileStream = new FileStream(CrossDomainXmlFile, FileMode.Open, FileAccess.Read)) {
                        using (var outputStream = GetOutputStream(request, response)) {
                            await fileStream.CopyToAsync(outputStream).ConfigureAwait(false);
                        }
                    };
                }
                else {
                    response.StatusCode = 404;
                }
                return true;
            }
            return false;
        }
        private async Task<bool> ClientAccessPolicyXmlHandler(HttpRequest request, HttpResponse response) {
            if (request.Url.AbsolutePath.ToLowerInvariant() == "/clientaccesspolicy.xml") {
                if (request.Headers["If-Modified-Since"] == lastModified &&
                    request.Headers["If-None-Match"] == etag) {
                    response.StatusCode = 304;
                }
                else if (ClientAccessPolicyXmlFile != null) {
                    response.AppendHeader("Last-Modified", lastModified);
                    response.AppendHeader("Etag", etag);
                    response.ContentType = "text/xml";
                    using (var fileStream = new FileStream(ClientAccessPolicyXmlFile, FileMode.Open, FileAccess.Read)) {
                        using (var outputStream = GetOutputStream(request, response)) {
                            await fileStream.CopyToAsync(outputStream).ConfigureAwait(false);
                        }
                    };
                }
                else {
                    response.StatusCode = 404;
                }
                return true;
            }
            return false;
        }
        public static string GetIP(HttpRequest request) {
            string result = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result)) {
                result = request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result)) {
                result = request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result)) {
                result = "0.0.0.0";
            }
            return result;
        }
        public virtual async Task Handler(HttpContext httpContext) {
            var request = httpContext.Request;
            var response = httpContext.Response;
            dynamic context = new ServiceContext(Service);
            context.HttpContext = httpContext;
            context.Request = request;
            context.Response = response;
            context.User = httpContext.User;
            context.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(GetIP(request)), 0);
            context.Handler = this;
            if (await ClientAccessPolicyXmlHandler(request, response).ConfigureAwait(false)) {
                return;
            }
            if (await CrossDomainXmlHandler(request, response).ConfigureAwait(false)) {
                return;
            }
            using (var instream = request.InputStream) {
                if (request.ContentLength > Service.MaxRequestLength) {
                    instream.Dispose();
                    response.StatusCode = 413;
                    response.StatusDescription = "Request Entity Too Large";
                    return;
                }
                string method = request.HttpMethod;
                Stream outstream = null;
                switch (method) {
                    case "GET":
                        if (!Get) {
                            response.StatusCode = 403;
                            response.StatusDescription = "Forbidden";
                            break;
                        }
                        goto case "POST";
                    case "POST":
                        try {
                            outstream = await Service.Handle(instream, context);
                        }
                        catch (Exception e) {
                            response.StatusCode = 500;
                            response.StatusDescription = "Internal Server Error";
                            using (var outputStream = GetOutputStream(request, response)) {
                                var stackTrace = Encoding.UTF8.GetBytes(e.StackTrace);
                                await outputStream.WriteAsync(stackTrace, 0, stackTrace.Length).ConfigureAwait(false);
                            }
                            return;
                        }
                        break;
                }
                SendHeader(request, response);
                if (outstream != null) {
                    using (var outputStream = GetOutputStream(request, response)) {
                        await outstream.CopyToAsync(outputStream).ConfigureAwait(false);
                    }
                    outstream.Dispose();
                }
            }
        }
    }
}
