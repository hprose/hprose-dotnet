/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  HttpTransportCF.cs                                      |
|                                                          |
|  HttpTransport class for .NET CF.                        |
|                                                          |
|  LastModified: Feb 27, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/
#if NET35_CF
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class HttpTransport : ITransport, IDisposable {
        public static string[] Schemes { get; } = new string[] { "http", "https" };
        public static bool DisableGlobalCookie { get; set; } = false;
        private static readonly CookieManager globalCookieManager = new CookieManager();
        private readonly CookieManager cookieManager = DisableGlobalCookie ? new CookieManager() : globalCookieManager;
        public Dictionary<string, string> Headers { get; }= new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        public ICredentials Credentials { get; set; } = null;
        public bool KeepAlive { get; set; } = true;
        public int KeepAliveTimeout { get; set; } = 300;
        public IWebProxy Proxy { get; set; } = null;
        public string AcceptEncoding { get; set; } = null;
        public string ConnectionGroupName { get; set; } = null;
        public X509CertificateCollection ClientCertificates { get; set; } = null;
        public ConcurrentDictionary<WebRequest, WebResponse> requests = new ConcurrentDictionary<WebRequest, WebResponse>();
        public Task Abort() {
            foreach (var request in requests) {
                request.Key.Abort();
                request.Value?.Close();
            }
            requests.Clear();
            return Task.CompletedTask;
        }
        public async Task<Stream> Transport(Stream request, Context context) {
            var clientContext = context as ClientContext;
            var uri = clientContext.Uri;
            var timeout = (int)clientContext.Timeout.TotalMilliseconds;
            var webRequest = WebRequest.Create(uri) as HttpWebRequest;
            requests[webRequest] = null;
            try {
                webRequest.Method = "POST";
                webRequest.ContentType = "application/hprose";
                webRequest.Credentials = Credentials;
                webRequest.Timeout = timeout;
                webRequest.SendChunked = false;
                if (AcceptEncoding != null) {
                    webRequest.Headers.Set("Accept-Encoding", AcceptEncoding);
                }
                webRequest.ReadWriteTimeout = timeout;
                webRequest.ProtocolVersion = HttpVersion.Version11;
                if (Proxy != null) {
                    webRequest.Proxy = Proxy;
                }
                webRequest.KeepAlive = KeepAlive;
                if (KeepAlive) {
                    webRequest.Headers.Set("Keep-Alive", KeepAliveTimeout.ToString());
                }
                webRequest.ConnectionGroupName = ConnectionGroupName;
                if (ClientCertificates != null) {
                    webRequest.ClientCertificates = ClientCertificates;
                }
                foreach (var header in Headers) {
                    webRequest.Headers[header.Key] = header.Value;
                }
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Headers.Set("Cookie", cookieManager.GetCookie(uri.Host, uri.AbsolutePath, uri.Scheme == "https"));
                var ostream = await webRequest.GetRequestStreamAsync().ConfigureAwait(false);
                await request.CopyToAsync(ostream).ConfigureAwait(false);
                await ostream.FlushAsync().ConfigureAwait(false);
                ostream.Dispose();
                var webResponse = await webRequest.GetResponseAsync().ConfigureAwait(false) as HttpWebResponse;
                requests[webRequest] = webResponse;
                cookieManager.SetCookie(webResponse.Headers.GetValues("Set-Cookie"), uri.Host);
                cookieManager.SetCookie(webResponse.Headers.GetValues("Set-Cookie2"), uri.Host);
                var istream = webResponse.GetResponseStream();
                string contentEncoding = webResponse.ContentEncoding.ToLower();
                if (contentEncoding.IndexOf("deflate") > -1) {
                    istream = new DeflateStream(istream, CompressionMode.Decompress);
                }
                else if (contentEncoding.IndexOf("gzip") > -1) {
                    istream = new GZipStream(istream, CompressionMode.Decompress);
                }
                int len = (int)webResponse.ContentLength;
                var response = (len > 0) ? new MemoryStream(len) : new MemoryStream();
                await istream.CopyToAsync(response).ConfigureAwait(false);
                istream.Dispose();
                webResponse.Close();
                return response;
            }
            finally {
                requests.Remove(webRequest, out var _);
            }
        }
        private bool disposed = false;
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing) {
            if (disposed) return;
            if (disposing) {
                if (cookieManager != globalCookieManager) {
                    cookieManager.Dispose();
                }
            }
            disposed = true;
        }
    }
}
#endif