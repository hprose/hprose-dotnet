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
 * LastModified: Dec 13, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Generic;
using System.IO;

namespace Hprose.IO.Serializers {
    public class Writer {
        private readonly Stream _stream;
        private readonly WriterRefer _refer;
        private readonly Mode _mode;
        private readonly Dictionary<object, int> _ref = new Dictionary<object, int>();
        private int _last = 0;

        public Stream Stream => _stream;
        public Mode Mode => _mode;

        public Writer(Stream stream, Mode mode = Mode.MemberMode) {
            _stream = stream;
            _refer = new WriterRefer();
            _mode = mode;
        }

        public Writer(Stream stream, bool simple, Mode mode = Mode.MemberMode) {
            _stream = stream;
            _refer = simple ? null : new WriterRefer();
            _mode = mode;
        }

        public void Serialize(object obj) => Serializer.Instance.Serialize(this, obj);

        public void Serialize<T>(T obj) => Serializer<T>.Instance.Serialize(this, obj);

        public void Write(object obj) => Serializer.Instance.Write(this, obj);

        public void Write<T>(T obj) => Serializer<T>.Instance.Write(this, obj);

        public bool WriteReference(object obj) => _refer?.Write(_stream, obj) ?? false;

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
