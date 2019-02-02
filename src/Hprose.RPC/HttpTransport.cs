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
|  LastModified: Feb 2, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class HttpTransport : ITransport {
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
                httpContext.Headers.ContentLength = request.Length;
                HttpResponseMessage response = await httpClient.PostAsync(clientContext.Uri, httpContext);
                if (response.IsSuccessStatusCode) {
                    clientContext.HttpResponseHeaders = response.Headers;
                    return await response.Content.ReadAsStreamAsync();
                }
                else {
                    throw new Exception(((int)response.StatusCode) + ":" + response.ReasonPhrase);
                }
            }
        }
    }
    public partial class Client {
        public HttpTransport Http => (HttpTransport)this["http"];
        static Client() {
            Register<HttpTransport>("http", new string[] { "http", "https" });
        }
    }
}