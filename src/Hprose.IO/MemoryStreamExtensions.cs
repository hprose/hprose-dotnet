/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  MemoryStreamExtensions.cs                               |
|                                                          |
|  MemoryStream Extensions for C#.                         |
|                                                          |
|  LastModified: Feb 7, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;

namespace Hprose.IO {
    public static class MemoryStreamExtensions {
#if NET40 || NET45 || NET451 || NET452
        public static ArraySegment<byte> GetArraySegment(this MemoryStream stream) {
            var bytes = stream.ToArray();
            return new ArraySegment<byte>(bytes, 0, bytes.Length);
        }
#else
        public static ArraySegment<byte> GetArraySegment(this MemoryStream stream) {
            if (stream.TryGetBuffer(out ArraySegment<byte> buffer)) {
                return buffer;
            }
            var bytes = stream.ToArray();
            return new ArraySegment<byte>(bytes, 0, bytes.Length);
        }
#endif
    }
}