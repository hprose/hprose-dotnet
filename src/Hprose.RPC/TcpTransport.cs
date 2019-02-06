/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  TcpTransport.cs                                         |
|                                                          |
|  TcpTransport class for C#.                              |
|                                                          |
|  LastModified: Feb 6, 2019                               |
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
    public class TcpTransport : ITransport {
        public static string[] Schemes { get; } = new string[] { "tcp", "tcp4", "tcp6", "tls", "tls4", "tls6", "ssl", "ssl4", "ssl6" };
        private volatile int counter = 0;
        private volatile int current = -1;
        public LingerOption LingerState { get; set; } = null;
        public bool NoDelay { get; set; } = true;
        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 30);
        public int ReceiveBufferSize { get; set; } = 8192;
        public int SendBufferSize { get; set; } = 8192;
        private ConcurrentDictionary<string, ConcurrentQueue<(int, Stream)>> Requests { get; } = new ConcurrentDictionary<string, ConcurrentQueue<(int, Stream)>>();
        private ConcurrentDictionary<string, ConcurrentDictionary<int, TaskCompletionSource<Stream>>> Results { get; } = new ConcurrentDictionary<string, ConcurrentDictionary<int, TaskCompletionSource<Stream>>>();
        private ConcurrentDictionary<string, ConcurrentDictionary<int, TaskCompletionSource<Stream>>> Sended { get; } = new ConcurrentDictionary<string, ConcurrentDictionary<int, TaskCompletionSource<Stream>>>();
        private ConcurrentDictionary<string, Lazy<Task<TcpClient>>> TcpClients { get; } = new ConcurrentDictionary<string, Lazy<Task<TcpClient>>>();
        private readonly Func<string, Lazy<Task<TcpClient>>> tcpClientFactory;
        public TcpTransport() {
            tcpClientFactory = (uri) => {
                return new Lazy<Task<TcpClient>>(async () => {
                    try {
                        var u = new Uri(uri);
                        var family = u.Scheme.Last() == '6' ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
#if NET40 || NET45 || NET451 || NET452
                        var host = u.DnsSafeHost;
#else
                        var host = u.IdnHost;
#endif
                        var port = u.Port > 0 ? u.Port : 8412;
                        var client = new TcpClient(family) {
                            NoDelay = NoDelay,
                            ReceiveBufferSize = ReceiveBufferSize,
                            SendBufferSize = SendBufferSize
                        };
                        if (LingerState != null) {
                            client.LingerState = LingerState;
                        }

                        await client.ConnectAsync(host, port);
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
                        throw e;
                    }
                });
            };
        }
        public async void Abort() {
            foreach (var LazyTcpClient in TcpClients.Values) {
                try {
                    var tcpClient = await LazyTcpClient.Value;
#if NET40 || NET45 || NET451 || NET452
                    tcpClient.Close();
#else
                    tcpClient.Dispose();
#endif
                }
                catch { }
            }
        }
        private async Task<byte[]> ReadAsync(Stream stream, byte[] bytes, int offset, int length) {
            while (length > 0) {
                int size = await stream.ReadAsync(bytes, offset, length);
                offset += size;
                length -= size;
            }
            return bytes;
        }
        private async void Receive(string uri, TcpClient tcpClient) {
            var sended = Sended[uri];
            var header = new byte[12];
            try {
                var netStream = tcpClient.GetStream();
                while (true) {
                    await ReadAsync(netStream, header, 0, 12);
                    uint crc = (uint)((header[0] << 24) | (header[1] << 16) | (header[2] << 8) | header[3]);
                    if (CRC32.Compute(header, 4, 8) != crc || (header[4] & 0x80) == 0) {
                        throw new IOException("invalid response");
                    }
                    int length = ((header[4] & 0x7F) << 24) | (header[5] << 16) | (header[6] << 8) | header[7];
                    int index = (header[4] << 24) | (header[5] << 16) | (header[6] << 8) | header[7];
                    bool has_error = (index & 0x80000000) != 0;
                    index &= 0x7FFFFFFF;
                    var data = await ReadAsync(netStream, new byte[length], 0, length);
                    if (sended.TryRemove(index, out var result)) {
                        if (has_error) {
                            result.TrySetException(new Exception(Encoding.UTF8.GetString(data)));
                            throw new IOException("connection closed");
                        }
                        result.TrySetResult(new MemoryStream(data));
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
                tcpClient.Close();
#else
                tcpClient.Dispose();
#endif
            }
        }
        private async void Send(string uri) {
            var requests = Requests[uri];
            var results = Results[uri];
            var sended = Sended.GetOrAdd(uri, (_) => new ConcurrentDictionary<int, TaskCompletionSource<Stream>>());
            var header = new byte[12];
            while (!requests.IsEmpty) {
                requests.TryPeek(out (int index, Stream stream) request);
                var index = request.index;
                if (Interlocked.Exchange(ref current, index) == index) return;
                if (results.TryGetValue(index, out var result)) {
                    sended[index] = result;
                    results.TryRemove(index, out result);
                }
                var stream = request.stream;
                try {
                    if (!stream.CanSeek) {
                        stream = new MemoryStream();
                        await request.stream.CopyToAsync(stream);
                        request.stream.Dispose();
                    }
                }
                catch (Exception e) {
                    if (sended.TryRemove(index, out var value)) {
                        value.TrySetException(e);
                    }
                    requests.TryDequeue(out request);
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
                TcpClient tcpClient = null;
                try {
                    tcpClient = await GetTcpClient(uri);
                    var netStream = tcpClient.GetStream();
                    await netStream.WriteAsync(header, 0, 12);
                    await stream.CopyToAsync(netStream);
                    stream.Dispose();
                }
                catch (Exception e) {
                    foreach (var i in sended.Keys) {
                        if (sended.TryRemove(i, out var value)) {
                            value.TrySetException(e);
                        }
                    }
                    if (tcpClient != null) {
#if NET40 || NET45 || NET451 || NET452
                        tcpClient.Close();
#else
                        tcpClient.Dispose();
#endif
                    }
                }
                requests.TryDequeue(out request);
            }
        }
        private async Task<TcpClient> GetTcpClient(string uri) {
            int retried = 0;
            while(true) {
                var LazyTcpClient = TcpClients.GetOrAdd(uri, tcpClientFactory);
                var tcpClient = await LazyTcpClient.Value;
                try {
                    var available = tcpClient.Available;
                    return tcpClient;
                }
                catch (Exception e) {
                    TcpClients.TryRemove(uri, out LazyTcpClient);
#if NET40 || NET45 || NET451 || NET452
                    tcpClient.Close();
#else
                    tcpClient.Dispose();
#endif
                    if (++retried > 1) {
                        throw e;
                    }
                }
            }
        }
        public async Task<Stream> Transport(Stream request, Context context) {
            var clientContext = context as ClientContext;
            var uri = clientContext.Uri;
            var index = Interlocked.Increment(ref counter);
            while (index < 0) {
                Interlocked.Add(ref counter, Int32.MinValue);
                index = Interlocked.Increment(ref counter);
            }
            var results = Results.GetOrAdd(uri, (_) => new ConcurrentDictionary<int, TaskCompletionSource<Stream>>());
            var result = new TaskCompletionSource<Stream>();
            results[index] = result;
            var requests = Requests.GetOrAdd(uri, (_) => new ConcurrentQueue<(int, Stream)>());
            requests.Enqueue((index, request));
            requests.TryPeek(out (int index, Stream stream) first);
            if (first.index == index) {
                Send(uri);
            }
            return await result.Task;
        }
    }
}