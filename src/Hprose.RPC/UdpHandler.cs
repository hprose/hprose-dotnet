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
|  LastModified: Mar 8, 2020                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class UdpHandler : IHandler<UdpClient> {
        public event Action<UdpClient> OnClose;
        public event Action<Exception> OnError;
        public Service Service { get; private set; }
        public UdpHandler(Service service) {
            Service = service;
        }
        public Task Bind(UdpClient server) {
            return Task.Factory.StartNew(async () => {
                try {
                    await Handler(server).ConfigureAwait(false);
                }
                catch (Exception e) {
                    OnError?.Invoke(e);
                }
            }, TaskCreationOptions.LongRunning);
        }
        private async Task Send(UdpClient udpClient, ConcurrentQueue<(int index, MemoryStream stream, IPEndPoint endPoint)> responses, AutoResetEvent autoResetEvent) {
            while (true) {
                (int index, MemoryStream stream, IPEndPoint endPoint) response;
                while (!responses.TryDequeue(out response)) {
#if NET40
                    await TaskEx.Yield();
#else
                    await Task.Yield();
#endif
                    try {
                        autoResetEvent.WaitOne(1);
                    }
                    catch (Exception) {
                        return;
                    }
                }
                var (index, stream, endPoint) = response;
                var n = (int)stream.Length;
#if NET35_CF || NET40
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
#if !NET35_CF && !NET40
                    System.Buffers.ArrayPool<byte>.Shared.Return(buffer);
#endif
                }
            }
        }
        public async void Process(UdpClient udpClient, UdpReceiveResult result, ConcurrentQueue<(int index, MemoryStream stream, IPEndPoint endPoint)> responses, AutoResetEvent autoResetEvent) {
            var buffer = result.Buffer;
            var ipEndPoint = result.RemoteEndPoint;
            uint crc = (uint)((buffer[0] << 24) | (buffer[1] << 16) | (buffer[2] << 8) | buffer[3]);
            int length = (buffer[4] << 8) | buffer[5];
            int index = (buffer[6] << 8) | buffer[7];
            if (CRC32.Compute(buffer, 4, 4) != crc || (length != buffer.Length - 8) || (buffer[6] & 0x80) != 0) {
                return;
            }
            if (length > Service.MaxRequestLength) {
                var bytes = Encoding.UTF8.GetBytes("Request entity too large");
                responses.Enqueue((index | 0x8000, new MemoryStream(bytes), ipEndPoint));
                try {
                    autoResetEvent.Set();
                }
                catch (Exception) { }
                return;
            }
            var context = new ServiceContext(Service);
            context["udpClient"] = udpClient;
            context["socket"] = udpClient.Client;
            context.RemoteEndPoint = ipEndPoint;
            context.LocalEndPoint = udpClient.Client.LocalEndPoint;
            context.Handler = this;
            using var request = new MemoryStream(buffer, 8, length, false, true);
            try {
                Stream response = await Service.Handle(request, context).ConfigureAwait(false);
                responses.Enqueue((index, (await response.ToMemoryStream().ConfigureAwait(false)), ipEndPoint));
            }
            catch (Exception e) {
                responses.Enqueue((index | 0x8000, new MemoryStream(Encoding.UTF8.GetBytes(e.Message)), ipEndPoint));
            }
            finally {
                try {
                    autoResetEvent.Set();
                }
                catch (Exception) { }
            }
        }
        public async Task Receive(UdpClient udpClient, ConcurrentQueue<(int index, MemoryStream stream, IPEndPoint endPoint)> responses, AutoResetEvent autoResetEvent) {
            while (true) {
                var result = await udpClient.ReceiveAsync().ConfigureAwait(false);
                Process(udpClient, result, responses, autoResetEvent);
            }
        }
        private async Task Handler(UdpClient udpClient) {
            try {
                var responses = new ConcurrentQueue<(int index, MemoryStream stream, IPEndPoint endPoint)>();
                using var autoResetEvent = new AutoResetEvent(false);
                var receive = Receive(udpClient, responses, autoResetEvent);
                var send = Send(udpClient, responses, autoResetEvent);
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