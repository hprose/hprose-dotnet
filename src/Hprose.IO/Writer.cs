/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Writer.cs                                               |
|                                                          |
|  hprose Writer class for C#.                             |
|                                                          |
|  LastModified: Jan 30, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.IO;

namespace Hprose.IO {
    using static Tags;

    internal sealed class WriterRefer {
        private readonly Dictionary<object, int> @ref = new();
        private int last = 0;
        public void AddCount(int count) => last += count;
        public void Set(object obj) => @ref[obj] = last++;
        public bool Write(Stream stream, object obj) {
            if (@ref.TryGetValue(obj, out int r)) {
                stream.WriteByte(TagRef);
                ValueWriter.WriteInt(stream, r);
                stream.WriteByte(TagSemicolon);
                return true;
            }
            return false;
        }
        public void Reset() {
            @ref.Clear();
            last = 0;
        }
    }

    public class Writer {
        private volatile WriterRefer refer;
        private readonly Dictionary<object, int> @ref = new();
        private int last = 0;

        public Stream Stream { get; private set; }
        public Mode Mode { get; private set; }
        public bool Simple {
            get {
                return refer == null;
            }
            set {
                refer = value ? null : new WriterRefer();
            }
        }
        public Writer(Stream stream, Mode mode = Mode.MemberMode) {
            Stream = stream;
            refer = new WriterRefer();
            Mode = mode;
        }

        public Writer(Stream stream, bool simple, Mode mode = Mode.MemberMode) {
            Stream = stream;
            Simple = simple;
            Mode = mode;
        }

        public void Serialize(object obj) => Serializer.Instance.Serialize(this, obj);

        public void Serialize<T>(T obj) => Serializer<T>.Instance.Serialize(this, obj);

        public void Write(object obj) => Serializer.Instance.Write(this, obj);

        public void Write<T>(T obj) => Serializer<T>.Instance.Write(this, obj);

        public bool WriteReference(object obj) => refer?.Write(Stream, obj) ?? false;

        public void SetReference(object obj) => refer?.Set(obj);

        public void AddReferenceCount(int count) => refer?.AddCount(count);

        public int WriteClass(object type, Action action) {
            if (!@ref.TryGetValue(type, out int r)) {
                action();
                r = last++;
                @ref[type] = r;
            }
            return r;
        }

        public void Reset() {
            refer?.Reset();
            @ref.Clear();
            last = 0;
        }
    }
}
