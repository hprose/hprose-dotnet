/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
| MockEndPoint.cs                                          |
|                                                          |
| MockEndPoint for C#.                                     |
|                                                          |
| LastModified: Feb 27, 2019                               |
| Author: Ma Bingyao <andot@hprose.com>                    |
|                                                          |
\*________________________________________________________*/

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Hprose.RPC {
    public sealed class MockEndPoint : EndPoint {
        private static readonly Encoding s_pathEncoding = Encoding.UTF8;
        private readonly byte[] _encodedAddress;
        public string Address { get; private set; }
        public MockEndPoint(string address) {
            Address = address ?? throw new ArgumentNullException(nameof(address));
            _encodedAddress = s_pathEncoding.GetBytes(address);
        }
        internal MockEndPoint(SocketAddress socketAddress) {
            if (socketAddress == null) {
                throw new ArgumentNullException(nameof(socketAddress));
            }
            if (socketAddress.Family != AddressFamily.Unknown) {
                throw new ArgumentOutOfRangeException(nameof(socketAddress));
            }
            _encodedAddress = new byte[socketAddress.Size];
            for (int i = 0; i < _encodedAddress.Length; i++) {
                _encodedAddress[i] = socketAddress[i];
            }
            Address = s_pathEncoding.GetString(_encodedAddress, 0, _encodedAddress.Length);
        }
        public override SocketAddress Serialize() {
            SocketAddress socketAddress = CreateSocketAddressForSerialize();
            for (int i = 0; i < _encodedAddress.Length; i++) {
                socketAddress[i] = _encodedAddress[i];
            }
            return socketAddress;
        }
        public override EndPoint Create(SocketAddress socketAddress) {
            return new MockEndPoint(socketAddress);
        }
        public override AddressFamily AddressFamily {
            get {
                return AddressFamily.Unknown;
            }
        }

        public override string ToString() {
            return Address;
        }
        private SocketAddress CreateSocketAddressForSerialize() {
            return new SocketAddress(AddressFamily.Unknown, _encodedAddress.Length);
        }
    }
}
