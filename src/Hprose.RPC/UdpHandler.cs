/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  UdpHandler.cs                                           |
|                                                          |
|  UdpHandler class for C#.                                |
|                                                          |
|  LastModified: Feb 9, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class UdpHandler : IHandler<UdpClient> {
        public Action<UdpClient> OnClose { get; set; } = null;
        public Action<Exception> OnError { get; set; } = null;
        public Service Service { get; private set; }
        public UdpHandler(Service service) {
            Service = service;
        }
        public async Task Bind(UdpClient server) {
            await Handler(server).ConfigureAwait(false);
        }
        private async Task Send(UdpClient udpClient, ConcurrentQueue<UdpReceiveResult> responses) {
            while (true) {
                UdpReceiveResult response;
                while (!responses.TryDequeue(out response)) {
#if NET40
                    await TaskEx.Yield();
#else
                    await Task.Yield();
#endif
                }
                await udpClient.SendAsync(
                    response.Buffer,
                    response.Buffer.Length,
                    response.RemoteEndPoint
                ).ConfigureAwait(false);
            }
        }
        public static byte[] Encode(int index, ArraySegment<byte> data) {
            var n = data.Count;
            var buffer = new byte[n + 8];
            buffer[4] = (byte)(n >> 8 & 0xFF);
            buffer[5] = (byte)(n & 0xFF);
            buffer[6] = (byte)(index >> 8 & 0xFF);
            buffer[7] = (byte)(index & 0xFF);
            var crc32 = CRC32.Compute(buffer, 4, 4);
            buffer[0] = (byte)(crc32 >> 24 & 0xFF);
            buffer[1] = (byte)(crc32 >> 16 & 0xFF);
            buffer[2] = (byte)(crc32 >> 8 & 0xFF);
            buffer[3] = (byte)(crc32 & 0xFF);
            Buffer.BlockCopy(data.Array, data.Offset, buffer, 8, n);
            return buffer;
        }
        public static bool Decode(byte[] buffer, out int length, out int index) {
            uint crc = (uint)((buffer[0] << 24) | (buffer[1] << 16) | (buffer[2] << 8) | buffer[3]);
            length = (buffer[4] << 8) | buffer[5];
            index = (buffer[6] << 8) | buffer[7];
            if (CRC32.Compute(buffer, 4, 4) != crc ||
                (length != buffer.Length - 8) ||
                (buffer[6] & 0x80) != 0) return false;
            return true;
        }
        public async void Process(UdpClient udpClient, UdpReceiveResult result, ConcurrentQueue<UdpReceiveResult> responses) {
            var buffer = result.Buffer;
            var ipEndPoint = result.RemoteEndPoint;
            if (!Decode(result.Buffer, out int length, out int index)) return;
            if (length > Service.MaxRequestLength) {
                var bytes = Encoding.UTF8.GetBytes("request too long");
                responses.Enqueue(new UdpReceiveResult(Encode(index | 0x8000, new ArraySegment<byte>(bytes)), ipEndPoint));
                return;
            }
            dynamic context = new ServiceContext(Service);
            context.UdpClient = udpClient;
            context.Socket = udpClient.Client;
            context.Handler = this;
            using (var request = new MemoryStream(buffer, 8, length, false, true)) {
                try {
                    Stream response = await Service.Handle(request, context).ConfigureAwait(false);
                    buffer = Encode(index, (await response.ToMemoryStream().ConfigureAwait(false)).GetArraySegment());
                }
                catch (Exception e) {
                    buffer = Encode(index | 0x8000, new ArraySegment<byte>(Encoding.UTF8.GetBytes(e.Message)));
                }
                finally {
                    responses.Enqueue(new UdpReceiveResult(buffer, ipEndPoint));
                }
            }
        }
        public async Task Receive(UdpClient udpClient, ConcurrentQueue<UdpReceiveResult> responses) {
            while (true) {
                var result = await udpClient.ReceiveAsync().ConfigureAwait(false);
                Process(udpClient, result, responses);
            }
        }
        private async Task Handler(UdpClient udpClient) {
            var responses = new ConcurrentQueue<UdpReceiveResult>();
            var receive = Receive(udpClient, responses);
            var send = Send(udpClient, responses);
            try {
                await receive.ConfigureAwait(false);
                await send.ConfigureAwait(false);
            }
            catch (Exception e) {
                OnError?.Invoke(e);
#if NET40 || NET45 || NET451 || NET452
                udpClient.Close();
#else
                udpClient.Dispose();
#endif
                OnClose?.Invoke(udpClient);
            }
        }
    }
}