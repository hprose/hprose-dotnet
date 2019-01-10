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
 * Writer.cs                                              *
 *                                                        *
 * hprose Writer class for C#.                            *
 *                                                        *
 * LastModified: Jan 10, 2019                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using Hprose.IO.Serializers;

namespace Hprose.IO {
    using static Tags;

    sealed class WriterRefer {
        private readonly Dictionary<object, int> _ref = new Dictionary<object, int>();
        private int _last = 0;
        public void AddCount(int count) => _last += count;
        public void Set(object obj) => _ref[obj] = _last++;
        public bool Write(Stream stream, object obj) {
            if (_ref.TryGetValue(obj, out int r)) {
                stream.WriteByte(TagRef);
                ValueWriter.WriteInt(stream, r);
                stream.WriteByte(TagSemicolon);
                return true;
            }
            return false;
        }
        public void Reset() {
            _ref.Clear();
            _last = 0;
        }
    }

    public class Writer {
        private readonly WriterRefer _refer;
        private readonly Dictionary<object, int> _ref = new Dictionary<object, int>();
        private int _last = 0;

        public Stream Stream { get; private set; }
        public Mode Mode { get; private set; }

        public Writer(Stream stream, Mode mode = Mode.MemberMode) {
            Stream = stream;
            _refer = new WriterRefer();
            Mode = mode;
        }

        public Writer(Stream stream, bool simple, Mode mode = Mode.MemberMode) {
            Stream = stream;
            _refer = simple ? null : new WriterRefer();
            Mode = mode;
        }

        public void Serialize(object obj) => Serializer.Instance.Serialize(this, obj);

        public void Serialize<T>(T obj) => Serializer<T>.Instance.Serialize(this, obj);

        public void Write(object obj) => Serializer.Instance.Write(this, obj);

        public void Write<T>(T obj) => Serializer<T>.Instance.Write(this, obj);

        public bool WriteReference(object obj) => _refer?.Write(Stream, obj) ?? false;

        public void SetReference(object obj) => _refer?.Set(obj);

        public void AddReferenceCount(int count) => _refer?.AddCount(count);

        public int WriteClass(object type, Action action) {
            if (!_ref.TryGetValue(type, out int r)) {
                action();
                r = _last++;
                _ref[type] = r;
            }
            return r;
        }

        public void Reset() {
            _refer?.Reset();
            _ref.Clear();
            _last = 0;
        }
    }
}
