﻿/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  OwinHttpHandler.cs                                      |
|                                                          |
|  OwinHttpHandler class for C#.                           |
|                                                          |
|  LastModified: Mar 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hprose.RPC.Owin {
    public class OwinHttpHandler : IHandler<IDictionary<string, object>> {
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
        public Service Service { get; private set; }
        public OwinHttpHandler(Service service) {
            Service = service;
            var rand = new Random();
            lastModified = DateTime.Now.ToString("R", CultureInfo.InvariantCulture);
            etag = '"' + rand.Next().ToString("x", CultureInfo.InvariantCulture) + ":" + rand.Next().ToString("x", CultureInfo.InvariantCulture) + '"';
        }
        public Task Bind(IDictionary<string, object> environment) {
            return Handler(environment);
        }
        public void AddAccessControlAllowOrigin(string origin) {
            origins[origin] = true;
        }
        public void RemoveAccessControlAllowOrigin(string origin) {
            origins.Remove(origin);
        }
        private Stream GetOutputStream(IDictionary<string, object> environment) {
            Stream ostream = new BufferedStream(environment["owin.ResponseBody"] as Stream);
            if (Compress) {
                var requestHeaders = environment["owin.RequestHeaders"] as IDictionary<string, string[]>;
                var responseHeaders = environment["owin.ResponseHeaders"] as IDictionary<string, string[]>;
                var acceptEncoding = requestHeaders["Accept-Encoding"]?[0];
                if (acceptEncoding != null) {
                    acceptEncoding = acceptEncoding.ToLowerInvariant();
                    if (acceptEncoding.Contains("gzip")) {
                        responseHeaders.Add("Content-Encoding", new string[] { "gzip" });
                        ostream = new GZipStream(ostream, CompressionMode.Compress);
                    }
                    else if (acceptEncoding.Contains("deflate")) {
                        responseHeaders.Add("Content-Encoding", new string[] { "deflate" });
                        ostream = new DeflateStream(ostream, CompressionMode.Compress);
                    }
                }
            }
            return ostream;
        }
        private void SendHeader(IDictionary<string, object> environment, Context context) {
            var requestHeaders = environment["owin.RequestHeaders"] as IDictionary<string, string[]>;
            var responseHeaders = environment["owin.ResponseHeaders"] as IDictionary<string, string[]>;
            if (context.Contains("httpStatusCode")) {
                environment["owin.ResponseStatusCode"] = context["httpStatusCode"];
            }
            else {
                environment["owin.ResponseStatusCode"] = 200;
            }
            responseHeaders.Add("Content-Type", new string[] { "text/plain" });
            if (P3P) {
                responseHeaders.Add("P3P", new string[] {
                    "CP=\"CAO DSP COR CUR ADM DEV TAI PSA PSD IVAi IVDi " +
                    "CONi TELo OTPi OUR DELi SAMi OTRi UNRi PUBi IND PHY ONL " +
                    "UNI PUR FIN COM NAV INT DEM CNT STA POL HEA PRE GOV\"" });
            }
            if (CrossDomain) {
                string origin = requestHeaders["Origin"]?[0];
                if (string.IsNullOrEmpty(origin) || origin == "null") {
                    responseHeaders.Add("Access-Control-Allow-Origin", new string[] { "*" });
                }
                else if (origins.Count == 0 || origins.ContainsKey(origin)) {
                    responseHeaders.Add("Access-Control-Allow-Origin", new string[] { origin });
                    responseHeaders.Add("Access-Control-Allow-Credentials", new string[] { "true" });
                }
            }
            if (HttpHeaders != null) {
                foreach (var key in HttpHeaders.AllKeys) {
                    responseHeaders.Add(key, HttpHeaders.GetValues(key));
                }
            }
            if (context.Contains("httpResponseHeaders")) {
                var headers = context["httpResponseHeaders"] as NameValueCollection;
                foreach (var key in headers.AllKeys) {
                    responseHeaders.Add(key, headers.GetValues(key));
                }
            }
        }
        private async Task<bool> CrossDomainXmlHandler(IDictionary<string, object> environment) {
            if ((environment["owin.RequestPath"] as string)?.ToLowerInvariant() == "/crossdomain.xml") {
                var requestHeaders = environment["owin.RequestHeaders"] as IDictionary<string, string[]>;
                var responseHeaders = environment["owin.ResponseHeaders"] as IDictionary<string, string[]>;
                if (requestHeaders["If-Modified-Since"]?[0] == lastModified &&
                    requestHeaders["If-None-Match"]?[0] == etag) {
                    environment["owin.ResponseStatusCode"] = 304;
                }
                else if (CrossDomainXmlFile != null) {
                    responseHeaders.Add("Last-Modified", new string[] { lastModified });
                    responseHeaders.Add("Etag", new string[] { etag });
                    responseHeaders.Add("Content-Type", new string[] { "text/xml" });
                    using var fileStream = new FileStream(CrossDomainXmlFile, FileMode.Open, FileAccess.Read);
                    using var outputStream = GetOutputStream(environment);
                    await fileStream.CopyToAsync(outputStream).ConfigureAwait(false);
                }
                else {
                    environment["owin.ResponseStatusCode"] = 404;
                }
                return true;
            }
            return false;
        }
        private async Task<bool> ClientAccessPolicyXmlHandler(IDictionary<string, object> environment) {
            if ((environment["owin.RequestPath"] as string)?.ToLowerInvariant() == "/clientaccesspolicy.xml") {
                var requestHeaders = environment["owin.RequestHeaders"] as IDictionary<string, string[]>;
                var responseHeaders = environment["owin.ResponseHeaders"] as IDictionary<string, string[]>;
                if (requestHeaders["If-Modified-Since"]?[0] == lastModified &&
                    requestHeaders["If-None-Match"]?[0] == etag) {
                    environment["owin.ResponseStatusCode"] = 304;
                }
                else if (ClientAccessPolicyXmlFile != null) {
                    responseHeaders.Add("Last-Modified", new string[] { lastModified });
                    responseHeaders.Add("Etag", new string[] { etag });
                    responseHeaders.Add("Content-Type", new string[] { "text/xml" });
                    using var fileStream = new FileStream(ClientAccessPolicyXmlFile, FileMode.Open, FileAccess.Read);
                    using var outputStream = GetOutputStream(environment);
                    await fileStream.CopyToAsync(outputStream).ConfigureAwait(false);
                }
                else {
                    environment["owin.ResponseStatusCode"] = 404;
                }
                return true;
            }
            return false;
        }
        public static IPEndPoint GetRemoteEndPoint(IDictionary<string, object> environment) {
            var ip = IPAddress.Parse(environment["server.RemoteIpAddress"] as string);
            if (int.TryParse(environment["server.RemotePort"] as string, out int port)) {
                return new IPEndPoint(ip, port);
            }
            return new IPEndPoint(ip, 0);
        }
        public static IPEndPoint GetLocalEndPoint(IDictionary<string, object> environment) {
            var ip = IPAddress.Parse(environment["server.LocalIpAddress"] as string);
            if (int.TryParse(environment["server.LocalPort"] as string, out int port)) {
                return new IPEndPoint(ip, port);
            }
            return new IPEndPoint(ip, 0);
        }
        public virtual async Task Handler(IDictionary<string, object> environment) {
            var context = new ServiceContext(Service);
            context["owin"] = environment;
            context.RemoteEndPoint = GetRemoteEndPoint(environment);
            context.LocalEndPoint = GetLocalEndPoint(environment);
            context.Handler = this;
            var requestHeaders = environment["owin.RequestHeaders"] as IDictionary<string, string[]>;
            var headers = new NameValueCollection(StringComparer.InvariantCultureIgnoreCase);
            foreach (var header in requestHeaders) {
                foreach (var value in header.Value) {
                    headers.Add(header.Key, value);
                }
            }
            context["httpRequestHeaders"] = headers;
            string method = environment["owin.RequestMethod"] as string;
            if (method == "GET") {
                if (await ClientAccessPolicyXmlHandler(environment).ConfigureAwait(false)) {
                    return;
                }
                if (await CrossDomainXmlHandler(environment).ConfigureAwait(false)) {
                    return;
                }
                if (!Get) {
                    environment["owin.ResponseStatusCode"] = 403;
                    environment["owin.ResponseReasonPhrase"] = "Forbidden";
                    return;
                }
            }
            using var instream = (environment["owin.RequestBody"] as Stream) ?? Stream.Null;
            if (instream.Length > Service.MaxRequestLength) {
                environment["owin.ResponseStatusCode"] = 413;
                environment["owin.ResponseReasonPhrase"] = "Request Entity Too Large";
                return;
            }
            try {
                using var outstream = await Service.Handle(instream, context).ConfigureAwait(false);
                SendHeader(environment, context);
                using var outputStream = GetOutputStream(environment);
                await outstream.CopyToAsync(outputStream).ConfigureAwait(false);
            }
            catch (Exception e) {
                environment["owin.ResponseStatusCode"] = 500;
                using var outputStream = GetOutputStream(environment);
                var stackTrace = Encoding.UTF8.GetBytes(e.StackTrace);
                await outputStream.WriteAsync(stackTrace, 0, stackTrace.Length).ConfigureAwait(false);
                return;
            }
        }
    }
}
