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
 * NullableKeyEqualityComparer.cs                         *
 *                                                        *
 * NullableKeyEqualityComparer class for C#.              *
 *                                                        *
 * LastModified: Mar 8, 2018                              *
 * Authors: Ma Bingyao <andot@hprose.com>                 *
 *                                                        *
\**********************************************************/
using System.Collections.Generic;

namespace Hprose.Collections.Generic {
    public class NullableKeyEqualityComparer<T> : IEqualityComparer<NullableKey<T>> {
        private readonly IEqualityComparer<T> _comparer;

        public IEqualityComparer<T> Comparer {
            get { return _comparer; }
        }

        public NullableKeyEqualityComparer(IEqualityComparer<T> comparer) {
            _comparer = comparer;
        }

        public bool Equals(NullableKey<T> x, NullableKey<T> y) {
            return _comparer == null ? x.Equals(y) : _comparer.Equals(x.Value, y.Value);
        }

        public int GetHashCode(NullableKey<T> obj) {
            if (obj.Value == null) return 0;
            return _comparer == null ? obj.GetHashCode() : _comparer.GetHashCode(obj.Value);
        }
    }
}
