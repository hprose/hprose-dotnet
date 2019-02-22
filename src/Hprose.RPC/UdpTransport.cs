/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  UdpTransport.cs                                         |
|                                                          |
|  UdpTransport class for C#.                              |
|                                                          |
|  LastModified: Feb 23, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class UdpTransport : ITransport {
        public static string[] Schemes { get; } = new string[] { "udp", "udp4", "udp6" };
        private volatile int counter = 0;
#if !NET35_CF
        public bool EnableBroadcast { get; set; } = true;
        public short Ttl { get; set; } = 0;
#endif
        private ConcurrentDictionary<Uri, ConcurrentQueue<(int, MemoryStream)>> Requests { get; } = new ConcurrentDictionary<Uri, ConcurrentQueue<(int, MemoryStream)>>();
        private ConcurrentDictionary<Uri, ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>> Results { get; } = new ConcurrentDictionary<Uri, ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>>();
        private ConcurrentDictionary<Uri, ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>> Sended { get; } = new ConcurrentDictionary<Uri, ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>>();
        private ConcurrentDictionary<Uri, Lazy<UdpClient>> UdpClients { get; } = new ConcurrentDictionary<Uri, Lazy<UdpClient>>();
#if !NET35_CF
        private readonly Func<Uri, Lazy<UdpClient>> udpClientFactory;
#else
        private readonly Func2<Uri, Lazy<UdpClient>> udpClientFactory;
#endif
        public UdpTransport() {
            udpClientFactory = (uri) => {
                return new Lazy<UdpClient>(() => {
                    try {
                        var family = uri.Scheme.Last() == '6' ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
#if NET35_CF || NET40 || NET45 || NET451 || NET452
                        var host = uri.DnsSafeHost;
#else
                        var host = uri.IdnHost;
#endif
                        var port = uri.Port > 0 ? uri.Port : 8412;
#if !NET35_CF
                        var client = new UdpClient(family) {
                            EnableBroadcast = EnableBroadcast
                        };
                        if (Ttl > 0) {
                            client.Ttl = Ttl;
                        }
#else
                        var client = new UdpClient(family);
#endif
                        client.Connect(host, port);
                        Receive(uri, client);
                        return client;
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
        public void Abort() {
            foreach (var LazyUdpClient in UdpClients.Values) {
                try {
                    var udpClient = LazyUdpClient.Value;
                    udpClient.Close();
                }
                catch { }
            }
        }
        private async void Receive(Uri uri, UdpClient udpClient) {
            var sended = Sended[uri];
            try {
                while (true) {
                    var data = await udpClient.ReceiveAsync().ConfigureAwait(false);
                    var buffer = data.Buffer;
                    uint crc = (uint)((buffer[0] << 24) | (buffer[1] << 16) | (buffer[2] << 8) | buffer[3]);
                    int length = (buffer[4] << 8) | buffer[5];
                    int index = (buffer[6] << 8) | buffer[7];
                    if (CRC32.Compute(buffer, 4, 4) != crc ||
                        (length != buffer.Length - 8)) {
                        continue;
                    }
                    bool has_error = (index & 0x8000) != 0;
                    index &= 0x7FFF;
                    if (sended.TryRemove(index, out var result)) {
                        if (has_error) {
                            result.TrySetException(new Exception(Encoding.UTF8.GetString(buffer, 8, length)));
                        }
                        result.TrySetResult(new MemoryStream(buffer, 8, length, false, true));
                    }
                }
            }
            catch (Exception e) {
                foreach (var index in sended.Keys) {
                    if (sended.TryRemove(index, out var result)) {
                        result.TrySetException(e);
                    }
                }
                udpClient.Close();
            }
        }
        private async void Send(Uri uri, int index, MemoryStream stream) {
            var n = (int)stream.Length;
#if NET35_CF || NET40
            var buffer = new byte[n + 8];
#else
            var buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(n + 8);
#endif
            UdpClient udpClient = null;
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
                udpClient = GetUdpClient(uri);
                await udpClient.SendAsync(buffer, n + 8).ConfigureAwait(false);
            }
            catch (Exception e) {
                var sended = Sended[uri];
                foreach (var i in sended.Keys) {
                    if (sended.TryRemove(i, out var value)) {
                        value.TrySetException(e);
                    }
                }
                if (udpClient != null) {
                    udpClient.Close();
                }
            }
            finally {
                stream.Dispose();
#if !NET35_CF && !NET40
                System.Buffers.ArrayPool<byte>.Shared.Return(buffer);
#endif
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
#if NET40
                        await TaskEx.Yield();
#else
                        await Task.Yield();
#endif
                        continue;
                    }
                }
                else {
                    return;
                }
                Send(uri, index, request.stream);
                requests.TryDequeue(out request);
            }
        }
        private UdpClient GetUdpClient(Uri uri) {
            int retried = 0;
            while(true) {
                var LazyUdpClient = UdpClients.GetOrAdd(uri, udpClientFactory);
                var udpClient = LazyUdpClient.Value;
                try {
#if !NET35_CF
                    var available = udpClient.Available;
#else
                    var available = udpClient.Client.Available;
#endif
                    return udpClient;
                }
                catch {
                    UdpClients.TryRemove(uri, out LazyUdpClient);
                    udpClient.Close();
                    if (++retried > 1) {
                        throw;
                    }
                }
            }
        }
        public async Task<Stream> Transport(Stream request, Context context) {
            var stream = await request.ToMemoryStream().ConfigureAwait(false);
            if (stream.Length > 65499) {
                throw new Exception("request too large");
            }
            var clientContext = context as ClientContext;
            var uri = clientContext.Uri;
            var index = Interlocked.Increment(ref counter) & 0x7FFF;
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