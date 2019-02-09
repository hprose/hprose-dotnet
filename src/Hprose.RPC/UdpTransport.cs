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
|  LastModified: Feb 10, 2019                              |
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
        public bool EnableBroadcast { get; set; } = true;
        public short Ttl { get; set; } = 0;
        private ConcurrentDictionary<string, ConcurrentQueue<(int, Stream)>> Requests { get; } = new ConcurrentDictionary<string, ConcurrentQueue<(int, Stream)>>();
        private ConcurrentDictionary<string, ConcurrentDictionary<int, TaskCompletionSource<Stream>>> Results { get; } = new ConcurrentDictionary<string, ConcurrentDictionary<int, TaskCompletionSource<Stream>>>();
        private ConcurrentDictionary<string, ConcurrentDictionary<int, TaskCompletionSource<Stream>>> Sended { get; } = new ConcurrentDictionary<string, ConcurrentDictionary<int, TaskCompletionSource<Stream>>>();
        private ConcurrentDictionary<string, Lazy<UdpClient>> UdpClients { get; } = new ConcurrentDictionary<string, Lazy<UdpClient>>();
        private readonly Func<string, Lazy<UdpClient>> udpClientFactory;
        public UdpTransport() {
            udpClientFactory = (uri) => {
                return new Lazy<UdpClient>(() => {
                    try {
                        var u = new Uri(uri);
                        var family = u.Scheme.Last() == '6' ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
#if NET40 || NET45 || NET451 || NET452
                        var host = u.DnsSafeHost;
#else
                        var host = u.IdnHost;
#endif
                        var port = u.Port > 0 ? u.Port : 8412;
                        var client = new UdpClient(family) {
                            EnableBroadcast = EnableBroadcast
                        };
                        if (Ttl > 0) {
                            client.Ttl = Ttl;
                        }
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
                                if (requests.TryDequeue(out (int index, Stream stream) request)) {
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
#if NET40 || NET45 || NET451 || NET452
                    udpClient.Close();
#else
                    udpClient.Dispose();
#endif
                }
                catch { }
            }
        }
        private async void Receive(string uri, UdpClient udpClient) {
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
#if NET40 || NET45 || NET451 || NET452
                udpClient.Close();
#else
                udpClient.Dispose();
#endif
            }
        }
        private async void Send(string uri, (int index, Stream stream) request) {
            var sended = Sended[uri];
            var index = request.index;
            MemoryStream stream;
            try {
                stream = await request.stream.ToMemoryStream().ConfigureAwait(false);
            }
            catch (Exception e) {
                if (sended.TryRemove(index, out var value)) {
                    value.TrySetException(e);
                }
                return;
            }
            var data = stream.GetArraySegment();
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
            stream.Dispose();
            UdpClient udpClient = null;
            try {
                udpClient = GetUdpClient(uri);
                await udpClient.SendAsync(buffer, buffer.Length).ConfigureAwait(false);
            }
            catch (Exception e) {
                foreach (var i in sended.Keys) {
                    if (sended.TryRemove(i, out var value)) {
                        value.TrySetException(e);
                    }
                }
                if (udpClient != null) {
#if NET40 || NET45 || NET451 || NET452
                    udpClient.Close();
#else
                    udpClient.Dispose();
#endif
                }
            }
        }
        private async void Send(string uri) {
            var requests = Requests[uri];
            var results = Results[uri];
            var sended = Sended.GetOrAdd(uri, (_) => new ConcurrentDictionary<int, TaskCompletionSource<Stream>>());
            while (!requests.IsEmpty) {
                requests.TryPeek(out (int index, Stream stream) request);
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
                Send(uri, request);
                requests.TryDequeue(out request);
            }
        }
        private UdpClient GetUdpClient(string uri) {
            int retried = 0;
            while(true) {
                var LazyUdpClient = UdpClients.GetOrAdd(uri, udpClientFactory);
                var udpClient = LazyUdpClient.Value;
                try {
                    var available = udpClient.Available;
                    return udpClient;
                }
                catch {
                    UdpClients.TryRemove(uri, out LazyUdpClient);
#if NET40 || NET45 || NET451 || NET452
                    udpClient.Close();
#else
                    udpClient.Dispose();
#endif
                    if (++retried > 1) {
                        throw;
                    }
                }
            }
        }
        public Task<Stream> Transport(Stream request, Context context) {
            var clientContext = context as ClientContext;
            var uri = clientContext.Uri;
            var index = Interlocked.Increment(ref counter) & 0x7FFF;
            var results = Results.GetOrAdd(uri, (_) => new ConcurrentDictionary<int, TaskCompletionSource<Stream>>());
            var result = new TaskCompletionSource<Stream>();
            results[index] = result;
            var requests = Requests.GetOrAdd(uri, (_) => new ConcurrentQueue<(int, Stream)>());
            requests.Enqueue((index, request));
            requests.TryPeek(out (int index, Stream stream) first);
            if (first.index == index) {
                Send(uri);
            }
            return result.Task;
        }
    }
}