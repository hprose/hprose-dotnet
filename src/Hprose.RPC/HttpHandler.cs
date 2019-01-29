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
|  LastModified: Jan 29, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class HttpListenerServiceContext : ServiceContext {
        public HttpListenerRequest Request { get; private set; }
        public HttpListenerResponse Response { get; private set; }
        public System.Security.Principal.IPrincipal User { get; private set; }
        public HttpListenerServiceContext(Service service, HttpListenerContext context) : base(service) {
            Request = context.Request;
            Response = context.Response;
            User = context.User;
        }
    }
    public class HttpHandler : IHandler<HttpListener> {
        private static readonly MemoryStream emptyStream = new MemoryStream(0);
        public bool CrossDomain { get; set; } = true;
        public bool P3P { get; set; } = true;
        public bool Get { get; set; } = true;
        public bool Compress { get; set; } = false;
        public string CrossDomainXmlFile { get; set; } = null;
        public string ClientAccessPolicyXmlFile { get; set; } = null;
        private string lastModified;
        private string etag;
        private readonly Dictionary<string, bool> origins = new Dictionary<string, bool>();
        public Service Service { get; private set; }
        public HttpHandler(Service service) {
            Service = service;
            var rand = new Random();
            lastModified = DateTime.Now.ToString("R");
            etag = '"' + rand.Next().ToString("x") + ":" + rand.Next().ToString("x") + '"';
        }
        public async Task Bind(HttpListener server) {
            while (server.IsListening) {
                await Handler(await server.GetContextAsync());
            }
        }
        public void AddAccessControlAllowOrigin(string origin) {
            origins[origin] = true;
        }
        public void RemoveAccessControlAllowOrigin(string origin) {
            origins.Remove(origin);
        }
        private Stream GetOutputStream(HttpListenerRequest request, HttpListenerResponse response) {
            Stream ostream = response.OutputStream;
            if (Compress) {
                string acceptEncoding = request.Headers["Accept-Encoding"];
                if (acceptEncoding != null) {
                    acceptEncoding = acceptEncoding.ToLower();
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
        private void SendHeader(HttpListenerRequest request, HttpListenerResponse response) {
            response.ContentType = "text/plain";
            if (P3P) {
                response.AddHeader("P3P",
                    "CP=\"CAO DSP COR CUR ADM DEV TAI PSA PSD IVAi IVDi " +
                    "CONi TELo OTPi OUR DELi SAMi OTRi UNRi PUBi IND PHY ONL " +
                    "UNI PUR FIN COM NAV INT DEM CNT STA POL HEA PRE GOV\"");
            }
            if (CrossDomain) {
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
        }
        private async Task<bool> CrossDomainXmlHandler(HttpListenerRequest request, HttpListenerResponse response) {
            if (request.Url.AbsolutePath.ToLower() == "/crossdomain.xml") {
                if (request.Headers["If-Modified-Since"] == lastModified &&
                    request.Headers["If-None-Match"] == etag) {
                    response.StatusCode = 304;
                }
                else if (CrossDomainXmlFile != null) {
                    response.AppendHeader("Last-Modified", lastModified);
                    response.AppendHeader("Etag", etag);
                    response.ContentType = "text/xml";
                    using (var fileStream = new FileStream(CrossDomainXmlFile, FileMode.Open, FileAccess.Read)) {
                        await fileStream.CopyToAsync(GetOutputStream(request, response));
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
            if (request.Url.AbsolutePath.ToLower() == "/clientaccesspolicy.xml") {
                if (request.Headers["If-Modified-Since"] == lastModified &&
                    request.Headers["If-None-Match"] == etag) {
                    response.StatusCode = 304;
                }
                else if (ClientAccessPolicyXmlFile != null) {
                    response.AppendHeader("Last-Modified", lastModified);
                    response.AppendHeader("Etag", etag);
                    response.ContentType = "text/xml";
                    using (var fileStream = new FileStream(ClientAccessPolicyXmlFile, FileMode.Open, FileAccess.Read)) {
                        await fileStream.CopyToAsync(GetOutputStream(request, response));
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
        public async Task Handler(HttpListenerContext httpContext) {
            HttpListenerServiceContext context = new HttpListenerServiceContext(Service, httpContext);
            var request = httpContext.Request;
            var response = httpContext.Response;
            if (await ClientAccessPolicyXmlHandler(request, response)) {
                return;
            }
            if (await CrossDomainXmlHandler(request, response)) {
                return;
            }
            var stream = request.InputStream ?? emptyStream;
            if (request.ContentLength64 > Service.MaxRequestLength
                || request.ContentLength64 == -1
                    && stream.CanSeek
                    && stream.Length > Service.MaxRequestLength) {
                response.StatusCode = 413;
                response.StatusDescription = "Request Entity Too Large";
                response.Close();
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
                    outstream = await Service.Handle(stream, context);
                    break;
            }
            SendHeader(request, response);
            if (outstream != null) {
                await outstream.CopyToAsync(GetOutputStream(request, response));
            }
            response.Close();
        }
    }

    public partial class Service {
        public HttpHandler Http => (HttpHandler)this["Http"];
        static Service() {
            Register<HttpHandler, HttpListener>("Http");
        }
    }
}

