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
using System.Collections.Generic;
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
        }

        public static void Initialize() { }

        public static void Register<T>(Func<Serializer<T>> ctor) {
            _serializers[typeof(T)] = new Lazy<ISerializer>(ctor);
        }

        private static ISerializer NewInstance(Type type) {
            Type serializerType = null;
            if (type.IsEnum) {
                serializerType = typeof(EnumSerializer<>).MakeGenericType(type);
            }
            else if (type.IsArray) {
                serializerType = typeof(ArraySerializer<>).MakeGenericType(type.GetElementType());
            }
            else if (type.IsConstructedGenericType) {
                Type genericType = type.GetGenericTypeDefinition();
                Type[] genericArgs = type.GetGenericArguments();
                switch (genericArgs.Length) {
                    case 1:
                        if (genericType == typeof(Nullable<>)) {
                            serializerType = typeof(NullableSerializer<>).MakeGenericType(genericArgs);
                        }
                        else if (genericType == typeof(NullableKey<>)) {
                            serializerType = typeof(NullableKeySerializer<>).MakeGenericType(genericArgs);
                        }
                        else if (genericType == typeof(ValueTuple<>)) {
                            serializerType = typeof(ValueTupleSerializer<>).MakeGenericType(genericArgs);
                        }
                        else if (typeof(ICollection<>).MakeGenericType(genericArgs).IsAssignableFrom(type)) {
                            if (genericArgs[0].IsConstructedGenericType) {
                                Type genType = genericArgs[0].GetGenericTypeDefinition();
                                if (genType == typeof(KeyValuePair<,>)) {
                                    Type[] genArgs = genericArgs[0].GetGenericArguments();
                                    serializerType = typeof(DictionarySerializer<,,>).MakeGenericType(type, genArgs[0], genArgs[1]);
                                }
                            }
                            if (serializerType == null) {
                                serializerType = typeof(CollectionSerializer<,>).MakeGenericType(type, genericArgs[0]);
                            }
                        }
                        break;
                    case 2:
                        if (genericType == typeof(ValueTuple<,>)) {
                            serializerType = typeof(ValueTupleSerializer<,>).MakeGenericType(genericArgs);
                        }
                        else if (typeof(IDictionary<,>).MakeGenericType(genericArgs).IsAssignableFrom(type)) {
                            serializerType = typeof(DictionarySerializer<,,>).MakeGenericType(type, genericArgs[0], genericArgs[1]);
                        }
                        break;
                    case 3:
                        if (genericType == typeof(ValueTuple<,,>)) {
                            serializerType = typeof(ValueTupleSerializer<,,>).MakeGenericType(genericArgs);
                        }
                        break;
                    case 4:
                        if (genericType == typeof(ValueTuple<,,,>)) {
                            serializerType = typeof(ValueTupleSerializer<,,,>).MakeGenericType(genericArgs);
                        }
                        break;
                    case 5:
                        if (genericType == typeof(ValueTuple<,,,,>)) {
                            serializerType = typeof(ValueTupleSerializer<,,,,>).MakeGenericType(genericArgs);
                        }
                        break;
                    case 6:
                        if (genericType == typeof(ValueTuple<,,,,,>)) {
                            serializerType = typeof(ValueTupleSerializer<,,,,,>).MakeGenericType(genericArgs);
                        }
                        break;
                    case 7:
                        if (genericType == typeof(ValueTuple<,,,,,,>)) {
                            serializerType = typeof(ValueTupleSerializer<,,,,,,>).MakeGenericType(genericArgs);
                        }
                        break;
                    case 8:
                        if (genericType == typeof(ValueTuple<,,,,,,,>)) {
                            serializerType = typeof(ValueTupleSerializer<,,,,,,,>).MakeGenericType(genericArgs);
                        }
                        break;
                }
            }
            return Activator.CreateInstance(serializerType) as ISerializer;
        }

        internal static ISerializer GetInstance(Type type) {
            return _serializers.GetOrAdd(type, t => new Lazy<ISerializer>(() => NewInstance(t))).Value;
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
