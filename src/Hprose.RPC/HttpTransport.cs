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
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

#if !NET35_CF
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class HttpTransport : ITransport, IDisposable {
        public static string[] Schemes { get; } = new string[] { "http", "https" };
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
        public Task Abort() {
            httpClient.CancelPendingRequests();
#if NET40
            return TaskEx.FromResult<object>(null);
#elif NET45 || NET451 || NET452
            return Task.FromResult<object>(null);
#else
            return Task.CompletedTask;
#endif
        }
        public async Task<Stream> Transport(Stream request, Context context) {
            var clientContext = context as ClientContext;
            using (var httpContext = new StreamContent(request)) {
                if (context.Contains("httpRequestHeaders")) {
                    var headers = context["httpRequestHeaders"] as HttpContentHeaders;
                    foreach (var pair in headers) {
                        httpContext.Headers.Add(pair.Key, pair.Value);
                    }
                }
                if (request.CanSeek) {
                    httpContext.Headers.ContentLength = request.Length;
                }
                var response = await httpClient.PostAsync(clientContext.Uri, httpContext).ConfigureAwait(false);
                if (response.IsSuccessStatusCode) {
                    context["httpResponseHeaders"] = response.Headers;
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
#endif