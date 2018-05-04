/**********************************************************\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: http://www.hprose.com/                 |
|                   http://www.hprose.org/                 |
|                                                          |
\**********************************************************/
/**********************************************************\
 *                                                        *
 * HproseFormatter.cs                                     *
 *                                                        *
 * HproseFormatter for C#.                                *
 *                                                        *
 * LastModified: May 4, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.IO;

using Hprose.IO.Deserializers;
using Hprose.IO.Serializers;

namespace Hprose.IO {
    public static class HproseFormatter {
        public static void Serialize<T>(T value, Stream stream, HproseMode mode = HproseMode.MemberMode) => new Writer(stream, mode).Serialize(value);
        public static void Serialize<T>(T value, Stream stream, bool simple, HproseMode mode = HproseMode.MemberMode) => new Writer(stream, simple, mode).Serialize(value);
        public static void Serialize(object value, Stream stream, HproseMode mode = HproseMode.MemberMode) => new Writer(stream, mode).Serialize(value);
        public static void Serialize(object value, Stream stream, bool simple, HproseMode mode = HproseMode.MemberMode) => new Writer(stream, simple, mode).Serialize(value);
        public static MemoryStream Serialize<T>(T value, HproseMode mode = HproseMode.MemberMode) {
            MemoryStream stream = new MemoryStream();
            Serialize(value, stream, mode);
            return stream;
        }
        public static MemoryStream Serialize<T>(T value, bool simple, HproseMode mode = HproseMode.MemberMode) {
            MemoryStream stream = new MemoryStream();
            Serialize(value, stream, simple, mode);
            return stream;
        }
        public static MemoryStream Serialize(object value, HproseMode mode = HproseMode.MemberMode) {
            MemoryStream stream = new MemoryStream();
            Serialize(value, stream, mode);
            return stream;
        }
        public static MemoryStream Serialize(object value, bool simple, HproseMode mode = HproseMode.MemberMode) {
            MemoryStream stream = new MemoryStream();
            Serialize(value, stream, simple, mode);
            return stream;
        }
        public static T Deserialize<T>(Stream stream, HproseMode mode = HproseMode.MemberMode) => new Reader(stream, mode).Deserialize<T>();
        public static T Deserialize<T>(byte[] data, HproseMode mode = HproseMode.MemberMode) => Deserialize<T>(new MemoryStream(data), mode);
        public static T Deserialize<T>(Stream stream, bool simple, HproseMode mode = HproseMode.MemberMode) => new Reader(stream, simple, mode).Deserialize<T>();
        public static T Deserialize<T>(byte[] data, bool simple, HproseMode mode = HproseMode.MemberMode) => Deserialize<T>(new MemoryStream(data), simple, mode);
        public static object Deserialize(Stream stream, Type type = null, HproseMode mode = HproseMode.MemberMode) => new Reader(stream, mode).Deserialize(type);
        public static object Deserialize(byte[] data, Type type = null, HproseMode mode = HproseMode.MemberMode) => Deserialize(new MemoryStream(data), type, mode);
        public static object Deserialize(Stream stream, Type type, bool simple, HproseMode mode = HproseMode.MemberMode) => new Reader(stream, simple, mode).Deserialize(type);
        public static object Deserialize(byte[] data, Type type, bool simple, HproseMode mode = HproseMode.MemberMode) => Deserialize(new MemoryStream(data), type, simple, mode);
    }
}