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
 * LastModified: Apr 8, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.Collections.Generic;

namespace Hprose.IO.Deserializers {
    sealed class ReaderRefer {
        private readonly List<object> _ref = new List<object>();
        public void Set(object obj) => _ref.Add(obj);
        public object Read(int index) => _ref[index];
        public void Reset() => _ref.Clear();
    }
}
