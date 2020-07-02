/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  AspNetWebSocketHandler.cs                               |
|                                                          |
|  AspNetWebSocketHandler class for C#.                    |
|                                                          |
|  LastModified: Jul 2, 2020                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/
#if !NET40
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;

namespace Hprose.RPC.AspNet {
    public class AspNetWebSocketHandler : AspNetHttpHandler {
        public event Action<WebSocket> OnAccept;
        public event Action<WebSocket> OnClose;
        public event Action<Exception> OnError;
        public AspNetWebSocketHandler(Service service) : base(service) { }
        private async Task Send(WebSocket webSocket, ConcurrentQueue<(int index, MemoryStream stream)> responses, AutoResetEvent autoResetEvent) {
            while (webSocket.State == WebSocketState.Open) {
                (int index, MemoryStream stream) response;
                while (!responses.TryDequeue(out response)) {
                    await Task.Yield();
                    try {
                        autoResetEvent.WaitOne(1);
                    }
                    catch (Exception) {
                        return;
                    }
                }
                int index = response.index;
                MemoryStream stream = response.stream;
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
        private async void Run(ConcurrentQueue<(int index, MemoryStream stream)> responses, int index, MemoryStream request, Context context, AutoResetEvent autoResetEvent) {
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
                try {
                    autoResetEvent.Set();
                }
                catch (Exception) { }
                request.Dispose();
            }
        }
        private async Task<(int, MemoryStream)> ReadAsync(WebSocket webSocket, ConcurrentQueue<(int index, MemoryStream stream)> responses, AutoResetEvent autoResetEvent) {
            MemoryStream stream = new MemoryStream();
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
                        try {
                            autoResetEvent.Set();
                        }
                        catch (Exception) { }
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
        public async Task Receive(WebSocket webSocket, ServiceContext context, ConcurrentQueue<(int index, MemoryStream stream)> responses, AutoResetEvent autoResetEvent) {
            while (webSocket.State == WebSocketState.Open) {
                var (index, stream) = await ReadAsync(webSocket, responses, autoResetEvent).ConfigureAwait(false);
                if (stream == null) return;
                Run(responses, index, stream, context.Clone() as Context, autoResetEvent);
            }
        }
        public override async Task Handler(HttpContext httpContext) {
            if (!httpContext.IsWebSocketRequest) {
                await base.Handler(httpContext).ConfigureAwait(false);
                return;
            }
            httpContext.AcceptWebSocketRequest(async (webSocketContext) => {
                try {
                    WebSocket webSocket = webSocketContext.WebSocket;
                    try {
                        var request = httpContext.Request;
                        var context = new ServiceContext(Service);
                        context["httpContext"] = httpContext;
                        context["webSocketContext"] = webSocketContext;
                        context["request"] = request;
                        context["response"] = httpContext.Response;
                        context["user"] = httpContext.User;
                        context.RemoteEndPoint = GetRemoteEndPoint(request);
                        context.LocalEndPoint = GetLocalEndPoint(request);
                        context.Handler = this;
                        var responses = new ConcurrentQueue<(int index, MemoryStream stream)>();
                        OnAccept?.Invoke(webSocket);
                        using var autoResetEvent = new AutoResetEvent(false);
                        var receive = Receive(webSocket, context, responses, autoResetEvent);
                        var send = Send(webSocket, responses, autoResetEvent);
                        await receive.ConfigureAwait(false);
                        await send.ConfigureAwait(false);
                    }
                    catch (Exception e) {
                        if (e.InnerException != null) {
                            e = e.InnerException;
                        }
                        try {
                            OnError?.Invoke(e);
                            OnClose?.Invoke(webSocket);
                        }
                        finally {
                            webSocket.Abort();
                        }
                    }
                }
                catch (Exception e) {
                    if (e.InnerException != null) {
                        e = e.InnerException;
                    }
                    OnError?.Invoke(e);
                }
            }, new AspNetWebSocketOptions { SubProtocol = "hprose" });
        }
    }
}
#endif