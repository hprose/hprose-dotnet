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
 * LastModified: Apr 1, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

using Hprose.Collections.Generic;

namespace Hprose.IO.Serializers {
    internal interface ISerializer {
        void Write(Writer writer, object obj);
    }

    public abstract class Serializer<T> : ISerializer {
        static Serializer() => Serializer.Initialize();
        private static volatile Serializer<T> _instance;
        public static Serializer<T> Instance {
            get {
                if (_instance == null) {
                    _instance = Serializer.GetInstance(typeof(T)) as Serializer<T>;
                }
                return _instance;
            }
        }
        public abstract void Write(Writer writer, T obj);
        void ISerializer.Write(Writer writer, object obj) => Write(writer, (T)obj);
    }

    public class Serializer : Serializer<object> {
        static readonly ConcurrentDictionary<Type, Lazy<ISerializer>> _serializers = new ConcurrentDictionary<Type, Lazy<ISerializer>>();
        static Serializer() {
            Register(() => new Serializer());
            Register(() => new DBNullSerializer());
            Register(() => new BooleanSerializer());
            Register(() => new CharSerializer());
            Register(() => new ByteSerializer());
            Register(() => new SByteSerializer());
            Register(() => new Int16Serializer());
            Register(() => new UInt16Serializer());
            Register(() => new Int32Serializer());
            Register(() => new UInt32Serializer());
            Register(() => new Int64Serializer());
            Register(() => new UInt64Serializer());
            Register(() => new SingleSerializer());
            Register(() => new DoubleSerializer());
            Register(() => new DecimalSerializer());
            Register(() => new IntPtrSerializer());
            Register(() => new UIntPtrSerializer());
            Register(() => new BigIntegerSerializer());
            Register(() => new TimeSpanSerializer());
            Register(() => new DateTimeSerializer());
            Register(() => new GuidSerializer());
            Register(() => new StringSerializer());
            Register(() => new StringBuilderSerializer());
            Register(() => new CharsSerializer());
            Register(() => new BytesSerializer());
            Register(() => new ValueTupleSerializer());
            Register(() => new BitArraySerializer());
        }

        public static void Initialize() { }

        public static void Register<T>(Func<Serializer<T>> ctor) {
            _serializers[typeof(T)] = new Lazy<ISerializer>(ctor);
        }

        private static Type GetSerializerType(Type type) {
            if (type.IsEnum) {
                return typeof(EnumSerializer<>).MakeGenericType(type);
            }
            if (type.IsArray) {
                return typeof(ArraySerializer<>).MakeGenericType(type.GetElementType());
            }
            if (type.IsConstructedGenericType) {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(ValueTuple<>) ||
                    genericType == typeof(ValueTuple<,>) ||
                    genericType == typeof(ValueTuple<,,>) ||
                    genericType == typeof(ValueTuple<,,,>) ||
                    genericType == typeof(ValueTuple<,,,,>) ||
                    genericType == typeof(ValueTuple<,,,,,>) ||
                    genericType == typeof(ValueTuple<,,,,,,>) ||
                    genericType == typeof(ValueTuple<,,,,,,,>)) {
                    return typeof(ValueTupleSerializer<>).MakeGenericType(type);
                }
                if (genericType == typeof(Tuple<>) ||
                    genericType == typeof(Tuple<,>) ||
                    genericType == typeof(Tuple<,,>) ||
                    genericType == typeof(Tuple<,,,>) ||
                    genericType == typeof(Tuple<,,,,>) ||
                    genericType == typeof(Tuple<,,,,,>) ||
                    genericType == typeof(Tuple<,,,,,,>) ||
                    genericType == typeof(Tuple<,,,,,,,>)) {
                    return typeof(TupleSerializer<>).MakeGenericType(type);
                }
                Type[] genericArgs = type.GetGenericArguments();
                if (genericType == typeof(Nullable<>)) {
                    return typeof(NullableSerializer<>).MakeGenericType(genericArgs);
                }
                if (genericType == typeof(NullableKey<>)) {
                    return typeof(NullableKeySerializer<>).MakeGenericType(genericArgs);
                }
                switch (genericArgs.Length) {
                    case 1:
                        bool isGenericCollection = typeof(ICollection<>).MakeGenericType(genericArgs).IsAssignableFrom(type);
                        bool isGenericIEnumerable = typeof(ICollection).IsAssignableFrom(type) &&
                            typeof(IEnumerable<>).MakeGenericType(genericArgs).IsAssignableFrom(type);

                        if (isGenericCollection || isGenericIEnumerable) {
                            if (genericArgs[0].IsConstructedGenericType) {
                                Type genType = genericArgs[0].GetGenericTypeDefinition();
                                if (genType == typeof(KeyValuePair<,>)) {
                                    Type[] genArgs = genericArgs[0].GetGenericArguments();
                                    return typeof(DictionarySerializer<,,>).MakeGenericType(type, genArgs[0], genArgs[1]);
                                }
                            }
                        }
                        if (isGenericCollection) {
                            return typeof(CollectionSerializer<,>).MakeGenericType(type, genericArgs[0]);
                        }
                        if (isGenericIEnumerable) {
                            return typeof(EnumerableSerializer<,>).MakeGenericType(type, genericArgs[0]);
                        }
                        break;
                    case 2:
                        if (typeof(IDictionary<,>).MakeGenericType(genericArgs).IsAssignableFrom(type)) {
                            return typeof(DictionarySerializer<,,>).MakeGenericType(type, genericArgs[0], genericArgs[1]);
                        }
                        break;
                }
            }
            if (typeof(IDictionary).IsAssignableFrom(type)) {
                return typeof(DictionarySerializer<>).MakeGenericType(type);
            }
            if (typeof(ICollection).IsAssignableFrom(type) && typeof(IEnumerable).IsAssignableFrom(type)) {
                return typeof(EnumerableSerializer<>).MakeGenericType(type);
            }
            if (typeof(Stream).IsAssignableFrom(type)) {
                return typeof(StreamSerializer<>).MakeGenericType(type);
            }
            return null;
        }

        internal static ISerializer GetInstance(Type type) {
            return _serializers.GetOrAdd(type, t => new Lazy<ISerializer>(
                () => Activator.CreateInstance(GetSerializerType(t)) as ISerializer
            )).Value;
        }

        public override void Write(Writer writer, object obj) {
            if (obj == null) {
                writer.Stream.WriteByte(HproseTags.TagNull);
            }
            else {
                GetInstance(obj.GetType()).Write(writer, obj);
            }
        }
    }
}
