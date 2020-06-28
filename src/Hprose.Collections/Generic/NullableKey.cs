/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  NullableKey.cs                                          |
|                                                          |
|  NullableKey class for C#.                               |
|                                                          |
|  LastModified: Jun 28, 2020                              |
|  Authors: Ma Bingyao <andot@hprose.com>                  |
|                                                          |
\*________________________________________________________*/
using System;
using System.Collections.Generic;

namespace Hprose.Collections.Generic {
    public struct NullableKey<T> : IComparable<NullableKey<T>>, IEquatable<NullableKey<T>> {
        private readonly T _value;

        public NullableKey(T value) => _value = value;

        public T Value => _value;

        public int CompareTo(NullableKey<T> other) => Comparer<T>.Default.Compare(_value, other._value);

        public override bool Equals(object obj) => obj is NullableKey<T> key && Equals(key);

        public bool Equals(NullableKey<T> other) => _value?.Equals(other._value) ?? other._value == null;

        public override int GetHashCode() => _value?.GetHashCode() ?? 0;

        public override string ToString() => _value?.ToString() ?? "<null>";

        public static implicit operator T(NullableKey<T> key) => key._value;

        public static implicit operator NullableKey<T>(T value) => new NullableKey<T>(value);

        public static bool operator ==(NullableKey<T> x, NullableKey<T> y) => x.Equals(y);

        public static bool operator !=(NullableKey<T> x, NullableKey<T> y) => !x.Equals(y);

        public static bool operator <(NullableKey<T> left, NullableKey<T> right) => left.CompareTo(right) < 0;

        public static bool operator <=(NullableKey<T> left, NullableKey<T> right) => left.CompareTo(right) <= 0;

        public static bool operator >(NullableKey<T> left, NullableKey<T> right) => left.CompareTo(right) > 0;

        public static bool operator >=(NullableKey<T> left, NullableKey<T> right) => left.CompareTo(right) >= 0;

        public T FromNullableKey(NullableKey<T> key) => key._value;

        public NullableKey<T> ToNullableKey(T value) => new NullableKey<T>(value);
    }
}
