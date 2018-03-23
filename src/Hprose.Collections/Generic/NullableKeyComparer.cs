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
 * NullableKeyComparer.cs                                 *
 *                                                        *
 * NullableKeyComparer class for C#.                      *
 *                                                        *
 * LastModified: Mar 8, 2018                              *
 * Authors: Ma Bingyao <andot@hprose.com>                 *
 *                                                        *
\**********************************************************/
using System.Collections.Generic;

namespace Hprose.Collections.Generic {
    public class NullableKeyComparer<T> : IComparer<NullableKey<T>> {
        private readonly IComparer<T> _comparer;

        public NullableKeyComparer(IComparer<T> comparer) {
            _comparer = comparer;
        }

        public int Compare(NullableKey<T> x, NullableKey<T> y) {
            return _comparer == null ? x.CompareTo(y) : _comparer.Compare(x.Value, y.Value);
        }
    }
}
