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
 * LastModified: Apr 7, 2018                              *
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
        private readonly HproseMode _mode;
        private readonly Dictionary<object, int> _ref = new Dictionary<object, int>();
        private int _last = 0;

        public Stream Stream => _stream;
        public HproseMode Mode => _mode;

        public Writer(Stream stream, bool simple = false, HproseMode mode = HproseMode.MemberMode) {
            _stream = stream;
            _refer = simple ? null : new WriterRefer();
            _mode = mode;
        }

        public void Serialize(object obj) => Serializer.Instance.Serialize(this, obj);

        public void Serialize<T>(T obj) => Serializer<T>.Instance.Serialize(this, obj);

        internal bool WriteRef(object obj) => _refer?.Write(_stream, obj) ?? false;

        internal void SetRef(object obj) => _refer?.Set(obj);

        public void AddCount(int count) => _refer?.AddCount(count);

        public int WriteMetaData(object type, Action action) {
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
