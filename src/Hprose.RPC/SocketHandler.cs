/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  SocketHandler.cs                                        |
|                                                          |
|  SocketHandler class for C#.                             |
|                                                          |
|  LastModified: Mar 8, 2020                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/
#if !NET35_CF && !NET40 && !NET45 && !NET451 && !NET452 && !NET46 && !NET461 && !NET462 && !NET47
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class SocketHandler : IHandler<Socket> {
        public event Action<Socket> OnAccept;
        public event Action<Socket> OnClose;
        public event Action<Exception> OnError;
        public Service Service { get; private set; }
        public SocketHandler(Service service) {
            Service = service;
        }
        public Task Bind(Socket server) {
            return Task.Factory.StartNew(async () => {
                while (true) {
                    try {
                        Handler(await server.AcceptAsync().ConfigureAwait(false));
                    }
                    catch (InvalidOperationException) {
                        return;
                    }
                    catch (SocketException) {
                        return;
                    }
                    catch (Exception error) {
                        OnError?.Invoke(error);
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }
        private static async Task<byte[]> ReadAsync(Socket socket, byte[] bytes, int offset, int length) {
            while (length > 0) {
                var buffer = new ArraySegment<byte>(bytes, offset, length);
                int size = await socket.ReceiveAsync(buffer, SocketFlags.None).ConfigureAwait(false);
                if (size == 0) throw new EndOfStreamException();
                offset += size;
                length -= size;
            }
            return bytes;
        }
        private async Task Send(Socket socket, ConcurrentQueue<(int index, MemoryStream stream)> responses, AutoResetEvent autoResetEvent) {
            var header = new byte[12];
            while (true) {
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
                var stream = response.stream;
                var n = (int)stream.Length;
                header[4] = (byte)(n >> 24 & 0xFF | 0x80);
                header[5] = (byte)(n >> 16 & 0xFF);
                header[6] = (byte)(n >> 8 & 0xFF);
                header[7] = (byte)(n & 0xFF);
                header[8] = (byte)(index >> 24 & 0xFF);
                header[9] = (byte)(index >> 16 & 0xFF);
                header[10] = (byte)(index >> 8 & 0xFF);
                header[11] = (byte)(index & 0xFF);
                var crc32 = CRC32.Compute(header, 4, 8);
                header[0] = (byte)(crc32 >> 24 & 0xFF);
                header[1] = (byte)(crc32 >> 16 & 0xFF);
                header[2] = (byte)(crc32 >> 8 & 0xFF);
                header[3] = (byte)(crc32 & 0xFF);
                await socket.SendAsync(new ArraySegment<byte>[] { new ArraySegment<byte>(header), stream.GetArraySegment() }, SocketFlags.None).ConfigureAwait(false);
                if ((index & 0x80000000) != 0) {
                    var data = (stream as MemoryStream).GetArraySegment();
                    var message = Encoding.UTF8.GetString(data.Array, data.Offset, data.Count);
                    stream.Dispose();
                    throw new Exception(message);
                }
                stream.Dispose();
            }
        }
        private async void Run(ConcurrentQueue<(int index, MemoryStream stream)> responses, int index, byte[] data, Context context, AutoResetEvent autoResetEvent) {
            using (var request = new MemoryStream(data, 0, data.Length, false, true)) {
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
                }
            }
        }
        public async Task Receive(Socket socket, ConcurrentQueue<(int index, MemoryStream stream)> responses, AutoResetEvent autoResetEvent) {
            var header = new byte[12];
            while (true) {
                await ReadAsync(socket, header, 0, 12).ConfigureAwait(false);
                uint crc = (uint)((header[0] << 24) | (header[1] << 16) | (header[2] << 8) | header[3]);
                if (CRC32.Compute(header, 4, 8) != crc || (header[4] & 0x80) == 0 || (header[8] & 0x80) != 0) {
                    throw new IOException("Invalid request");
                }
                int length = ((header[4] & 0x7F) << 24) | (header[5] << 16) | (header[6] << 8) | header[7];
                int index = (header[8] << 24) | (header[9] << 16) | (header[10] << 8) | header[11];
                if (length > Service.MaxRequestLength) {
                    var bytes = Encoding.UTF8.GetBytes("Request entity too large");
                    responses.Enqueue(((int)(index | 0x80000000), new MemoryStream(bytes, 0, bytes.Length, false, true)));
                    try {
                        autoResetEvent.Set();
                    }
                    catch (Exception) { }
                    return;
                }
                var data = await ReadAsync(socket, new byte[length], 0, length).ConfigureAwait(false);
                var context = new ServiceContext(Service);
                context["socket"] = socket;
                context.RemoteEndPoint = socket.RemoteEndPoint;
                context.LocalEndPoint = socket.LocalEndPoint;
                context.Handler = this;
                Run(responses, index, data, context, autoResetEvent);
            }
        }
        private async void Handler(Socket socket) {
            try {
                var responses = new ConcurrentQueue<(int index, MemoryStream stream)>();
                OnAccept?.Invoke(socket);
                using (var autoResetEvent = new AutoResetEvent(false)) {
                    var receive = Receive(socket, responses, autoResetEvent);
                    var send = Send(socket, responses, autoResetEvent);
                    await receive.ConfigureAwait(false);
                    await send.ConfigureAwait(false);
                }
            }
            catch (Exception e) {
                OnError?.Invoke(e);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                OnClose?.Invoke(socket);
            }
        }
    }
}

#endif