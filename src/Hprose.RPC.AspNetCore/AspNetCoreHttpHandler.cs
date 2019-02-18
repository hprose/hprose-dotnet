/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  AspNetCoreHttpHandler.cs                                |
|                                                          |
|  AspNetCoreHttpHandler class for C#.                     |
|                                                          |
|  LastModified: Feb 12, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hprose.RPC.AspNetCore {
    public class AspNetCoreHttpHandler : IHandler<HttpContext> {
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
        public AspNetCoreHttpHandler(Service service) {
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
            Stream ostream = new BufferedStream(response.Body);
            if (Compress) {
                string acceptEncoding = request.Headers["Accept-Encoding"];
                if (acceptEncoding != null) {
                    acceptEncoding = acceptEncoding.ToLowerInvariant();
                    if (acceptEncoding.Contains("gzip")) {
                        response.Headers.Add("Content-Encoding", "gzip");
                        ostream = new GZipStream(ostream, CompressionMode.Compress);
                    }
                    else if (acceptEncoding.Contains("deflate")) {
                        response.Headers.Add("Content-Encoding", "deflate");
                        ostream = new DeflateStream(ostream, CompressionMode.Compress);
                    }
                }
            }
            return ostream;
        }
        private void SendHeader(HttpRequest request, HttpResponse response) {
            response.ContentType = "text/plain";
            if (P3P) {
                response.Headers.Add("P3P",
                    "CP=\"CAO DSP COR CUR ADM DEV TAI PSA PSD IVAi IVDi " +
                    "CONi TELo OTPi OUR DELi SAMi OTRi UNRi PUBi IND PHY ONL " +
                    "UNI PUR FIN COM NAV INT DEM CNT STA POL HEA PRE GOV\"");
            }
            if (CrossDomain) {
                string origin = request.Headers["Origin"];
                if (string.IsNullOrEmpty(origin) || origin == "null") {
                    response.Headers.Add("Access-Control-Allow-Origin", "*");
                }
                else if (origins.Count == 0 || origins.ContainsKey(origin)) {
                    response.Headers.Add("Access-Control-Allow-Origin", origin);
                    response.Headers.Add("Access-Control-Allow-Credentials", "true");
                }
            }
        }
        private async Task<bool> CrossDomainXmlHandler(HttpRequest request, HttpResponse response) {
            if (request.Path.Value.ToLowerInvariant() == "/crossdomain.xml") {
                if (request.Headers["If-Modified-Since"] == lastModified &&
                    request.Headers["If-None-Match"] == etag) {
                    response.StatusCode = 304;
                }
                else if (CrossDomainXmlFile != null) {
                    response.Headers.Add("Last-Modified", lastModified);
                    response.Headers.Add("Etag", etag);
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
            if (request.Path.Value.ToLowerInvariant() == "/clientaccesspolicy.xml") {
                if (request.Headers["If-Modified-Since"] == lastModified &&
                    request.Headers["If-None-Match"] == etag) {
                    response.StatusCode = 304;
                }
                else if (ClientAccessPolicyXmlFile != null) {
                    response.Headers.Add("Last-Modified", lastModified);
                    response.Headers.Add("Etag", etag);
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
        public static IPEndPoint GetIPEndPoint(HttpContext httpContext) {
            var ip = httpContext.Connection.RemoteIpAddress;
            var port = httpContext.Connection.RemotePort;
            return new IPEndPoint(ip, port);
        }
        public virtual async Task Handler(HttpContext httpContext) {
            var request = httpContext.Request;
            var response = httpContext.Response;
            var context = new ServiceContext(Service);
            context["httpContext"] = httpContext;
            context["request"] = request;
            context["response"] = response;
            context["user"] = httpContext.User;
            context.RemoteEndPoint = GetIPEndPoint(httpContext);
            context.Handler = this;
            if (await ClientAccessPolicyXmlHandler(request, response).ConfigureAwait(false)) {
                return;
            }
            if (await CrossDomainXmlHandler(request, response).ConfigureAwait(false)) {
                return;
            }
            using (var instream = request.Body) {
                if (request.ContentLength > Service.MaxRequestLength) {
                    instream.Dispose();
                    response.StatusCode = 413;
                    return;
                }
                string method = request.Method;
                Stream outstream = null;
                switch (method) {
                    case "GET":
                        if (!Get) {
                            response.StatusCode = 403;
                            break;
                        }
                        goto case "POST";
                    case "POST":
                        try {
                            outstream = await Service.Handle(instream, context).ConfigureAwait(false);
                        }
                        catch (Exception e) {
                            response.StatusCode = 500;
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
