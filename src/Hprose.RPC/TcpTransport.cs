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
|  LastModified: May 17, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
#if !NET35_CF
using System.Net.Security;
#endif
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class TcpTransport : ITransport {
#if !NET35_CF
        public static string[] Schemes { get; } = new string[] { "tcp", "tcp4", "tcp6", "tls", "tls4", "tls6", "ssl", "ssl4", "ssl6" };
#else
        public static string[] Schemes { get; } = new string[] { "tcp", "tcp4", "tcp6" };
#endif
        private volatile int counter = 0;
        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 30);
#if !NET35_CF
        public LingerOption LingerState { get; set; } = null;
        public bool NoDelay { get; set; } = true;
        public int ReceiveBufferSize { get; set; } = 8192;
        public int SendBufferSize { get; set; } = 8192;
        public RemoteCertificateValidationCallback ValidateServerCertificate { get; set; } = (sender, certificate, chain, sslPolicyErrors) => true;
        public LocalCertificateSelectionCallback CertificateSelection { get; set; } = null;
        public EncryptionPolicy EncryptionPolicy { get; set; } = EncryptionPolicy.RequireEncryption;
        public string ServerCertificateName { get; set; } = null;
#endif
        private ConcurrentDictionary<TcpClient, ConcurrentQueue<(int, MemoryStream)>> Requests { get; } = new ConcurrentDictionary<TcpClient, ConcurrentQueue<(int, MemoryStream)>>();
        private ConcurrentDictionary<TcpClient, ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>> Results { get; } = new ConcurrentDictionary<TcpClient, ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>>();
        private ConcurrentDictionary<TcpClient, byte> Lock { get; } = new ConcurrentDictionary<TcpClient, byte>();
        private ConcurrentDictionary<TcpClient, Uri> Uris { get; } = new ConcurrentDictionary<TcpClient, Uri>();
        private ConcurrentDictionary<Uri, Lazy<Task<(TcpClient, Stream)>>> TcpClients { get; } = new ConcurrentDictionary<Uri, Lazy<Task<(TcpClient, Stream)>>>();
#if !NET35_CF
        private readonly Func<Uri, Lazy<Task<(TcpClient, Stream)>>> tcpClientFactory;
#else
        private readonly Func2<Uri, Lazy<Task<(TcpClient, Stream)>>> tcpClientFactory;
