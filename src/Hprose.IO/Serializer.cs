/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Serializer.cs                                           |
|                                                          |
|  hprose Serializer class for C#.                         |
|                                                          |
|  LastModified: Jan 19, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.Collections.Generic;
using Hprose.IO.Serializers;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Hprose.IO {
    public interface ISerializer {
        void Write(Writer writer, object obj);
        void Serialize(Writer writer, object obj);
    }

    public abstract class Serializer<T> : ISerializer {
        static Serializer() => Serializer.Initialize();
        private static volatile Serializer<T> instance;
        public static Serializer<T> Instance {
            get {
                if (instance == null) {
                    instance = Serializer.GetInstance(typeof(T)) as Serializer<T>;
                }
                return instance;
            }
        }
        public abstract void Write(Writer writer, T obj);
        public virtual void Serialize(Writer writer, T obj) => Write(writer, obj);
        void ISerializer.Write(Writer writer, object obj) => Write(writer, (T)obj);
        void ISerializer.Serialize(Writer writer, object obj) => Serialize(writer, (T)obj);
    }

    public class Serializer : Serializer<object> {
        private static readonly ConcurrentDictionary<Type, Lazy<ISerializer>> serializers = new ConcurrentDictionary<Type, Lazy<ISerializer>>();
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
            Register(() => new ExpandoObjectSerializer());
        }

        public static void Initialize() { }

        public static void Register<T>(Func<Serializer<T>> ctor) => serializers[typeof(T)] = new Lazy<ISerializer>(ctor);

        private static Type GetSerializerType(Type type) {
            if (type.IsEnum) {
                return typeof(EnumSerializer<>).MakeGenericType(type);
            }
            if (type.IsArray) {
                switch (type.GetArrayRank()) {
                    case 1:
                        return typeof(ArraySerializer<>).MakeGenericType(type.GetElementType());
                    case 2:
                        return typeof(Array2Serializer<>).MakeGenericType(type.GetElementType());
                    default:
                        return typeof(MultiDimArraySerializer<>).MakeGenericType(type);
                }
            }
            if (type.IsGenericType) {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType.Name.StartsWith("ValueTuple`")) {
                    return typeof(ValueTupleSerializer<>).MakeGenericType(type);
                }
                if (genericType.Name.StartsWith("Tuple`")) {
                    return typeof(TupleSerializer<>).MakeGenericType(type);
                }
                Type[] genericArgs = type.GetGenericArguments();
                if (genericType == typeof(Nullable<>)) {
                    return typeof(NullableSerializer<>).MakeGenericType(genericArgs);
                }
                if (genericType == typeof(NullableKey<>)) {
                    return typeof(NullableKeySerializer<>).MakeGenericType(genericArgs);
                }
                if (genericType == typeof(ArraySegment<>)) {
                    return typeof(ArraySegmentSerializer<>).MakeGenericType(genericArgs);
                }
                switch (genericArgs.Length) {
                    case 1:
                        bool isGenericCollection = typeof(ICollection<>).MakeGenericType(genericArgs).IsAssignableFrom(type);
                        bool isGenericIEnumerable = typeof(IEnumerable<>).MakeGenericType(genericArgs).IsAssignableFrom(type);
                        if (isGenericCollection) {
                            if (genericArgs[0].IsGenericType) {
                                Type genType = genericArgs[0].GetGenericTypeDefinition();
                                if (genType == typeof(KeyValuePair<,>)) {
                                    Type[] genArgs = genericArgs[0].GetGenericArguments();
                                    return typeof(DictionarySerializer<,,>).MakeGenericType(type, genArgs[0], genArgs[1]);
                                }
                            }
                            return typeof(CollectionSerializer<,>).MakeGenericType(type, genericArgs[0]);
                        }
                        if (isGenericIEnumerable) {
                            bool isCollection = typeof(ICollection).IsAssignableFrom(type);
                            if (genericArgs[0].IsGenericType) {
                                Type genType = genericArgs[0].GetGenericTypeDefinition();
                                if (genType == typeof(KeyValuePair<,>)) {
                                    Type[] genArgs = genericArgs[0].GetGenericArguments();
                                    if (isCollection) {
                                        return typeof(FastEnumerableSerializer<,,>).MakeGenericType(type, genArgs[0], genArgs[1]);
                                    }
                                    return typeof(EnumerableSerializer<,,>).MakeGenericType(type, genArgs[0], genArgs[1]);
                                }
                            }
                            if (isCollection) {
                                return typeof(FastEnumerableSerializer<,>).MakeGenericType(type, genericArgs[0]);
                            }
                            return typeof(EnumerableSerializer<,>).MakeGenericType(type, genericArgs[0]);
                        }
                        break;
                    case 2:
                        if (typeof(IDictionary<,>).MakeGenericType(genericArgs).IsAssignableFrom(type)) {
                            return typeof(DictionarySerializer<,,>).MakeGenericType(type, genericArgs[0], genericArgs[1]);
                        }
                        if (typeof(ICollection<>).MakeGenericType(genericArgs[0]).IsAssignableFrom(type)) {
                            return typeof(CollectionSerializer<,>).MakeGenericType(type, genericArgs[0]);
                        }
                        if (typeof(ICollection<>).MakeGenericType(genericArgs[1]).IsAssignableFrom(type)) {
                            return typeof(CollectionSerializer<,>).MakeGenericType(type, genericArgs[1]);
                        }
                        break;
                }
            }
            if (typeof(IDictionary).IsAssignableFrom(type)) {
                return typeof(DictionarySerializer<>).MakeGenericType(type);
            }
            if (typeof(IEnumerable).IsAssignableFrom(type)) {
                return typeof(EnumerableSerializer<>).MakeGenericType(type);
            }
            if (type.IsGenericType && type.Name.StartsWith("<>f__AnonymousType")) {
                return typeof(AnonymousTypeSerializer<>).MakeGenericType(type);
            }
            if (typeof(Stream).IsAssignableFrom(type)) {
                return typeof(StreamSerializer<>).MakeGenericType(type);
            }
            if (typeof(DataTable).IsAssignableFrom(type)) {
                return typeof(DataTableSerializer<>).MakeGenericType(type);
            }
            if (typeof(DataSet).IsAssignableFrom(type)) {
                return typeof(DataSetSerializer<>).MakeGenericType(type);
            }
            return typeof(ObjectSerializer<>).MakeGenericType(type);
        }

        private static readonly Func<Type, Lazy<ISerializer>> serializerFactory = (type) => new Lazy<ISerializer>(
                () => Activator.CreateInstance(GetSerializerType(type)) as ISerializer
            );

        public static ISerializer GetInstance(Type type) => serializers.GetOrAdd(type, serializerFactory).Value;

        public override void Serialize(Writer writer, object obj) {
            if (obj == null) {
                writer.Stream.WriteByte(Tags.TagNull);
            }
            else {
                GetInstance(obj.GetType()).Serialize(writer, obj);
            }
        }

        public override void Write(Writer writer, object obj) {
            if (obj == null) {
                writer.Stream.WriteByte(Tags.TagNull);
            }
            else {
                GetInstance(obj.GetType()).Write(writer, obj);
            }
        }
    }
}
