/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  StreamExtensions.cs                                     |
|                                                          |
|  Stream Extensions for C#.                               |
|                                                          |
|  LastModified: Feb 9, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;
using System.Threading.Tasks;

namespace Hprose.IO {
    public static class StreamExtensions {
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
        public static async Task<MemoryStream> ToMemoryStream(this Stream stream) {
            if (stream is MemoryStream) return stream as MemoryStream;
            MemoryStream memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
            memoryStream.Position = 0;
            stream.Dispose();
            return memoryStream;
        }
    }
}