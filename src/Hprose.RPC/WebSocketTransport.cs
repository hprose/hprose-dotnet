﻿/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  WebSocketTransport.cs                                   |
|                                                          |
|  WebSocketTransport class for C#.                        |
|                                                          |
|  LastModified: Jul 2, 2020                               |
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
        private ConcurrentDictionary<ClientWebSocket, ConcurrentQueue<(int, MemoryStream)>> Requests { get; } = new();
        private ConcurrentDictionary<ClientWebSocket, ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>> Results { get; } = new();
        private ConcurrentDictionary<ClientWebSocket, byte> Lock { get; } = new();
        private ConcurrentDictionary<ClientWebSocket, Uri> Uris { get; } = new();
        private ConcurrentDictionary<Uri, Lazy<Task<ClientWebSocket>>> WebSockets { get; } = new();
        private readonly Func<Uri, Lazy<Task<ClientWebSocket>>> webSocketFactory;
        public WebSocketTransport() {
            webSocketFactory = (uri) => {
                return new Lazy<Task<ClientWebSocket>>(async () => {
                    ClientWebSocket webSocket = null;
                    try {
                        webSocket = new ClientWebSocket();
                        Uris.TryAdd(webSocket, uri);
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
                        await webSocket.ConnectAsync(uri, CancellationToken.None).ConfigureAwait(false);
                        Requests.TryAdd(webSocket, new ConcurrentQueue<(int, MemoryStream)>());
                        Results.TryAdd(webSocket, new ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>());
                        Receive(webSocket);
                        return webSocket;
                    }
                    catch (Exception e) {
                        if (webSocket != null) {
                            await Close(webSocket, e).ConfigureAwait(false);
                        }
                        throw;
                    }
                });
            };
        }
        private async Task Close(ClientWebSocket webSocket, Exception e) {
            try {
                if (e.InnerException != null) {
                    e = e.InnerException;
                }
                if (Uris.TryRemove(webSocket, out var uri)) {
                    WebSockets.TryGetValue(uri, out var value);
                    if (await value.Value.ConfigureAwait(false) == webSocket) {
                        WebSockets.TryRemove(uri, out _);
                    }
                }
                Requests.TryRemove(webSocket, out var requests);
                if (Results.TryRemove(webSocket, out var results)) {
                    while (!results.IsEmpty) {
                        foreach (var i in results.Keys) {
                            if (results.TryRemove(i, out var result)) {
                                result.TrySetException(e);
                            }
                        }
                    }
                }
                webSocket.Abort();
                webSocket.Dispose();
            }
            catch { }
        }
        private async Task<(int, MemoryStream)> ReadAsync(ClientWebSocket webSocket) {
            var stream = new MemoryStream();
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
        private async void Receive(ClientWebSocket webSocket) {
            var results = Results[webSocket];
            try {
                while (true) {
                    var (index, response) = await ReadAsync(webSocket).ConfigureAwait(false);
                    bool has_error = (index & 0x80000000) != 0;
                    index &= 0x7FFFFFFF;
                    if (results.TryRemove(index, out var result)) {
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
                await Close(webSocket, e).ConfigureAwait(false);
            }
        }
        private async void Send(ClientWebSocket webSocket) {
            if (!Lock.TryAdd(webSocket, default)) return;
            try {
                var requests = Requests[webSocket];
                while (requests.TryDequeue(out var request)) {
                    var (index, stream) = request;
                    int n = (int)stream.Length;
                    var buffer = ArrayPool<byte>.Shared.Rent(4 + n);
                    try {
                        buffer[0] = (byte)(index >> 24 & 0xFF);
                        buffer[1] = (byte)(index >> 16 & 0xFF);
                        buffer[2] = (byte)(index >> 8 & 0xFF);
                        buffer[3] = (byte)(index & 0xFF);
                        stream.Read(buffer, 4, n);
                        await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, 4 + n), WebSocketMessageType.Binary, true, CancellationToken.None).ConfigureAwait(false);
                    }
                    catch (Exception e) {
                        await Close(webSocket, e).ConfigureAwait(false);
                    }
                    finally {
                        stream.Dispose();
                        ArrayPool<byte>.Shared.Return(buffer);
                    }
                }
            }
            finally {
                Lock.TryRemove(webSocket, out var _);
            }
        }
        private Task<ClientWebSocket> GetWebSocket(Uri uri) {
            return WebSockets.GetOrAdd(uri, webSocketFactory).Value;
        }
        public async Task<Stream> Transport(Stream request, Context context) {
            var stream = await request.ToMemoryStream().ConfigureAwait(false);
            var clientContext = context as ClientContext;
            var index = Interlocked.Increment(ref counter) & 0x7FFFFFFF;
            var webSocket = await GetWebSocket(clientContext.Uri).ConfigureAwait(false);
            var results = Results[webSocket];
            var result = new TaskCompletionSource<MemoryStream>();
            results[index] = result;
            var requests = Requests[webSocket];
            requests.Enqueue((index, stream));
            Send(webSocket);
            var timeout = clientContext.Timeout;
            if (timeout > TimeSpan.Zero) {
                using CancellationTokenSource source = new CancellationTokenSource();
                var timer = Task.Delay(timeout, source.Token);
                var task = await Task.WhenAny(timer, result.Task).ConfigureAwait(false);
                source.Cancel();
                if (task == timer) {
                    await Close(webSocket, new TimeoutException()).ConfigureAwait(false);
                }
            }
            return await result.Task.ConfigureAwait(false);
        }
        public async Task Abort() {
            foreach (var LazyWebSocket in WebSockets.Values) {
                try {
                    var webSocket = await LazyWebSocket.Value.ConfigureAwait(false);
                    await Close(webSocket, new WebSocketException(WebSocketError.ConnectionClosedPrematurely)).ConfigureAwait(false);
                }
                catch { }
            }
        }
    }
}
#endif