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
 * SerializerFactory.cs                                   *
 *                                                        *
 * hprose serializer factory for C#.                      *
 *                                                        *
 * LastModified: Mar 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;
using System.Collections.Concurrent;
using System.Numerics;
using Hprose.Collections.Generic;

namespace Hprose.IO.Serializers {
    public static class SerializerFactory {
        private static readonly ConcurrentDictionary<Type, Serializer> _serializers = new ConcurrentDictionary<Type, Serializer>();
        static SerializerFactory() {
            _serializers[typeof(void)] = NullSerializer.Instance;
            _serializers[typeof(DBNull)] = NullSerializer.Instance;
            _serializers[typeof(bool)] = BooleanSerializer.Instance;
            _serializers[typeof(char)] = CharSerializer.Instance;
            _serializers[typeof(byte)] = ByteSerializer.Instance;
            _serializers[typeof(sbyte)] = SByteSerializer.Instance;
            _serializers[typeof(short)] = Int16Serializer.Instance;
            _serializers[typeof(ushort)] = UInt16Serializer.Instance;
            _serializers[typeof(int)] = Int32Serializer.Instance;
            _serializers[typeof(uint)] = UInt32Serializer.Instance;
            _serializers[typeof(long)] = Int64Serializer.Instance;
            _serializers[typeof(ulong)] = UInt64Serializer.Instance;
            _serializers[typeof(float)] = SingleSerializer.Instance;
            _serializers[typeof(double)] = DoubleSerializer.Instance;
            _serializers[typeof(decimal)] = DecimalSerializer.Instance;
            _serializers[typeof(IntPtr)] = IntPtrSerializer.Instance;
            _serializers[typeof(UIntPtr)] = UIntPtrSerializer.Instance;
            _serializers[typeof(BigInteger)] = BigIntegerSerializer.Instance;
            _serializers[typeof(Guid)] = GuidSerializer.Instance;
            _serializers[typeof(TimeSpan)] = TimeSpanSerializer.Instance;

            _serializers[typeof(bool?)] = NullableSerializer<bool>.Instance;
            _serializers[typeof(char?)] = NullableSerializer<char>.Instance;
            _serializers[typeof(byte?)] = NullableSerializer<byte>.Instance;
            _serializers[typeof(sbyte?)] = NullableSerializer<sbyte>.Instance;
            _serializers[typeof(short?)] = NullableSerializer<short>.Instance;
            _serializers[typeof(ushort?)] = NullableSerializer<ushort>.Instance;
            _serializers[typeof(int?)] = NullableSerializer<int>.Instance;
            _serializers[typeof(uint?)] = NullableSerializer<uint>.Instance;
            _serializers[typeof(long?)] = NullableSerializer<long>.Instance;
            _serializers[typeof(ulong?)] = NullableSerializer<ulong>.Instance;
            _serializers[typeof(float?)] = NullableSerializer<float>.Instance;
            _serializers[typeof(double?)] = NullableSerializer<double>.Instance;
            _serializers[typeof(decimal?)] = NullableSerializer<decimal>.Instance;
            _serializers[typeof(IntPtr?)] = NullableSerializer<IntPtr>.Instance;
            _serializers[typeof(UIntPtr?)] = NullableSerializer<UIntPtr>.Instance;
            _serializers[typeof(BigInteger?)] = NullableSerializer<BigInteger>.Instance;
            _serializers[typeof(Guid?)] = NullableSerializer<Guid>.Instance;
            _serializers[typeof(TimeSpan?)] = NullableSerializer<TimeSpan>.Instance;

            _serializers[typeof(NullableKey<bool?>)] = NullableKeySerializer<bool?>.Instance;
            _serializers[typeof(NullableKey<char?>)] = NullableKeySerializer<char?>.Instance;
            _serializers[typeof(NullableKey<byte?>)] = NullableKeySerializer<byte?>.Instance;
            _serializers[typeof(NullableKey<sbyte?>)] = NullableKeySerializer<sbyte?>.Instance;
            _serializers[typeof(NullableKey<short?>)] = NullableKeySerializer<short?>.Instance;
            _serializers[typeof(NullableKey<ushort?>)] = NullableKeySerializer<ushort?>.Instance;
            _serializers[typeof(NullableKey<int?>)] = NullableKeySerializer<int?>.Instance;
            _serializers[typeof(NullableKey<uint?>)] = NullableKeySerializer<uint?>.Instance;
            _serializers[typeof(NullableKey<long?>)] = NullableKeySerializer<long?>.Instance;
            _serializers[typeof(NullableKey<ulong?>)] = NullableKeySerializer<ulong?>.Instance;
            _serializers[typeof(NullableKey<float?>)] = NullableKeySerializer<float?>.Instance;
            _serializers[typeof(NullableKey<double?>)] = NullableKeySerializer<double?>.Instance;
            _serializers[typeof(NullableKey<decimal?>)] = NullableKeySerializer<decimal?>.Instance;
            _serializers[typeof(NullableKey<IntPtr?>)] = NullableKeySerializer<IntPtr?>.Instance;
            _serializers[typeof(NullableKey<UIntPtr?>)] = NullableKeySerializer<UIntPtr?>.Instance;
            _serializers[typeof(NullableKey<BigInteger?>)] = NullableKeySerializer<BigInteger?>.Instance;
            _serializers[typeof(NullableKey<Guid?>)] = NullableKeySerializer<Guid?>.Instance;
            _serializers[typeof(NullableKey<TimeSpan?>)] = NullableKeySerializer<TimeSpan?>.Instance;
        }
        public static void Register(Type type, Serializer serializer) {
            _serializers[type] = serializer;
        }
        public static Serializer Get(Type type) {
            Serializer serializer = _serializers[type];
            if (serializer == null) {
                //    if (type.isEnum()) {
                //        serializer = EnumSerializer.instance;
                //    }
                //    else if (type.isArray()) {
                //        serializer = OtherTypeArraySerializer.instance;
                //    }
                //    else if (Map.class.isAssignableFrom(type)) {
                //    serializer = MapSerializer.instance;
                //}
                //else if (List.class.isAssignableFrom(type)) {
                //    serializer = ListSerializer.instance;
                //}
                //else if (Collection.class.isAssignableFrom(type)) {
                //    serializer = CollectionSerializer.instance;
                //}
                //else if (TimeZone.class.isAssignableFrom(type)) {
                //    serializer = TimeZoneSerializer.instance;
                //}
                //else if (Calendar.class.isAssignableFrom(type)) {
                //    serializer = CalendarSerializer.instance;
                //}
                //else {
                //    serializer = OtherTypeSerializer.instance;
                //}
                //serializers.put(type, serializer);
                //}
            }
            return serializer;
        }
    }
}
