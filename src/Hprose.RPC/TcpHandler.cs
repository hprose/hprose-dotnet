/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  TcpHandler.cs                                           |
|                                                          |
|  TcpHandler class for C#.                                |
|                                                          |
|  LastModified: Feb 8, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class TcpHandler : IHandler<TcpListener> {
        public Action<TcpClient> OnAccept { get; set; } = null;
        public Action<TcpClient> OnClose { get; set; } = null;
        public Action<Exception> OnError { get; set; } = null;
        public Service Service { get; private set; }
        public TcpHandler(Service service) {
            Service = service;
        }
        public async Task Bind(TcpListener server) {
            while (true) {
                try {
                    Handler(await server.AcceptTcpClientAsync().ConfigureAwait(false));
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
        }
        private static async Task<byte[]> ReadAsync(Stream stream, byte[] bytes, int offset, int length) {
            while (length > 0) {
                int size = await stream.ReadAsync(bytes, offset, length).ConfigureAwait(false);
                offset += size;
                length -= size;
            }
            return bytes;
        }
        private async Task Send(TcpClient tcpClient, ConcurrentQueue<(int index, Stream stream)> responses) {
            var header = new byte[12];
            var netStream = tcpClient.GetStream();
            while (true) {
                (int index, Stream stream) response;
                while (!responses.TryDequeue(out response)) {
#if NET40
                    await TaskEx.Yield();
#else
                    await Task.Yield();
#endif
                }
                int index = response.index;
                Stream stream = response.stream;
                try {
                    if (!stream.CanSeek) {
                        var memstream = new MemoryStream();
                        await stream.CopyToAsync(memstream).ConfigureAwait(false);
                        memstream.Position = 0;
                        stream.Dispose();
                        stream = memstream;
                    }
                }
                catch (Exception e) {
                    OnError?.Invoke(e);
                    continue;
                }
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
                await netStream.WriteAsync(header, 0, 12).ConfigureAwait(false);
                await stream.CopyToAsync(netStream).ConfigureAwait(false);
                await netStream.FlushAsync().ConfigureAwait(false);
                if ((index & 0x80000000) != 0) {
                    var data = (stream as MemoryStream).GetArraySegment();
                    var message = Encoding.UTF8.GetString(data.Array, data.Offset, data.Count);
                    stream.Dispose();
                    throw new Exception(message);
                }
                stream.Dispose();
            }
        }
        private async void Run(ConcurrentQueue<(int index, Stream stream)> responses, int index, byte[] data, Context context) {
            using (var request = new MemoryStream(data, 0, data.Length, false, true)) {
                Stream response = null;
                try {
                    response = await Service.Handle(request, context).ConfigureAwait(false);
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
        public async Task Receive(TcpClient tcpClient, ConcurrentQueue<(int index, Stream stream)> responses) {
            var header = new byte[12];
            var netStream = tcpClient.GetStream();
            while (true) {
                await ReadAsync(netStream, header, 0, 12).ConfigureAwait(false);
                uint crc = (uint)((header[0] << 24) | (header[1] << 16) | (header[2] << 8) | header[3]);
                if (CRC32.Compute(header, 4, 8) != crc || (header[4] & 0x80) == 0 || (header[8] & 0x80) != 0) {
                    throw new IOException("invalid request");
                }
                int length = ((header[4] & 0x7F) << 24) | (header[5] << 16) | (header[6] << 8) | header[7];
                int index = (header[8] << 24) | (header[9] << 16) | (header[10] << 8) | header[11];
                if (length > Service.MaxRequestLength) {
                    var bytes = Encoding.UTF8.GetBytes("request too long");
                    responses.Enqueue(((int)(index | 0x80000000), new MemoryStream(bytes, 0, bytes.Length, false, true)));
                    return;
                }
                var data = await ReadAsync(netStream, new byte[length], 0, length).ConfigureAwait(false);
                dynamic context = new ServiceContext(Service);
                context.TcpClient = tcpClient;
                context.Socket = tcpClient.Client;
                context.Handler = this;
                Run(responses, index, data, context);
            }
        }
        private async void Handler(TcpClient tcpClient) {
            var responses = new ConcurrentQueue<(int index, Stream stream)>();
            OnAccept?.Invoke(tcpClient);
            var receive = Receive(tcpClient, responses);
            var send = Send(tcpClient, responses);
            try {
                await receive.ConfigureAwait(false);
                await send.ConfigureAwait(false);
            }
            catch (Exception e) {
                OnError?.Invoke(e);
#if NET40 || NET45 || NET451 || NET452
                tcpClient.Close();
#else
                tcpClient.Dispose();
#endif
                OnClose?.Invoke(tcpClient);
            }
        }
    }
}

