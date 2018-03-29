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
 * WriterRefer.cs                                         *
 *                                                        *
 * WriterRefer class for C#.                              *
 *                                                        *
 * LastModified: Mar 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System.IO;
using System.Collections.Generic;
using static Hprose.IO.HproseTags;

namespace Hprose.IO.Serializers {
    sealed class WriterRefer {
        private readonly Dictionary<object, int> _ref = new Dictionary<object, int>();
        private int _last = 0;
        public void AddCount(int count) {
            _last += count;
        }
        public void Set(object obj) {
            _ref[obj] = _last++;
        }
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
}
