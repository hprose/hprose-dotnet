/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  UdpClientExtensions.cs                                  |
|                                                          |
|  UdpClient Extensions for .NET 4.0                       |
|                                                          |
|  LastModified: Feb 9, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

#if NET40
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public static class UdpClientExtensions {
        public static Task<UdpReceiveResult> ReceiveAsync(this UdpClient udpClient) {
            return Task<UdpReceiveResult>.Factory.FromAsync((AsyncCallback callback, object state) => udpClient.BeginReceive(callback, state), delegate (IAsyncResult ar)
            {
                IPEndPoint remoteEndPoint = null;
                byte[] buffer = udpClient.EndReceive(ar, ref remoteEndPoint);
                return new UdpReceiveResult(buffer, remoteEndPoint);
            }, null);
        }
        public static Task<int> SendAsync(this UdpClient udpClient, byte[] datagram, int bytes) => Task<int>.Factory.FromAsync<byte[], int>(new Func<byte[], int, AsyncCallback, object, IAsyncResult>(udpClient.BeginSend), new Func<IAsyncResult, int>(udpClient.EndSend), datagram, bytes, null);
        public static Task<int> SendAsync(this UdpClient udpClient, byte[] datagram, int bytes, IPEndPoint endPoint) => Task<int>.Factory.FromAsync<byte[], int, IPEndPoint>(new Func<byte[], int, IPEndPoint, AsyncCallback, object, IAsyncResult>(udpClient.BeginSend), new Func<IAsyncResult, int>(udpClient.EndSend), datagram, bytes, endPoint, null);
        public static Task<int> SendAsync(this UdpClient udpClient, byte[] datagram, int bytes, string hostname, int port) => Task<int>.Factory.FromAsync((AsyncCallback callback, object state) => udpClient.BeginSend(datagram, bytes, hostname, port, callback, state), new Func<IAsyncResult, int>(udpClient.EndSend), null);
    }
}
#endif