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
|  LastModified: Feb 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/
#if !NET40
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
        public Action<WebSocket> OnAccept { get; set; } = null;
        public Action<WebSocket> OnClose { get; set; } = null;
        public WebSocketHandler(Service service) : base(service) { }
        private static async Task Send(WebSocket webSocket, ConcurrentQueue<(int index, MemoryStream stream)> responses) {
            var header = new byte[4];
            while (true) {
                (int index, MemoryStream stream) response;
                while (!responses.TryDequeue(out response)) {
                    await Task.Yield();
                }
                int index = response.index;
                MemoryStream stream = response.stream;
                header[0] = (byte)(index >> 24 & 0xFF);
                header[1] = (byte)(index >> 16 & 0xFF);
                header[2] = (byte)(index >> 8 & 0xFF);
                header[3] = (byte)(index & 0xFF);
                await webSocket.SendAsync(new ArraySegment<byte>(header), WebSocketMessageType.Binary, false, CancellationToken.None).ConfigureAwait(false);
                await webSocket.SendAsync(stream.GetArraySegment(), WebSocketMessageType.Binary, true, CancellationToken.None).ConfigureAwait(false);
                if ((index & 0x80000000) != 0) {
                    var data = (stream as MemoryStream).GetArraySegment();
                    var message = Encoding.UTF8.GetString(data.Array, data.Offset, data.Count);
                    stream.Dispose();
                    throw new Exception(message);
                }
                stream.Dispose();
            }
        }
        private async void Run(ConcurrentQueue<(int index, MemoryStream stream)> responses, int index, ArraySegment<byte> data, Context context) {
            using (var request = new MemoryStream(data.Array, data.Offset, data.Count, false, true)) {
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
                }
            }
        }
        private async Task<MemoryStream> ReadAsync(WebSocket webSocket, ConcurrentQueue<(int index, MemoryStream stream)> responses) {
            MemoryStream stream = new MemoryStream();
            var buffer = ArrayPool<byte>.Shared.Rent(16384);
            try {
                while (true) {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).ConfigureAwait(false);
                    if (result.CloseStatus != null) {
                        throw new WebSocketException((int)result.CloseStatus, result.CloseStatusDescription);
                    }
                    stream.Write(buffer, 0, result.Count);
                    if (stream.Length > Service.MaxRequestLength) {
                        var data = stream.GetArraySegment();
                        int index = (data.Array[data.Offset] << 24) |
                                    (data.Array[data.Offset + 1] << 16) |
                                    (data.Array[data.Offset + 2] << 8) |
                                    data.Array[data.Offset + 3];
                        var bytes = Encoding.UTF8.GetBytes("request too long");
                        responses.Enqueue(((int)(index | 0x80000000), new MemoryStream(bytes, 0, bytes.Length, false, true)));
                        return null;
                    }
                    if (result.EndOfMessage) return stream;
                }
            }
            finally {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
        public async Task Receive(WebSocket webSocket, ServiceContext context, ConcurrentQueue<(int index, MemoryStream stream)> responses) {
            while (true) {
                MemoryStream stream = await ReadAsync(webSocket, responses).ConfigureAwait(false);
                if (stream == null) return;
                var buffer = stream.GetArraySegment();
                int index = (buffer.Array[buffer.Offset] << 24) |
                            (buffer.Array[buffer.Offset + 1] << 16) |
                            (buffer.Array[buffer.Offset + 2] << 8) |
                            buffer.Array[buffer.Offset + 3];
                Run(responses, index, new ArraySegment<byte>(buffer.Array, buffer.Offset + 4, buffer.Count - 4), context.Clone() as Context);
            }
        }
        public override async void Handler(HttpListenerContext httpContext) {
            var request = httpContext.Request;
            if (!request.IsWebSocketRequest) {
                base.Handler(httpContext);
                return;
            }
            var webSocketContext = await httpContext.AcceptWebSocketAsync("hprose").ConfigureAwait(false);
            WebSocket webSocket = webSocketContext.WebSocket;
            dynamic context = new ServiceContext(Service);
            context.WebSocketContext = webSocketContext;
            context.Request = request;
            context.Response = httpContext.Response;
            context.User = httpContext.User;
            context.RemoteEndPoint = request.RemoteEndPoint;
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
    }
}
#endif