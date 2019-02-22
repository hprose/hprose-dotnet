/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  WebSocketTransport.cs                                   |
|                                                          |
|  WebSocketTransport class for C#.                        |
|                                                          |
|  LastModified: Feb 23, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/
#if !NET35_CF && !NET40
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class WebSocketTransport : ITransport {
        public static string[] Schemes { get; } = new string[] { "ws", "wss" };
        private volatile int counter = 0;
        public X509CertificateCollection ClientCertificates { get; set; } = null;
        public CookieContainer Cookies { get; set; } = null;
        public ICredentials Credentials { get; set; } = null;
        public IWebProxy Proxy { get; set; } = null;
        public TimeSpan KeepAliveInterval { get; set; } = default;
        public bool UseDefaultCredentials { get; set; } = true;
        public int ReceiveBufferSize { get; set; } = 16384;
        public int SendBufferSize { get; set; } = 16384;
        private ConcurrentDictionary<Uri, ConcurrentQueue<(int, MemoryStream)>> Requests { get; } = new ConcurrentDictionary<Uri, ConcurrentQueue<(int, MemoryStream)>>();
        private ConcurrentDictionary<Uri, ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>> Results { get; } = new ConcurrentDictionary<Uri, ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>>();
        private ConcurrentDictionary<Uri, ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>> Sended { get; } = new ConcurrentDictionary<Uri, ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>>();
        private ConcurrentDictionary<Uri, Lazy<Task<ClientWebSocket>>> WebSockets { get; } = new ConcurrentDictionary<Uri, Lazy<Task<ClientWebSocket>>>();
        private readonly Func<Uri, Lazy<Task<ClientWebSocket>>> webSocketFactory;
        public WebSocketTransport() {
            webSocketFactory = (uri) => {
                return new Lazy<Task<ClientWebSocket>>(async () => {
                    try {
                        var webSocket = new ClientWebSocket();
                        var options = webSocket.Options;
                        options.AddSubProtocol("hprose");
                        options.UseDefaultCredentials = UseDefaultCredentials;
                        options.SetBuffer(ReceiveBufferSize, SendBufferSize);
                        if (ClientCertificates != null) {
                            options.ClientCertificates = ClientCertificates;
                        }
                        if (Cookies != null) {
                            options.Cookies = Cookies;
                        }
                        if (Credentials != null) {
                            options.Credentials = Credentials;
                        }
                        if (Proxy != null) {
                            options.Proxy = Proxy;
                        }
                        if (KeepAliveInterval > TimeSpan.Zero) {
                            options.KeepAliveInterval = KeepAliveInterval;
                        }
                        await webSocket.ConnectAsync(uri, CancellationToken.None);
                        Receive(uri, webSocket);
                        return webSocket;
                    }
                    catch (Exception e) {
                        var requests = Requests[uri];
                        var sended = Sended[uri];
                        var results = Results[uri];
                        if (requests != null) {
                            while (!requests.IsEmpty) {
                                if (requests.TryDequeue(out (int index, MemoryStream stream) request)) {
                                    if (sended.TryRemove(request.index, out var result)) {
                                        result.TrySetException(e);
                                    }
                                    if (results.TryRemove(request.index, out result)) {
                                        result.TrySetException(e);
                                    }
                                }
                            }
                        }
                        throw;
                    }
                });
            };
        }
        public async void Abort() {
            foreach (var LazyWebSocket in WebSockets.Values) {
                try {
                    var webSocket = await LazyWebSocket.Value.ConfigureAwait(false);
                    webSocket.Abort();
                    webSocket.Dispose();
                }
                catch { }
            }
        }
        private async Task<(int, MemoryStream)> ReadAsync(ClientWebSocket webSocket) {
            MemoryStream stream = new MemoryStream();
            var buffer = ArrayPool<byte>.Shared.Rent(ReceiveBufferSize);
            var index = -1;
            try {
                while (true) {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).ConfigureAwait(false);
                    if (result.CloseStatus != null) {
                        throw new WebSocketException((int)result.CloseStatus, result.CloseStatusDescription);
                    }
                    if (index < 0) {
                        index = (buffer[0] << 24) | (buffer[1] << 16) | (buffer[2] << 8) | buffer[3];
                        stream.Write(buffer, 4, result.Count - 4);
                    }
                    else {
                        stream.Write(buffer, 0, result.Count);
                    }
                    if (result.EndOfMessage) {
                        stream.Position = 0;
                        return (index, stream);
                    }
                }
            }
            finally {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
        private async void Receive(Uri uri, ClientWebSocket webSocket) {
            var sended = Sended[uri];
            try {
                while (true) {
                    var (index, response) = await ReadAsync(webSocket).ConfigureAwait(false);
                    bool has_error = (index & 0x80000000) != 0;
                    index &= 0x7FFFFFFF;
                    if (sended.TryRemove(index, out var result)) {
                        if (has_error) {
                            var buffer = response.GetArraySegment();
                            result.TrySetException(new Exception(Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count)));
                            throw new WebSocketException(WebSocketError.Faulted);
                        }
                        result.TrySetResult(response);
                    }
                }
            }
            catch (Exception e) {
                foreach (var index in sended.Keys) {
                    if (sended.TryRemove(index, out var result)) {
                        result.TrySetException(e);
                    }
                }
                webSocket.Abort();
                webSocket.Dispose();
            }
        }
        private async Task Send(Uri uri, int index, MemoryStream stream) {
            int n = (int)stream.Length;
            var buffer = ArrayPool<byte>.Shared.Rent(4 + n);
            ClientWebSocket webSocket = null;
            try {
                webSocket = await GetWebSocket(uri).ConfigureAwait(false);
                buffer[0] = (byte)(index >> 24 & 0xFF);
                buffer[1] = (byte)(index >> 16 & 0xFF);
                buffer[2] = (byte)(index >> 8 & 0xFF);
                buffer[3] = (byte)(index & 0xFF);
                stream.Read(buffer, 4, n);
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, 4 + n), WebSocketMessageType.Binary, true, CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception e) {
                var sended = Sended[uri];
                foreach (var i in sended.Keys) {
                    if (sended.TryRemove(i, out var value)) {
                        value.TrySetException(e);
                    }
                }
                if (webSocket != null) {
                    webSocket.Abort();
                    webSocket.Dispose();
                }
            }
            finally {
                stream.Dispose();
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
        private async void Send(Uri uri) {
            var requests = Requests[uri];
            var results = Results[uri];
            var sended = Sended.GetOrAdd(uri, (_) => new ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>());
            while (!requests.IsEmpty) {
                requests.TryPeek(out (int index, MemoryStream stream) request);
                var index = request.index;
                if (results.TryGetValue(index, out var result)) {
                    if (!(sended.TryAdd(index, result) && results.TryRemove(index, out result))) {
                        await Task.Yield();
                        continue;
                    }
                }
                else {
                    return;
                }
                await Send(uri, index, request.stream).ConfigureAwait(false);
                requests.TryDequeue(out request);
            }
        }
        private async Task<ClientWebSocket> GetWebSocket(Uri uri) {
            int retried = 0;
            while(true) {
                var LazyWebSocket = WebSockets.GetOrAdd(uri, webSocketFactory);
                var webSocket = await LazyWebSocket.Value.ConfigureAwait(false);
                if (webSocket.CloseStatus == null && webSocket.State == WebSocketState.Open) {
                    return webSocket;
                }
                WebSockets.TryRemove(uri, out LazyWebSocket);
                webSocket.Abort();
                webSocket.Dispose();
                if (++retried > 1) {
                    throw new WebSocketException(WebSocketError.Faulted);
                }
            }
        }
        public async Task<Stream> Transport(Stream request, Context context) {
            var stream = await request.ToMemoryStream().ConfigureAwait(false);
            var uri = (context as ClientContext).Uri;
            var index = Interlocked.Increment(ref counter) & 0x7FFFFFFF;
            var results = Results.GetOrAdd(uri, (_) => new ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>());
            var result = new TaskCompletionSource<MemoryStream>();
            results[index] = result;
            var requests = Requests.GetOrAdd(uri, (_) => new ConcurrentQueue<(int, MemoryStream)>());
            requests.Enqueue((index, stream));
            requests.TryPeek(out (int index, MemoryStream stream) first);
            if (first.index == index) {
                Send(uri);
            }
            return await result.Task.ConfigureAwait(false);
        }
    }
}
#endif