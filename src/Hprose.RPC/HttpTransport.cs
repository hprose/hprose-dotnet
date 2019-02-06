/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  HttpTransport.cs                                        |
|                                                          |
|  HttpTransport class for C#.                             |
|                                                          |
|  LastModified: Feb 5, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class HttpTransport : ITransport, IDisposable {
        public static ReadOnlyCollection<string> Schemes { get; } = new ReadOnlyCollection<string>(new string[] { "http", "https" });
        private readonly HttpClient httpClient = new HttpClient();
        public HttpRequestHeaders HttpRequestHeaders => httpClient.DefaultRequestHeaders;
        public TimeSpan Timeout {
            get => httpClient.Timeout;
            set => httpClient.Timeout = value;
        }
        public long MaxResponseContentBufferSize {
            get => httpClient.MaxResponseContentBufferSize;
            set => httpClient.MaxResponseContentBufferSize = value;
        }
        public void Abort() {
            httpClient.CancelPendingRequests();
        }
        public async Task<Stream> Transport(Stream request, Context context) {
            dynamic clientContext = context;
            using (HttpContent httpContext = new StreamContent(request)) {
                if (context.Contains("HttpRequestHeaders")) {
                    HttpContentHeaders headers = clientContext.HttpRequestHeaders;
                    foreach (var pair in headers) {
                        httpContext.Headers.Add(pair.Key, pair.Value);
                    }
                }
                if (request.CanSeek) {
                    httpContext.Headers.ContentLength = request.Length;
                }
                HttpResponseMessage response = await httpClient.PostAsync(clientContext.Uri, httpContext).ConfigureAwait(false);
                if (response.IsSuccessStatusCode) {
                    clientContext.HttpResponseHeaders = response.Headers;
                    return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                }
                else {
                    throw new Exception(((int)response.StatusCode) + ":" + response.ReasonPhrase);
                }
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
                httpClient.Dispose();
            }
            disposed = true;
        }
    }
}