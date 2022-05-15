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
|  LastModified: May 15, 2022                              |
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
        private ConcurrentDictionary<UdpClient, ConcurrentQueue<(int, MemoryStream)>> Requests { get; } = new();
        private ConcurrentDictionary<UdpClient, ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>> Results { get; } = new();
        private ConcurrentDictionary<UdpClient, byte> Lock { get; } = new();
        private ConcurrentDictionary<UdpClient, Uri> Uris { get; } = new();
        private ConcurrentDictionary<Uri, Lazy<UdpClient>> UdpClients { get; } = new();
#if !NET35_CF
        private readonly Func<Uri, Lazy<UdpClient>> udpClientFactory;
#else
        private readonly Func2<Uri, Lazy<UdpClient>> udpClientFactory;
#endif
        public UdpTransport() {
            udpClientFactory = (uri) => {
                return new Lazy<UdpClient>(() => {
                    UdpClient udpClient = null;
                    try {
                        var family = uri.Scheme.Last() == '6' ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
#if NET35_CF || NET40 || NET45 || NET451 || NET452
                        var host = uri.DnsSafeHost;
#else
                        var host = uri.IdnHost;
#endif
                        var port = uri.Port > 0 ? uri.Port : 8412;
#if !NET35_CF
                        udpClient = new UdpClient(family) {
                            EnableBroadcast = EnableBroadcast
                        };
                        if (Ttl > 0) {
                            udpClient.Ttl = Ttl;
                        }
#else
                        udpClient = new UdpClient(family);
#endif
                        Uris.TryAdd(udpClient, uri);
                        udpClient.Connect(host, port);
                        Requests.TryAdd(udpClient, new ConcurrentQueue<(int, MemoryStream)>());
                        Results.TryAdd(udpClient, new ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>());
                        Receive(udpClient);
                        return udpClient;
                    }
                    catch (Exception e) {
                        if (udpClient != null) {
                            Close(udpClient, e);
                        }
                        throw;
                    }
                });
            };
        }
        private void Close(UdpClient udpClient, Exception e) {
            try {
                if (e.InnerException != null) {
                    e = e.InnerException;
                }
                if (Uris.TryRemove(udpClient, out var uri)) {
                    UdpClients.TryGetValue(uri, out var value);
                    if (value.Value == udpClient) {
                        UdpClients.TryRemove(uri, out _);
                    }
                }
                Requests.TryRemove(udpClient, out var requests);
                if (Results.TryRemove(udpClient, out var results)) {
                    while (!results.IsEmpty) {
                        foreach (var i in results.Keys) {
                            if (results.TryRemove(i, out var result)) {
                                result.TrySetException(e);
                            }
                        }
                    }
                }
                udpClient.Close();
            }
            catch { }
        }
        private async void Receive(UdpClient udpClient) {
            var results = Results[udpClient];
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
                    if (results.TryRemove(index, out var result)) {
                        if (has_error) {
                            result.TrySetException(new Exception(Encoding.UTF8.GetString(buffer, 8, length)));
                        }
                        result.TrySetResult(new MemoryStream(buffer, 8, length, false, true));
                    }
                }
            }
            catch (Exception e) {
                Close(udpClient, e);
            }
        }
        private async void Send(UdpClient udpClient) {
            if (!Lock.TryAdd(udpClient, default)) return;
            try {
                var requests = Requests[udpClient];
                while (requests.TryDequeue(out var request)) {
                    var (index, stream) = request;
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
                        await udpClient.SendAsync(buffer, n + 8).ConfigureAwait(false);
                    }
                    catch (Exception e) {
                        Close(udpClient, e);
                    }
                    finally {
                        stream.Dispose();
#if !NET35_CF && !NET40
                        System.Buffers.ArrayPool<byte>.Shared.Return(buffer);
#endif
                    }
                }
            }
            finally {
                Lock.TryRemove(udpClient, out var _);
            }
        }
        private UdpClient GetUdpClient(Uri uri) {
            return UdpClients.GetOrAdd(uri, udpClientFactory).Value;
        }
        public async Task<Stream> Transport(Stream request, Context context) {
            var stream = await request.ToMemoryStream().ConfigureAwait(false);
            if (stream.Length > 65499) {
                throw new Exception("request too large");
            }
            var clientContext = context as ClientContext;
            var timeout = clientContext.Timeout;
            if (timeout <= TimeSpan.Zero) {
                timeout = TimeSpan.MaxValue;
            }
            using CancellationTokenSource source = new();
#if NET40
            var timer = TaskEx.Delay(timeout, source.Token);
#else
            var timer = Task.Delay(timeout, source.Token);
#endif
            var udpClient = GetUdpClient(clientContext.Uri);
            var result = new TaskCompletionSource<MemoryStream>();
            var index = Interlocked.Increment(ref counter) & 0x7FFF;
            Results[udpClient][index] = result;
            Requests[udpClient].Enqueue((index, stream));
            Send(udpClient);
#if NET40
            var task = await TaskEx.WhenAny(timer, result.Task).ConfigureAwait(false);
#else
            var task = await Task.WhenAny(timer, result.Task).ConfigureAwait(false);
#endif
            source.Cancel();
            if (task == timer) {
                Close(udpClient, new TimeoutException());
            }
            return await result.Task.ConfigureAwait(false);
        }
        public Task Abort() {
            foreach (var LazyUdpClient in UdpClients.Values) {
                try {
                    var udpClient = LazyUdpClient.Value;
                    Close(udpClient, new SocketException(10053));
                }
                catch { }
            }
#if NET40
            return TaskEx.FromResult<object>(null);
#elif NET45 || NET451 || NET452
            return Task.FromResult<object>(null);
#else
            return Task.CompletedTask;
#endif
        }
    }
}