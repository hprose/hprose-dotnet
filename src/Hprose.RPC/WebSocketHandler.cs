/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  WebSocketHandler.cs                                     |
|                                                          |
|  WebSocketHandler class for C#.                          |
|                                                          |
|  LastModified: Mar 20, 2019                              |
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class WebSocketHandler : HttpHandler {
        public event Action<WebSocket> OnAccept;
        public event Action<WebSocket> OnClose;
        public override event Action<Exception> OnError;
        public WebSocketHandler(Service service) : base(service) { }
        private static async Task Send(WebSocket webSocket, ConcurrentQueue<(int index, MemoryStream stream)> responses) {
            while (true) {
                (int index, MemoryStream stream) response;
                while (!responses.TryDequeue(out response)) {
                    await Task.Yield();
                }
                int index = response.index;
                var stream = response.stream;
                int n = (int)stream.Length;
                var buffer = ArrayPool<byte>.Shared.Rent(4 + n);
                try {
                    buffer[0] = (byte)(index >> 24 & 0xFF);
                    buffer[1] = (byte)(index >> 16 & 0xFF);
                    buffer[2] = (byte)(index >> 8 & 0xFF);
                    buffer[3] = (byte)(index & 0xFF);
                    stream.Read(buffer, 4, n);
                    await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, 4 + n), WebSocketMessageType.Binary, true, CancellationToken.None).ConfigureAwait(false);
                    if ((index & 0x80000000) != 0) {
                        var data = stream.GetArraySegment();
                        var message = Encoding.UTF8.GetString(data.Array, data.Offset, data.Count);
                        throw new Exception(message);
                    }
                }
                finally {
                    stream.Dispose();
                    ArrayPool<byte>.Shared.Return(buffer);
                }
            }
        }
        private async void Run(ConcurrentQueue<(int index, MemoryStream stream)> responses, int index, MemoryStream request, Context context) {
            MemoryStream response = null;
            try {
                response = await (await Service.Handle(request, context).ConfigureAwait(false)).ToMemoryStream().ConfigureAwait(false);
            }
            catch (Exception e) {
                index = (int)(index | 0x80000000);
                var bytes = Encoding.UTF8.GetBytes(e.Message);
                response = new MemoryStream(bytes, 0, bytes.Length, false, true);
            }
            finally {
                responses.Enqueue((index, response));
                request.Dispose();
            }
        }
        private async Task<(int, MemoryStream)> ReadAsync(WebSocket webSocket, ConcurrentQueue<(int index, MemoryStream stream)> responses) {
            var stream = new MemoryStream();
            var buffer = ArrayPool<byte>.Shared.Rent(16384);
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
                    if (stream.Length > Service.MaxRequestLength) {
                        var data = stream.GetArraySegment();
                        var bytes = Encoding.UTF8.GetBytes("Request entity too large");
                        responses.Enqueue(((int)(index | 0x80000000), new MemoryStream(bytes, 0, bytes.Length, false, true)));
                        return (index, null);
                    }
                    if (result.EndOfMessage) {
                        if (index < 0) {
                            throw new IOException("Invalid request");
                        }
                        stream.Position = 0;
                        return (index, stream);
                    }
                }
            }
            finally {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
        public async Task Receive(WebSocket webSocket, ServiceContext context, ConcurrentQueue<(int index, MemoryStream stream)> responses) {
            while (true) {
                var (index, stream) = await ReadAsync(webSocket, responses).ConfigureAwait(false);
                if (stream == null) return;
                Run(responses, index, stream, context.Clone() as Context);
            }
        }
        public override async void Handler(HttpListenerContext httpContext) {
            try {
                var request = httpContext.Request;
                if (!request.IsWebSocketRequest) {
                    base.Handler(httpContext);
                    return;
                }
                var webSocketContext = await httpContext.AcceptWebSocketAsync("hprose").ConfigureAwait(false);
                var webSocket = webSocketContext.WebSocket;
                var context = new ServiceContext(Service);
                context["httpContext"] = httpContext;
                context["webSocketContext"] = webSocketContext;
                context["request"] = request;
                context["response"] = httpContext.Response;
                context["user"] = httpContext.User;
                context.RemoteEndPoint = request.RemoteEndPoint;
                context.LocalEndPoint = request.LocalEndPoint;
                context.Handler = this;
                var responses = new ConcurrentQueue<(int index, MemoryStream stream)>();
                OnAccept?.Invoke(webSocket);
                var receive = Receive(webSocket, context, responses);
                var send = Send(webSocket, responses);
                try {
                    await receive.ConfigureAwait(false);
                    await send.ConfigureAwait(false);
                }
                catch (Exception e) {
                    OnError?.Invoke(e);
                    webSocket.Abort();
                    OnClose?.Invoke(webSocket);
                }
            }
            catch (Exception e) {
                OnError?.Invoke(e);
            }
        }
    }
}
#endif