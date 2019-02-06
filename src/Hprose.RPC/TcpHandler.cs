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
|  LastModified: Feb 5, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System.Net.Sockets;
using System.Collections.Concurrent;

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
                    TcpClient tcpClient = await server.AcceptTcpClientAsync();
                    OnAccept?.Invoke(tcpClient);
                    Handler(tcpClient);
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
        private async Task<byte[]> ReadAsync(Stream stream, byte[] bytes, int offset, int length) {
            while (length > 0) {
                int size = await stream.ReadAsync(bytes, offset, length);
                offset += size;
                length -= size;
            }
            return bytes;
        }
        private async void Send(TcpClient tcpClient, BlockingCollection<(int index, Stream stream)> responses) {
            await Task.Factory.StartNew(async () => {
                try {
                    var header = new byte[12];
                    var netStream = tcpClient.GetStream();
                    while (true) {
                        (int index, Stream stream) = responses.Take();
                        try {
                            if (!stream.CanSeek) {
                                var memstream = new MemoryStream();
                                await stream.CopyToAsync(memstream);
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
                        await netStream.WriteAsync(header, 0, 12);
                        await stream.CopyToAsync(netStream);
                        stream.Dispose();
                    }
                }
                catch (Exception e) {
#if NET40 || NET45 || NET451 || NET452
                    tcpClient.Close();
#else
                    tcpClient.Dispose();
#endif
                    if (!(e is InvalidOperationException)) {
                        OnError?.Invoke(e);
                    }
                    OnClose?.Invoke(tcpClient);
                }
            });
        }
        private async void Run(BlockingCollection<(int index, Stream stream)> responses, int index, byte[] data, Context context) {
            using (var request = new MemoryStream(data)) {
                Stream response = null;
                try {
                    response = await Service.Handle(request, context);
                }
                catch (Exception e) {
                    index = (int)(index | 0x80000000);
                    response = new MemoryStream(Encoding.UTF8.GetBytes(e.Message));
                }
                responses.Add((index, response));
            }
        }
        public async void Handler(TcpClient tcpClient) {
            using (var responses = new BlockingCollection<(int index, Stream stream)>()) {
                Send(tcpClient, responses);
                try {
                    var header = new byte[12];
                    var netStream = tcpClient.GetStream();
                    while (true) {
                        await ReadAsync(netStream, header, 0, 12);
                        uint crc = (uint)((header[0] << 24) | (header[1] << 16) | (header[2] << 8) | header[3]);
                        if (CRC32.Compute(header, 4, 8) != crc || (header[4] & 0x80) == 0 || (header[8] & 0x80) != 0) {
                            throw new IOException("invalid request");
                        }
                        int length = ((header[4] & 0x7F) << 24) | (header[5] << 16) | (header[6] << 8) | header[7];
                        int index = (header[4] << 24) | (header[5] << 16) | (header[6] << 8) | header[7];
                        if (length > Service.MaxRequestLength) {
                            responses.Add(((int)(index | 0x80000000), new MemoryStream(Encoding.UTF8.GetBytes("request too long"))));
                            responses.CompleteAdding();
                            return;
                        }
                        var data = await ReadAsync(netStream, new byte[length], 0, length);
                        dynamic context = new ServiceContext(Service);
                        context.TcpClient = tcpClient;
                        context.Socket = tcpClient.Client;
                        context.Handler = this;
                        Run(responses, index, data, context);
                    }
                }
                catch (Exception e) {
#if NET40 || NET45 || NET451 || NET452
                    tcpClient.Close();
#else
                    tcpClient.Dispose();
#endif
                    OnError?.Invoke(e);
                    OnClose?.Invoke(tcpClient);
                }
            }
        }
    }
}

