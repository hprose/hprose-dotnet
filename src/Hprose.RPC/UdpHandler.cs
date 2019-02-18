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
|  LastModified: Feb 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
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
        private static async void Send(UdpClient udpClient, int index, MemoryStream stream, IPEndPoint endPoint) {
            var n = (int)stream.Length;
#if NET40
            var buffer = new byte[n + 8];
#else
            var buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(n + 8);
#endif
            try {
                buffer[4] = (byte)(n >> 8 & 0xFF);
                buffer[5] = (byte)(n & 0xFF);
                buffer[6] = (byte)(index >> 8 & 0xFF);
                buffer[7] = (byte)(index & 0xFF);
                var crc32 = CRC32.Compute(buffer, 4, 4);
                buffer[0] = (byte)(crc32 >> 24 & 0xFF);
                buffer[1] = (byte)(crc32 >> 16 & 0xFF);
                buffer[2] = (byte)(crc32 >> 8 & 0xFF);
                buffer[3] = (byte)(crc32 & 0xFF);
                stream.Read(buffer, 8, n);
                await udpClient.SendAsync(buffer, n + 8, endPoint).ConfigureAwait(false);
            }
            finally {
                stream.Dispose();
#if !NET40
                System.Buffers.ArrayPool<byte>.Shared.Return(buffer);
#endif
            }
        }
        private static async Task Send(UdpClient udpClient, ConcurrentQueue<(int index, MemoryStream stream, IPEndPoint endPoint)> responses) {
            while (true) {
                (int index, MemoryStream stream, IPEndPoint endPoint) response;
                while (!responses.TryDequeue(out response)) {
#if NET40
                    await TaskEx.Yield();
#else
                    await Task.Yield();
#endif
                }
                Send(udpClient, response.index, response.stream, response.endPoint);
            }
        }
        public async void Process(UdpClient udpClient, UdpReceiveResult result, ConcurrentQueue<(int index, MemoryStream stream, IPEndPoint endPoint)> responses) {
            var buffer = result.Buffer;
            var ipEndPoint = result.RemoteEndPoint;
            uint crc = (uint)((buffer[0] << 24) | (buffer[1] << 16) | (buffer[2] << 8) | buffer[3]);
            int length = (buffer[4] << 8) | buffer[5];
            int index = (buffer[6] << 8) | buffer[7];
            if (CRC32.Compute(buffer, 4, 4) != crc || (length != buffer.Length - 8) || (buffer[6] & 0x80) != 0) {
                return;
            }
            if (length > Service.MaxRequestLength) {
                var bytes = Encoding.UTF8.GetBytes("request too long");
                responses.Enqueue((index | 0x8000, new MemoryStream(bytes), ipEndPoint));
                return;
            }
            var context = new ServiceContext(Service);
            context["udpClient"] = udpClient;
            context["socket"] = udpClient.Client;
            context.RemoteEndPoint = ipEndPoint;
            context.Handler = this;
            using (var request = new MemoryStream(buffer, 8, length, false, true)) {
                try {
                    Stream response = await Service.Handle(request, context).ConfigureAwait(false);
                    responses.Enqueue((index, (await response.ToMemoryStream().ConfigureAwait(false)), ipEndPoint));
                }
                catch (Exception e) {
                    responses.Enqueue((index | 0x8000, new MemoryStream(Encoding.UTF8.GetBytes(e.Message)), ipEndPoint));
                }
            }
        }
        public async Task Receive(UdpClient udpClient, ConcurrentQueue<(int index, MemoryStream stream, IPEndPoint endPoint)> responses) {
            while (true) {
                var result = await udpClient.ReceiveAsync().ConfigureAwait(false);
                Process(udpClient, result, responses);
            }
        }
        private async Task Handler(UdpClient udpClient) {
            var responses = new ConcurrentQueue<(int index, MemoryStream stream, IPEndPoint endPoint)>();
            var receive = Receive(udpClient, responses);
            var send = Send(udpClient, responses);
            try {
                await receive.ConfigureAwait(false);
                await send.ConfigureAwait(false);
            }
            catch (Exception e) {
                OnError?.Invoke(e);
                udpClient.Close();
                OnClose?.Invoke(udpClient);
            }
        }
    }
}