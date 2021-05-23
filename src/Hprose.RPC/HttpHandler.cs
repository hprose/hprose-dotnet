/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  HttpHandler.cs                                          |
|                                                          |
|  HttpHandler class for C#.                               |
|                                                          |
|  LastModified: Mar 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

#if !NET35_CF
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System.Globalization;
using System.Collections.Specialized;

namespace Hprose.RPC {
    public class HttpHandler : IHandler<HttpListener> {
        public bool CrossDomain { get; set; } = true;
        public bool P3P { get; set; } = true;
        public bool Get { get; set; } = true;
        public bool Compress { get; set; } = false;
        public NameValueCollection HttpHeaders { get; set; } = new(StringComparer.InvariantCultureIgnoreCase);
        public string CrossDomainXmlFile { get; set; } = null;
        public string ClientAccessPolicyXmlFile { get; set; } = null;
        private readonly string lastModified;
        private readonly string etag;
        private readonly Dictionary<string, bool> origins = new();
        public virtual event Action<Exception> OnError;
        public Service Service { get; private set; }
        public HttpHandler(Service service) {
            Service = service;
            var rand = new Random();
            lastModified = DateTime.Now.ToString("R", CultureInfo.InvariantCulture);
            etag = '"' + rand.Next().ToString("x", CultureInfo.InvariantCulture) + ":" + rand.Next().ToString("x", CultureInfo.InvariantCulture) + '"';
        }
        public Task Bind(HttpListener server) {
            return Task.Factory.StartNew(async () => {
                while (server.IsListening) {
                    Handler(await server.GetContextAsync().ConfigureAwait(false));
                }
            }, TaskCreationOptions.LongRunning);
        }
        public void AddAccessControlAllowOrigin(string origin) {
            origins[origin] = true;
        }
        public void RemoveAccessControlAllowOrigin(string origin) {
            origins.Remove(origin);
        }
        private Stream GetOutputStream(HttpListenerRequest request, HttpListenerResponse response) {
            var ostream = response.OutputStream;
            if (Compress) {
                string acceptEncoding = request.Headers["Accept-Encoding"];
                if (acceptEncoding != null) {
                    acceptEncoding = acceptEncoding.ToLowerInvariant();
                    if (acceptEncoding.Contains("gzip")) {
                        response.AddHeader("Content-Encoding", "gzip");
                        ostream = new GZipStream(ostream, CompressionMode.Compress);
                    }
                    else if (acceptEncoding.Contains("deflate")) {
                        response.AddHeader("Content-Encoding", "deflate");
                        ostream = new DeflateStream(ostream, CompressionMode.Compress);
                    }
                }
            }
            return ostream;
        }
        private void SendHeader(HttpListenerRequest request, HttpListenerResponse response, Context context) {
            if (context.Contains("httpStatusCode")) {
                response.StatusCode = (int)context["httpStatusCode"];
                if (context.Contains("httpStatusText")) {
                    response.StatusDescription = (string)context["httpStatusText"];
                }
            }
            else {
                response.StatusCode = 200;
            }
            response.ContentType = "text/plain";
            if (P3P) {
                response.AddHeader("P3P",
                    "CP=\"CAO DSP COR CUR ADM DEV TAI PSA PSD IVAi IVDi " +
                    "CONi TELo OTPi OUR DELi SAMi OTRi UNRi PUBi IND PHY ONL " +
                    "UNI PUR FIN COM NAV INT DEM CNT STA POL HEA PRE GOV\"");
            }
            if (CrossDomain) {
                string origin = request.Headers["Origin"];
                if (string.IsNullOrEmpty(origin) || origin == "null") {
                    response.AddHeader("Access-Control-Allow-Origin", "*");
                }
                else if (origins.Count == 0 || origins.ContainsKey(origin)) {
                    response.AddHeader("Access-Control-Allow-Origin", origin);
                    response.AddHeader("Access-Control-Allow-Credentials", "true");
                }
            }
            if (HttpHeaders != null) {
                response.Headers.Add(HttpHeaders);
            }
            if (context.Contains("httpResponseHeaders")) {
                var headers = context["httpResponseHeaders"] as NameValueCollection;
                response.Headers.Add(headers);
            }
        }
        private async Task<bool> CrossDomainXmlHandler(HttpListenerRequest request, HttpListenerResponse response) {
            if (request.Url.AbsolutePath.ToLowerInvariant() == "/crossdomain.xml") {
                if (request.Headers["If-Modified-Since"] == lastModified &&
                    request.Headers["If-None-Match"] == etag) {
                    response.StatusCode = 304;
                }
                else if (CrossDomainXmlFile != null) {
                    response.AddHeader("Last-Modified", lastModified);
                    response.AddHeader("Etag", etag);
                    response.ContentType = "text/xml";
                    using (var fileStream = new FileStream(CrossDomainXmlFile, FileMode.Open, FileAccess.Read)) {
                        using var outputStream = GetOutputStream(request, response);
                        await fileStream.CopyToAsync(outputStream).ConfigureAwait(false);
                    };
                }
                else {
                    response.StatusCode = 404;
                }
                response.Close();
                return true;
            }
            return false;
        }

