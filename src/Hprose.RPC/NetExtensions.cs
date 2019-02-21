/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  NetExtensions.cs                                        |
|                                                          |
|  System.Net Extensions for .NET 3.5 CF                   |
|                                                          |
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

#if NET35_CF
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public static class NetExtensions {
        public static Task<Socket> AcceptSocketAsync(this TcpListener tcpListener) => Task.Run<Socket>(() => tcpListener.AcceptSocket());
        public static Task<TcpClient> AcceptTcpClientAsync(this TcpListener tcpListener) => Task.Run<TcpClient>(() => tcpListener.AcceptTcpClient());
        public static Task ConnectAsync(this TcpClient tcpClient, string host, int port) => Task.Run(() => tcpClient.Connect(host, port));
        public static Task ConnectAsync(this TcpClient tcpClient, IPAddress address, int port) => Task.Run(() => tcpClient.Connect(address, port));
        public static Task<Stream> GetRequestStreamAsync(this HttpWebRequest request) => Task<Stream>.Factory.FromAsync(new Func2<AsyncCallback, object, IAsyncResult>(request.BeginGetRequestStream), new Func2<IAsyncResult, Stream>(request.EndGetRequestStream), null);
        public static Task<WebResponse> GetResponse(this HttpWebRequest request) => Task<WebResponse>.Factory.FromAsync(new Func2<AsyncCallback, object, IAsyncResult>(request.BeginGetResponse), new Func2<IAsyncResult, WebResponse>(request.EndGetResponse), null);
    }
}
#endif