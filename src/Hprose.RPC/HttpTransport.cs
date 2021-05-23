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
|  LastModified: Mar 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

#if !NET35_CF
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class HttpTransport : ITransport, IDisposable {
        public static string[] Schemes { get; } = new string[] { "http", "https" };
        private readonly HttpClient httpClient = new();
        public NameValueCollection HttpRequestHeaders { get; } = new(StringComparer.InvariantCultureIgnoreCase);
        public HttpTransport() {
            httpClient.Timeout = new TimeSpan(0, 0, 0, 0, Timeout.Infinite);
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
            using var httpContext = new StreamContent(request);
            foreach (var header in HttpRequestHeaders.AllKeys) {
                httpContext.Headers.Add(header, HttpRequestHeaders.GetValues(header));
            }
            if (context.Contains("httpRequestHeaders")) {
                var headers = context["httpRequestHeaders"] as NameValueCollection;
                foreach (var header in headers.AllKeys) {
                    httpContext.Headers.Add(header, headers.GetValues(header));
                }
            }
            if (request.CanSeek) {
                httpContext.Headers.ContentLength = request.Length;
            }
            HttpResponseMessage response;
            var timeout = clientContext.Timeout;
            if (timeout > TimeSpan.Zero) {
                using CancellationTokenSource source = new();
                var responseTask = httpClient.PostAsync(clientContext.Uri, httpContext, source.Token);
#if NET40
                var timer = TaskEx.Delay(timeout, source.Token);
                var task = await TaskEx.WhenAny(timer, responseTask).ConfigureAwait(false);
#else
                var timer = Task.Delay(timeout, source.Token);
                var task = await Task.WhenAny(timer, responseTask).ConfigureAwait(false);
#endif
                source.Cancel();
                if (task == timer) {
                    throw new TimeoutException();
                }
                response = await responseTask.ConfigureAwait(false);
            }
            else {
                response = await httpClient.PostAsync(clientContext.Uri, httpContext).ConfigureAwait(false);
            }
            context["httpStatusCode"] = (int)response.StatusCode;
            context["httpStatusText"] = response.ReasonPhrase;
            if (response.IsSuccessStatusCode) {
                var headers = new NameValueCollection(StringComparer.InvariantCultureIgnoreCase);
                foreach (var header in response.Headers) {
                    foreach (var value in header.Value) {
                        headers.Add(header.Key, value);
                    }
                }
                context["httpResponseHeaders"] = headers;
                return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }
            else {
                throw new Exception(((int)response.StatusCode) + ":" + response.ReasonPhrase);
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