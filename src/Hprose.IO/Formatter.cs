/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Formatter.cs                                            |
|                                                          |
|  hprose Formatter for C#.                                |
|                                                          |
|  LastModified: Feb 7, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;

namespace Hprose.IO {
    public static class Formatter {
        public static void Serialize<T>(T value, Stream stream, Mode mode = Mode.MemberMode) => new Writer(stream, mode).Serialize(value);
        public static void Serialize<T>(T value, Stream stream, bool simple, Mode mode = Mode.MemberMode) => new Writer(stream, simple, mode).Serialize(value);
        public static void Serialize(object value, Stream stream, Mode mode = Mode.MemberMode) => new Writer(stream, mode).Serialize(value);
        public static void Serialize(object value, Stream stream, bool simple, Mode mode = Mode.MemberMode) => new Writer(stream, simple, mode).Serialize(value);
        public static ArraySegment<byte> Serialize<T>(T value, Mode mode = Mode.MemberMode) {
            using (MemoryStream stream = new MemoryStream()) {
                Serialize(value, stream, mode);
                return stream.GetArraySegment();
            }
        }
        public static ArraySegment<byte> Serialize<T>(T value, bool simple, Mode mode = Mode.MemberMode) {
            using (MemoryStream stream = new MemoryStream()) {
                Serialize(value, stream, simple, mode);
                return stream.GetArraySegment();
            }
        }
        public static ArraySegment<byte> Serialize(object value, Mode mode = Mode.MemberMode) {
            using (MemoryStream stream = new MemoryStream()) {
                Serialize(value, stream, mode);
                return stream.GetArraySegment();
            }
        }
        public static ArraySegment<byte> Serialize(object value, bool simple, Mode mode = Mode.MemberMode) {
            using (MemoryStream stream = new MemoryStream()) {
                Serialize(value, stream, simple, mode);
                return stream.GetArraySegment();
            }
        }
        public static T Deserialize<T>(Stream stream, Mode mode = Mode.MemberMode) => new Reader(stream, mode).Deserialize<T>();
        public static T Deserialize<T>(byte[] data, Mode mode = Mode.MemberMode) {
            using (MemoryStream stream = new MemoryStream(data, 0, data.Length, false, true)) {
                return Deserialize<T>(stream, mode);
            }
        }
        public static T Deserialize<T>(ArraySegment<byte> data, Mode mode = Mode.MemberMode) {
            using(MemoryStream stream = new MemoryStream(data.Array, data.Offset, data.Count, false, true)) {
                return Deserialize<T>(stream, mode);
            }
        }
        public static T Deserialize<T>(Stream stream, bool simple, Mode mode = Mode.MemberMode) => new Reader(stream, simple, mode).Deserialize<T>();
        public static T Deserialize<T>(byte[] data, bool simple, Mode mode = Mode.MemberMode) {
            using (MemoryStream stream = new MemoryStream(data, 0, data.Length, false, true)) {
                return Deserialize<T>(stream, simple, mode);
            }
        }
        public static T Deserialize<T>(ArraySegment<byte> data, bool simple, Mode mode = Mode.MemberMode) {
            using (MemoryStream stream = new MemoryStream(data.Array, data.Offset, data.Count, false, true)) {
                return Deserialize<T>(stream, simple, mode);
            }
        }
        public static object Deserialize(Stream stream, Type type = null, Mode mode = Mode.MemberMode) => new Reader(stream, mode).Deserialize(type);
        public static object Deserialize(byte[] data, Type type = null, Mode mode = Mode.MemberMode) {
            using (MemoryStream stream = new MemoryStream(data, 0, data.Length, false, true)) {
                return Deserialize(stream, type, mode);
            }
        }
        public static object Deserialize(ArraySegment<byte> data, Type type = null, Mode mode = Mode.MemberMode) {
            using (MemoryStream stream = new MemoryStream(data.Array, data.Offset, data.Count, false, true)) {
                return Deserialize(stream, type, mode);
            }
        }
        public static object Deserialize(Stream stream, Type type, bool simple, Mode mode = Mode.MemberMode) => new Reader(stream, simple, mode).Deserialize(type);
        public static object Deserialize(byte[] data, Type type, bool simple, Mode mode = Mode.MemberMode) {
            using (MemoryStream stream = new MemoryStream(data, 0, data.Length, false, true)) {
                return Deserialize(stream, type, simple, mode);
            }
        }
        public static object Deserialize(ArraySegment<byte> data, Type type, bool simple, Mode mode = Mode.MemberMode) {
            using (MemoryStream stream = new MemoryStream(data.Array, data.Offset, data.Count, false, true)) {
                return Deserialize(stream, type, simple, mode);
            }
        }
    }
}