﻿/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Formatter.cs                                            |
|                                                          |
|  hprose Formatter for C#.                                |
|                                                          |
|  LastModified: Jul 2, 2020                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;
using System.Threading;

namespace Hprose.IO {
    public static class Formatter {
        private static readonly ThreadLocal<MemoryStream> memoryStream = new(() => new MemoryStream());
        public static void Serialize<T>(T value, Stream stream, Mode mode = Mode.MemberMode) => new Writer(stream, mode).Serialize(value);
        public static void Serialize<T>(T value, Stream stream, bool simple, Mode mode = Mode.MemberMode) => new Writer(stream, simple, mode).Serialize(value);
        public static void Serialize(object value, Stream stream, Mode mode = Mode.MemberMode) => new Writer(stream, mode).Serialize(value);
        public static void Serialize(object value, Stream stream, bool simple, Mode mode = Mode.MemberMode) => new Writer(stream, simple, mode).Serialize(value);
        public static byte[] Serialize<T>(T value, Mode mode = Mode.MemberMode) {
            var stream = memoryStream.Value;
            stream.SetLength(0);
            Serialize(value, stream, mode);
            return stream.ToArray();
        }
        public static byte[] Serialize<T>(T value, bool simple, Mode mode = Mode.MemberMode) {
            var stream = memoryStream.Value;
            stream.SetLength(0);
            Serialize(value, stream, simple, mode);
            return stream.ToArray();
        }
        public static byte[] Serialize(object value, Mode mode = Mode.MemberMode) {
            var stream = memoryStream.Value;
            stream.SetLength(0);
            Serialize(value, stream, mode);
            return stream.ToArray();
        }
        public static byte[] Serialize(object value, bool simple, Mode mode = Mode.MemberMode) {
            var stream = memoryStream.Value;
            stream.SetLength(0);
            Serialize(value, stream, simple, mode);
            return stream.ToArray();
        }
        public static T Deserialize<T>(Stream stream, Mode mode = Mode.MemberMode) => new Reader(stream, mode).Deserialize<T>();
        public static T Deserialize<T>(byte[] data, Mode mode = Mode.MemberMode) {
            using MemoryStream stream = new(data, 0, data.Length, false, true);
            return Deserialize<T>(stream, mode);
        }
        public static T Deserialize<T>(ArraySegment<byte> data, Mode mode = Mode.MemberMode) {
            using MemoryStream stream = new(data.Array, data.Offset, data.Count, false, true);
            return Deserialize<T>(stream, mode);
        }
        public static T Deserialize<T>(Stream stream, bool simple, Mode mode = Mode.MemberMode) => new Reader(stream, simple, mode).Deserialize<T>();
        public static T Deserialize<T>(byte[] data, bool simple, Mode mode = Mode.MemberMode) {
            using MemoryStream stream = new(data, 0, data.Length, false, true);
            return Deserialize<T>(stream, simple, mode);
        }
        public static T Deserialize<T>(ArraySegment<byte> data, bool simple, Mode mode = Mode.MemberMode) {
            using MemoryStream stream = new(data.Array, data.Offset, data.Count, false, true);
            return Deserialize<T>(stream, simple, mode);
        }
        public static object Deserialize(Stream stream, Type type = null, Mode mode = Mode.MemberMode) => new Reader(stream, mode).Deserialize(type);
        public static object Deserialize(byte[] data, Type type = null, Mode mode = Mode.MemberMode) {
            using MemoryStream stream = new(data, 0, data.Length, false, true);
            return Deserialize(stream, type, mode);
        }
        public static object Deserialize(ArraySegment<byte> data, Type type = null, Mode mode = Mode.MemberMode) {
            using MemoryStream stream = new(data.Array, data.Offset, data.Count, false, true);
            return Deserialize(stream, type, mode);
        }
        public static object Deserialize(Stream stream, Type type, bool simple, Mode mode = Mode.MemberMode) => new Reader(stream, simple, mode).Deserialize(type);
        public static object Deserialize(byte[] data, Type type, bool simple, Mode mode = Mode.MemberMode) {
            using MemoryStream stream = new(data, 0, data.Length, false, true);
            return Deserialize(stream, type, simple, mode);
        }
        public static object Deserialize(ArraySegment<byte> data, Type type, bool simple, Mode mode = Mode.MemberMode) {
            using MemoryStream stream = new(data.Array, data.Offset, data.Count, false, true);
            return Deserialize(stream, type, simple, mode);
        }
    }
}