#endif
        public TcpTransport() {
            tcpClientFactory = (uri) => {
                return new Lazy<Task<(TcpClient, Stream)>>(async () => {
                    TcpClient tcpClient = null;
                    try {
                        var family = uri.Scheme.Last() == '6' ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
#if NET35_CF || NET40 || NET45 || NET451 || NET452
                        var host = uri.DnsSafeHost;
#else
                        var host = uri.IdnHost;
#endif
                        var port = uri.Port > 0 ? uri.Port : 8412;
#if !NET35_CF
                        tcpClient = new TcpClient(family) {
                            NoDelay = NoDelay,
                            ReceiveBufferSize = ReceiveBufferSize,
                            SendBufferSize = SendBufferSize
                        };
                        if (LingerState != null) {
                            tcpClient.LingerState = LingerState;
                        }
#else
                        tcpClient = new TcpClient(family);
#endif
                        Uris.TryAdd(tcpClient, uri);
                        await tcpClient.ConnectAsync(host, port).ConfigureAwait(false);
                        Stream tcpStream = tcpClient.GetStream();
#if !NET35_CF
                        switch (uri.Scheme) {
                            case "tls":
                            case "tls4":
                            case "tls6":
                            case "ssl":
                            case "ssl4":
                            case "ssl6":
                                SslStream sslstream = new SslStream(tcpStream, false, ValidateServerCertificate, CertificateSelection, EncryptionPolicy);
                                await sslstream.AuthenticateAsClientAsync(ServerCertificateName ?? uri.Host).ConfigureAwait(false);
                                tcpStream = sslstream;
                                break;
                        }
#endif
                        Requests.TryAdd(tcpClient, new ConcurrentQueue<(int, MemoryStream)>());
                        Results.TryAdd(tcpClient, new ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>());
                        Receive(tcpClient, tcpStream);
                        return (tcpClient, tcpStream);
                    }
                    catch (Exception e) {
                        if (tcpClient != null) {
                            await Close(tcpClient, e).ConfigureAwait(false);
                        }
                        throw;
                    }
                });
            };
        }
        private async Task Close(TcpClient tcpClient, Exception e) {
            try {
                if (e.InnerException != null) {
                    e = e.InnerException;
                }
                if (Uris.TryRemove(tcpClient, out var uri)) {
                    TcpClients.TryGetValue(uri, out var value);
                    if ((await value.Value.ConfigureAwait(false)).Item1 == tcpClient) {
                        TcpClients.TryRemove(uri, out _);
                    }
                }
                Requests.TryRemove(tcpClient, out var requests);
                if (Results.TryRemove(tcpClient, out var results)) {
                    while (!results.IsEmpty) {
                        foreach (var i in results.Keys) {
                            if (results.TryRemove(i, out var result)) {
                                result.TrySetException(e);
                            }
                        }
                    }
                }
                tcpClient.Close();
            }
            catch { }
        }
        private static async Task<byte[]> ReadAsync(Stream stream, byte[] bytes, int offset, int length) {
            while (length > 0) {
                int size = await stream.ReadAsync(bytes, offset, length).ConfigureAwait(false);
                if (size == 0) throw new EndOfStreamException();
                offset += size;
                length -= size;
            }
            return bytes;
        }
        private async void Receive(TcpClient tcpClient, Stream tcpStream) {
            var results = Results[tcpClient];
            var header = new byte[12];
            try {
                while (true) {
                    await ReadAsync(tcpStream, header, 0, 12).ConfigureAwait(false);
                    uint crc = (uint)((header[0] << 24) | (header[1] << 16) | (header[2] << 8) | header[3]);
                    if (CRC32.Compute(header, 4, 8) != crc || (header[4] & 0x80) == 0) {
                        throw new IOException("invalid response");
                    }
                    int length = ((header[4] & 0x7F) << 24) | (header[5] << 16) | (header[6] << 8) | header[7];
                    int index = (header[8] << 24) | (header[9] << 16) | (header[10] << 8) | header[11];
                    bool has_error = (index & 0x80000000) != 0;
                    index &= 0x7FFFFFFF;
                    var data = await ReadAsync(tcpStream, new byte[length], 0, length).ConfigureAwait(false);
                    if (results.TryRemove(index, out var result)) {
                        if (has_error) {
                            result.TrySetException(new Exception(Encoding.UTF8.GetString(data, 0, data.Length)));
                            throw new IOException("connection closed");
                        }
                        result.TrySetResult(new MemoryStream(data, 0, data.Length, false, true));
                    }
                }
            }
            catch (Exception e) {
                await Close(tcpClient, e).ConfigureAwait(false);
            }
        }
        private async void Send(TcpClient tcpClient, Stream tcpStream) {
            if (!Lock.TryAdd(tcpClient, default)) return;
            try {
                var requests = Requests[tcpClient];
                var header = new byte[12];
                while (requests.TryDequeue(out var request)) {
                    var (index, stream) = request;
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
                    try {
                        await tcpStream.WriteAsync(header, 0, 12).ConfigureAwait(false);
                        await stream.CopyToAsync(tcpStream).ConfigureAwait(false);
                        await tcpStream.FlushAsync().ConfigureAwait(false);
                    }
                    catch (Exception e) {
                        await Close(tcpClient, e).ConfigureAwait(false);
                    }
                    finally {
                        stream.Dispose();
                    }
                }
            }
            finally {
                Lock.TryRemove(tcpClient, out var _);
            }
        }
        private Task<(TcpClient, Stream)> GetTcpClient(Uri uri) {
            return TcpClients.GetOrAdd(uri, tcpClientFactory).Value;
        }
        public async Task<Stream> Transport(Stream request, Context context) {
            var stream = await request.ToMemoryStream().ConfigureAwait(false);
            var clientContext = context as ClientContext;
            var index = Interlocked.Increment(ref counter) & 0x7FFFFFFF;
            var (tcpClient, tcpStream) = await GetTcpClient(clientContext.Uri).ConfigureAwait(false);
            var result = new TaskCompletionSource<MemoryStream>();
            Results[tcpClient][index] = result;
            Requests[tcpClient].Enqueue((index, stream));
            Send(tcpClient, tcpStream);
            var timeout = clientContext.Timeout;
            if (timeout > TimeSpan.Zero) {
                using (CancellationTokenSource source = new CancellationTokenSource()) {
#if NET40
                   var timer = TaskEx.Delay(timeout, source.Token);
                   var task = await TaskEx.WhenAny(timer, result.Task).ConfigureAwait(false);
#else
                    var timer = Task.Delay(timeout, source.Token);
                    var task = await Task.WhenAny(timer, result.Task).ConfigureAwait(false);
#endif
                    source.Cancel();
                    if (task == timer) {
                        await Close(tcpClient, new TimeoutException()).ConfigureAwait(false);
                    }
                }
            }
            return await result.Task.ConfigureAwait(false);
        }
        public async Task Abort() {
            foreach (var LazyTcpClient in TcpClients.Values) {
                try {
                    var (tcpClient, _) = await LazyTcpClient.Value.ConfigureAwait(false);
                    await Close(tcpClient, new SocketException(10053)).ConfigureAwait(false);
                }
                catch { }
            }
        }
    }
}