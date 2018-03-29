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

        public IEqualityComparer<T> Comparer => _comparer;

        public NullableKeyEqualityComparer(IEqualityComparer<T> comparer) => _comparer = comparer;

        public bool Equals(NullableKey<T> x, NullableKey<T> y) => _comparer?.Equals(x.Value, y.Value) ?? x.Equals(y);

        public int GetHashCode(NullableKey<T> obj) => (obj.Value != null) ? (_comparer?.GetHashCode(obj.Value) ?? obj.GetHashCode()) : 0;
    }
}
