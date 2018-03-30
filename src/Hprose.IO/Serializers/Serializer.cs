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
 * Serializer.cs                                          *
 *                                                        *
 * hprose Serializer class for C#.                        *
 *                                                        *
 * LastModified: Mar 30, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;
using System.Collections.Concurrent;
using System.Numerics;
using System.Text;

namespace Hprose.IO.Serializers {
    public abstract class Serializer {
        internal static readonly ConcurrentDictionary<Type, Serializer> _serializers = new ConcurrentDictionary<Type, Serializer>();
        static Serializer() {
            Register(new DBNullSerializer());
            Register(new BooleanSerializer());
            Register(new CharSerializer());
            Register(new ByteSerializer());
            Register(new SByteSerializer());
            Register(new Int16Serializer());
            Register(new UInt16Serializer());
            Register(new Int32Serializer());
            Register(new UInt32Serializer());
            Register(new Int64Serializer());
            Register(new UInt64Serializer());
            Register(new SingleSerializer());
            Register(new DoubleSerializer());
            Register(new DecimalSerializer());
            Register(new IntPtrSerializer());
            Register(new UIntPtrSerializer());
            Register(new BigIntegerSerializer());
            Register(new TimeSpanSerializer());
            Register(new DateTimeSerializer());
            Register(new GuidSerializer());
            Register(new StringSerializer());
            Register(new StringBuilderSerializer());
            Register(new CharsSerializer());
            Register(new BytesSerializer());

            Register(new NullableSerializer<bool>());
            Register(new NullableSerializer<char>());
            Register(new NullableSerializer<byte>());
            Register(new NullableSerializer<sbyte>());
            Register(new NullableSerializer<short>());
            Register(new NullableSerializer<ushort>());
            Register(new NullableSerializer<int>());
            Register(new NullableSerializer<uint>());
            Register(new NullableSerializer<long>());
            Register(new NullableSerializer<ulong>());
            Register(new NullableSerializer<float>());
            Register(new NullableSerializer<double>());
            Register(new NullableSerializer<decimal>());
            Register(new NullableSerializer<IntPtr>());
            Register(new NullableSerializer<UIntPtr>());
            Register(new NullableSerializer<BigInteger>());
            Register(new NullableSerializer<TimeSpan>());
            Register(new NullableSerializer<DateTime>());
            Register(new NullableSerializer<Guid>());

            Register(new NullableKeySerializer<bool?>());
            Register(new NullableKeySerializer<char?>());
            Register(new NullableKeySerializer<byte?>());
            Register(new NullableKeySerializer<sbyte?>());
            Register(new NullableKeySerializer<short?>());
            Register(new NullableKeySerializer<ushort?>());
            Register(new NullableKeySerializer<int?>());
            Register(new NullableKeySerializer<uint?>());
            Register(new NullableKeySerializer<long?>());
            Register(new NullableKeySerializer<ulong?>());
            Register(new NullableKeySerializer<float?>());
            Register(new NullableKeySerializer<double?>());
            Register(new NullableKeySerializer<decimal?>());
            Register(new NullableKeySerializer<IntPtr?>());
            Register(new NullableKeySerializer<UIntPtr?>());
            Register(new NullableKeySerializer<BigInteger?>());
            Register(new NullableKeySerializer<TimeSpan?>());
            Register(new NullableKeySerializer<DateTime?>());
            Register(new NullableKeySerializer<Guid?>());
            Register(new NullableKeySerializer<string>());
            Register(new NullableKeySerializer<StringBuilder>());
            Register(new NullableKeySerializer<char[]>());
            Register(new NullableKeySerializer<byte[]>());
        }

        public static void Initialize() { }

        public static void Register<T>(Serializer<T> serializer) {
            Serializer<T>._instance = serializer;
            _serializers[typeof(T)] = serializer;
        }
        public static Serializer Get(Type type) {
            if (type == null) {
                return NullSerializer.Instance;
            }
            return _serializers[type];
        }
        public abstract void Write(Writer writer, object obj);
    }

    public abstract class Serializer<T> : Serializer {
        static Serializer() => Initialize();
        internal static Serializer<T> _instance;
        public static Serializer<T> Instance => _instance;
        public abstract void Write(Writer writer, T obj);
        public override void Write(Writer writer, object obj) => Write(writer, (T)obj);
    }
}
