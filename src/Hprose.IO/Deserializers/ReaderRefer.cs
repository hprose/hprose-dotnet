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
 * ReaderRefer.cs                                         *
 *                                                        *
 * ReaderRefer class for C#.                              *
 *                                                        *
 * LastModified: Dec 13, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.Collections.Generic;

namespace Hprose.IO.Deserializers {
    sealed class ReaderRefer {
        private readonly List<object> _ref = new List<object>();
        public int LastIndex => _ref.Count - 1;
        public void Add(object obj) => _ref.Add(obj);
        public void Set(int index, object obj) => _ref[index] = obj;
        public object Read(int index) => _ref[index];
        public void Reset() => _ref.Clear();
    }
}