        private async Task<bool> ClientAccessPolicyXmlHandler(HttpListenerRequest request, HttpListenerResponse response) {
            if (request.Url.AbsolutePath.ToLowerInvariant() == "/clientaccesspolicy.xml") {
                if (request.Headers["If-Modified-Since"] == lastModified &&
                    request.Headers["If-None-Match"] == etag) {
                    response.StatusCode = 304;
                }
                else if (ClientAccessPolicyXmlFile != null) {
                    response.AddHeader("Last-Modified", lastModified);
                    response.AddHeader("Etag", etag);
                    response.ContentType = "text/xml";
                    using (var fileStream = new FileStream(ClientAccessPolicyXmlFile, FileMode.Open, FileAccess.Read)) {
                        using var outputStream = GetOutputStream(request, response);
                        await fileStream.CopyToAsync(outputStream).ConfigureAwait(false);
                    };
                }
                else {
                    response.StatusCode = 404;
                }
                response.Close();
                return true;
            }
            return false;
        }
        public virtual async void Handler(HttpListenerContext httpContext) {
            try {
                var request = httpContext.Request;
                var response = httpContext.Response;
                var context = new ServiceContext(Service);
                context["httpContext"] = httpContext;
                context["request"] = request;
                context["response"] = response;
                context["user"] = httpContext.User;
                context.RemoteEndPoint = request.RemoteEndPoint;
                context.LocalEndPoint = request.LocalEndPoint;
                context.Handler = this;
                context["httpRequestHeaders"] = request.Headers;
                if (request.HttpMethod == "GET") {
                    if (await ClientAccessPolicyXmlHandler(request, response).ConfigureAwait(false)) {
                        return;
                    }
                    if (await CrossDomainXmlHandler(request, response).ConfigureAwait(false)) {
                        return;
                    }
                    if (!Get) {
                        response.StatusCode = 403;
                        response.StatusDescription = "Forbidden";
                        response.Close();
                        return;
                    }
                }
                using (var instream = request.InputStream) {
                    if (request.ContentLength64 > Service.MaxRequestLength
                        || request.ContentLength64 == -1
                            && instream.CanSeek
                            && instream.Length > Service.MaxRequestLength) {
                        instream.Dispose();
                        response.StatusCode = 413;
                        response.StatusDescription = "Request Entity Too Large";
                        response.Close();
                        return;
                    }
                    Stream outstream = null;
                    try {
                        outstream = await Service.Handle(instream, context).ConfigureAwait(false);
                    }
                    catch (Exception e) {
                        response.StatusCode = 500;
                        response.StatusDescription = "Internal Server Error";
                        using (var outputStream = GetOutputStream(request, response)) {
                            var stackTrace = Encoding.UTF8.GetBytes(e.StackTrace);
                            await outputStream.WriteAsync(stackTrace, 0, stackTrace.Length).ConfigureAwait(false);
                        }
                        response.Close();
                        return;
                    }
                    SendHeader(request, response, context);
                    if (outstream != null) {
                        using (var outputStream = GetOutputStream(request, response)) {
                            await outstream.CopyToAsync(outputStream).ConfigureAwait(false);
                        }
                        outstream.Dispose();
                    }
                }
                response.Close();
            }
            catch (Exception error) {
                OnError?.Invoke(error);
            }
        }
    }
}
#endif