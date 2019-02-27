/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  SocketTransport.cs                                      |
|                                                          |
|  SocketTransport class for C#.                           |
|                                                          |
|  LastModified: Feb 27, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/
#if !NET35_CF && !NET40 && !NET45 && !NET451 && !NET452 && !NET46 && !NET461 && !NET462 && !NET47
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class SocketTransport : ITransport {
        public static string[] Schemes { get; } = new string[] { "tcp", "tcp4", "tcp6", "unix" };
        private volatile int counter = 0;
        public LingerOption LingerState { get; set; } = null;
        public bool NoDelay { get; set; } = true;
        public int ReceiveBufferSize { get; set; } = 8192;
        public int SendBufferSize { get; set; } = 8192;
        private ConcurrentDictionary<Socket, ConcurrentQueue<(int, MemoryStream)>> Requests { get; } = new ConcurrentDictionary<Socket, ConcurrentQueue<(int, MemoryStream)>>();
        private ConcurrentDictionary<Socket, ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>> Results { get; } = new ConcurrentDictionary<Socket, ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>>();
        private ConcurrentDictionary<Socket, byte> Lock { get; } = new ConcurrentDictionary<Socket, byte>();
        private ConcurrentDictionary<Uri, Lazy<Task<Socket>>> Sockets { get; } = new ConcurrentDictionary<Uri, Lazy<Task<Socket>>>();
        private readonly Func<Uri, Lazy<Task<Socket>>> socketFactory;
        public SocketTransport() {
            socketFactory = (uri) => {
                return new Lazy<Task<Socket>>(async () => {
                    Socket socket = null;
                    try {
                        var family = uri.Scheme.Last() == '6' ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
                        var protocol = ProtocolType.Tcp;
                        if (uri.Scheme == "unix") {
                            family = AddressFamily.Unix;
                            protocol = ProtocolType.Unspecified;
                        }
                        socket = new Socket(family, SocketType.Stream, protocol);
                        if (family == AddressFamily.Unix) {
#if NETCOREAPP2_1 || NETCOREAPP2_2
                            await socket.ConnectAsync(new UnixDomainSocketEndPoint(uri.AbsolutePath));
#else
                            throw new NotSupportedException("not supported unix protocol");
#endif
                        }
                        else {
                            var host = uri.IdnHost;
                            var port = uri.Port > 0 ? uri.Port : 8412;
                            socket.NoDelay = NoDelay;
                            socket.ReceiveBufferSize = ReceiveBufferSize;
                            socket.SendBufferSize = SendBufferSize;
                            if (LingerState != null) {
                                socket.LingerState = LingerState;
                            }
                            await socket.ConnectAsync(host, port);
                        }
                        Requests.TryAdd(socket, new ConcurrentQueue<(int, MemoryStream)>());
                        Results.TryAdd(socket, new ConcurrentDictionary<int, TaskCompletionSource<MemoryStream>>());
                        Receive(socket);
                        return socket;
                    }
                    catch (Exception e) {
                        if (socket != null) {
                            Close(socket, e);
                        }
                        throw;
                    }
                });
            };
        }
        private void Close(Socket socket, Exception e) {
            try {
                if (e.InnerException != null) {
                    e = e.InnerException;
                }
                Requests.TryRemove(socket, out var requests);
                if (Results.TryRemove(socket, out var results)) {
                    while (!results.IsEmpty) {
                        foreach (var i in results.Keys) {
                            if (results.TryRemove(i, out var result)) {
                                result.TrySetException(e);
                            }
                        }
                    }
                }
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch { }
        }
        private static async Task<byte[]> ReadAsync(Socket socket, byte[] bytes, int offset, int length) {
            while (length > 0) {
                var buffer = new ArraySegment<byte>(bytes, offset, length);
                int size = await socket.ReceiveAsync(buffer, SocketFlags.None).ConfigureAwait(false);
                offset += size;
                length -= size;
            }
            return bytes;
        }
        private async void Receive(Socket socket) {
            var results = Results[socket];
            var header = new byte[12];
            try {
                while (true) {
                    await ReadAsync(socket, header, 0, 12).ConfigureAwait(false);
                    uint crc = (uint)((header[0] << 24) | (header[1] << 16) | (header[2] << 8) | header[3]);
                    if (CRC32.Compute(header, 4, 8) != crc || (header[4] & 0x80) == 0) {
                        throw new IOException("invalid response");
                    }
                    int length = ((header[4] & 0x7F) << 24) | (header[5] << 16) | (header[6] << 8) | header[7];
                    int index = (header[8] << 24) | (header[9] << 16) | (header[10] << 8) | header[11];
                    bool has_error = (index & 0x80000000) != 0;
                    index &= 0x7FFFFFFF;
                    var data = await ReadAsync(socket, new byte[length], 0, length).ConfigureAwait(false);
                    if (results.TryRemove(index, out var result)) {
                        if (has_error) {
                            result.TrySetException(new Exception(Encoding.UTF8.GetString(data)));
                            throw new IOException("connection closed");
                        }
                        result.TrySetResult(new MemoryStream(data, 0, data.Length, false, true));
                    }
                }
            }
            catch (Exception e) {
                Close(socket, e);
            }
        }
        private async void Send(Socket socket) {
            if (!Lock.TryAdd(socket, default)) return;
            try {
                var requests = Requests[socket];
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
                        await socket.SendAsync(new ArraySegment<byte>[] { new ArraySegment<byte>(header), stream.GetArraySegment() }, SocketFlags.None).ConfigureAwait(false);
                    }
                    catch (Exception e) {
                        Close(socket, e);
                    }
                    finally {
                        stream.Dispose();
                    }
                }
            }
            finally {
                Lock.TryRemove(socket, out var _);
            }
        }
        private async Task<Socket> GetSocket(Uri uri) {
            int retried = 0;
            while (true) {
                var LazySocket = Sockets.GetOrAdd(uri, socketFactory);
                var socket = await LazySocket.Value.ConfigureAwait(false);
                try {
                    var available = socket.Available;
                    return socket;
                }
                catch (Exception e) {
                    Sockets.TryRemove(uri, out LazySocket);
                    Close(socket, e);
                    if (++retried > 1) {
                        throw;
                    }
                }
            }
        }
        public async Task<Stream> Transport(Stream request, Context context) {
            var stream = await request.ToMemoryStream().ConfigureAwait(false);
            var clientContext = context as ClientContext;
            var index = Interlocked.Increment(ref counter) & 0x7FFFFFFF;
            var socket = await GetSocket(clientContext.Uri).ConfigureAwait(false);
            var results = Results[socket];
            var result = new TaskCompletionSource<MemoryStream>();
            results[index] = result;
            var requests = Requests[socket];
            requests.Enqueue((index, stream));
            Send(socket);
            var timeout = clientContext.Timeout;
            if (timeout > TimeSpan.Zero) {
                using (CancellationTokenSource source = new CancellationTokenSource()) {
                    var timer = Task.Delay(timeout, source.Token);
                    var task = await Task.WhenAny(timer, result.Task).ConfigureAwait(false);
                    source.Cancel();
                    if (task == timer) {
                        Close(socket, new TimeoutException());
                    }
                }
            }
            return await result.Task.ConfigureAwait(false);
        }
        public async Task Abort() {
            foreach (var LazySocket in Sockets.Values) {
                try {
                    var socket = await LazySocket.Value.ConfigureAwait(false);
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch { }
            }
        }
    }
}
#endif