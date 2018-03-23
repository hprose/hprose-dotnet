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
 * NullableKey.cs                                         *
 *                                                        *
 * NullableKey class for C#.                              *
 *                                                        *
 * LastModified: Mar 8, 2018                              *
 * Authors: Ma Bingyao <andot@hprose.com>                 *
 *                                                        *
\**********************************************************/
using System;
using System.Collections.Generic;

namespace Hprose.Collections.Generic {
    public struct NullableKey<T> : IComparable<NullableKey<T>>, IEquatable<NullableKey<T>> {
        private readonly T _value;

        public NullableKey(T value) {
            _value = value;
        }

        public T Value {
            get { return _value; }
        }

        public int CompareTo(NullableKey<T> other) {
            return Comparer<T>.Default.Compare(_value, other._value);
        }

        public override bool Equals(object obj) {
            return obj is NullableKey<T> && Equals((NullableKey<T>)obj);
        }

        public bool Equals(NullableKey<T> other) {
            return _value == null ? other._value == null : _value.Equals(other._value);
        }

        public override int GetHashCode() {
            return _value == null ? 0 : _value.GetHashCode();
        }

        public override string ToString() {
            return _value == null ? "<null>" : _value.ToString();
        }

        public static implicit operator T(NullableKey<T> key) {
            return key._value;
        }

        public static implicit operator NullableKey<T>(T value) {
            return new NullableKey<T>(value);
        }

        public static bool operator ==(NullableKey<T> x, NullableKey<T> y) {
            return x.Equals(y);
        }

        public static bool operator !=(NullableKey<T> x, NullableKey<T> y) {
            return !x.Equals(y);
        }
    }
}
