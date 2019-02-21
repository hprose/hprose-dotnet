/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  UdpReceiveResult.cs                                     |
|                                                          |
|  UdpReceiveResult struct for .NET 4.0                    |
|                                                          |
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

#if NET35_CF || NET40
using System;
using System.Net;

namespace Hprose.RPC {
    public struct UdpReceiveResult : IEquatable<UdpReceiveResult> {
        public UdpReceiveResult(byte[] buffer, IPEndPoint remoteEndPoint) {
            this.Buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
            this.RemoteEndPoint = remoteEndPoint ?? throw new ArgumentNullException(nameof(remoteEndPoint));
        }

        public byte[] Buffer { get; private set; }

        public IPEndPoint RemoteEndPoint { get; private set; }

        public override int GetHashCode() => Buffer == null ? 0 : Buffer.GetHashCode() ^ RemoteEndPoint.GetHashCode();

        public override bool Equals(object obj) => obj is UdpReceiveResult && Equals((UdpReceiveResult)obj);

        public bool Equals(UdpReceiveResult other) => Equals(Buffer, other.Buffer) && Equals(RemoteEndPoint, other.RemoteEndPoint);

        public static bool operator ==(UdpReceiveResult left, UdpReceiveResult right) => left.Equals(right);

        public static bool operator !=(UdpReceiveResult left, UdpReceiveResult right) => !left.Equals(right);
    }
}
#endif