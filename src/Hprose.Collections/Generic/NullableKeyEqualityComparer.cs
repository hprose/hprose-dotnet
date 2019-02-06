/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  NullableKeyEqualityComparer.cs                          |
|                                                          |
|  NullableKeyEqualityComparer class for C#.               |
|                                                          |
|  LastModified: Mar 8, 2018                               |
|  Authors: Ma Bingyao <andot@hprose.com>                  |
|                                                          |
\*________________________________________________________*/
using System.Collections.Generic;

namespace Hprose.Collections.Generic {
    public class NullableKeyEqualityComparer<T> : IEqualityComparer<NullableKey<T>> {
        public IEqualityComparer<T> Comparer { get; private set; }

        public NullableKeyEqualityComparer(IEqualityComparer<T> comparer) => Comparer = comparer;

        public bool Equals(NullableKey<T> x, NullableKey<T> y) => Comparer?.Equals(x.Value, y.Value) ?? x.Equals(y);

        public int GetHashCode(NullableKey<T> obj) => (obj.Value != null) ? (Comparer?.GetHashCode(obj.Value) ?? obj.GetHashCode()) : 0;
    }
}
