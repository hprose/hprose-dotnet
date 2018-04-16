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
 * Deserializer.cs                                        *
 *                                                        *
 * hprose Deserializer class for C#.                      *
 *                                                        *
 * LastModified: Apr 15, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization;
using Hprose.Collections.Generic;
using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    internal interface IDeserializer {
        object Read(Reader reader, int tag);
        object Deserialize(Reader reader);
    }

    public abstract class Deserializer<T> : IDeserializer {
        static Deserializer() => Deserializer.Initialize();
        private static volatile Deserializer<T> _instance;
        public static Deserializer<T> Instance {
            get {
                if (_instance == null) {
                    _instance = Deserializer.GetInstance(typeof(T)) as Deserializer<T>;
                }
                return _instance;
            }
        }
        public virtual T Read(Reader reader, int tag) {
            switch (tag) {
                case TagNull:
                    return default;
                case TagRef:
                    return reader.ReadRef<T>();
                case TagClass:
                    reader.ReadClass();
                    return Deserialize(reader);
                case TagError:
                    throw new SerializationException(reader.Deserialize<string>());
            }
            throw new InvalidCastException("Cannot convert " + HproseTags.ToString(tag) + " to " + typeof(T).ToString() + ".");
        }
        public virtual T Deserialize(Reader reader) => Read(reader, reader.Stream.ReadByte());
        object IDeserializer.Read(Reader reader, int tag) => Read(reader, tag);
        object IDeserializer.Deserialize(Reader reader) => Deserialize(reader);
    }

    public class Deserializer : Deserializer<object> {
        static readonly ConcurrentDictionary<Type, Lazy<IDeserializer>> _deserializers = new ConcurrentDictionary<Type, Lazy<IDeserializer>>();
        static Deserializer() {
            Register(() => new Deserializer());
            Register(() => new DBNullDeserializer());
            Register(() => new BooleanDeserializer());
            Register(() => new CharDeserializer());
            Register(() => new ByteDeserializer());
            Register(() => new SByteDeserializer());
            Register(() => new Int16Deserializer());
            Register(() => new UInt16Deserializer());
            Register(() => new Int32Deserializer());
            Register(() => new UInt32Deserializer());
            Register(() => new Int64Deserializer());
            Register(() => new UInt64Deserializer());
            Register(() => new SingleDeserializer());
            Register(() => new DoubleDeserializer());
            Register(() => new DecimalDeserializer());
            Register(() => new IntPtrDeserializer());
            Register(() => new UIntPtrDeserializer());
            Register(() => new BigIntegerDeserializer());
            Register(() => new TimeSpanDeserializer());
            Register(() => new DateTimeDeserializer());
            Register(() => new GuidDeserializer());
            Register(() => new StringDeserializer());
            Register(() => new StringBuilderDeserializer());
            Register(() => new CharsDeserializer());
            Register(() => new BytesDeserializer());
            //Register(() => new ValueTupleSerializer());
            //Register(() => new BitArraySerializer());
            //Register(() => new DictionarySerializer<ExpandoObject, string, object>());
        }

        public static void Initialize() { }

        public static void Register<T>(Func<Deserializer<T>> ctor) {
            _deserializers[typeof(T)] = new Lazy<IDeserializer>(ctor);
        }

        private static Type GetDeserializerType(Type type) {
            if (type.IsArray) {
                switch (type.GetArrayRank()) {
                    case 1:
                        return typeof(ArrayDeserializer<>).MakeGenericType(type.GetElementType());
                    case 2:
                        return typeof(Array2Deserializer<>).MakeGenericType(type.GetElementType());
                    default:
                        return typeof(MultiDimArrayDeserializer<,>).MakeGenericType(type, type.GetElementType());
                }
            }
            if (type.IsGenericType) {
                Type genericType = type.GetGenericTypeDefinition();
                Type[] genericArgs = type.GetGenericArguments();
                switch (genericArgs.Length) {
                    case 1:
                        if ((typeof(ICollection<>) == genericType) ||
                            (typeof(IEnumerable<>) == genericType) ||
                            (typeof(IList<>) == genericType)) {
                            return typeof(CollectionDeserializer<,,>).MakeGenericType(type, typeof(List<>).MakeGenericType(genericArgs), genericArgs[0]);
                        }
                        if (typeof(Queue<>) == genericType) {
                            return typeof(QueueDeserializer<>).MakeGenericType(genericArgs[0]);
                        }
                        Type genericCollection = typeof(ICollection<>).MakeGenericType(genericArgs);
                        if (genericCollection.IsAssignableFrom(type)) {
                            if (genericArgs[0].IsGenericType) {
                                Type genType = genericArgs[0].GetGenericTypeDefinition();
                                //if (genType == typeof(KeyValuePair<,>)) {
                                //    Type[] genArgs = genericArgs[0].GetGenericArguments();
                                //    return typeof(DictionarySerializer<,,>).MakeGenericType(type, genArgs[0], genArgs[1]);
                                //}
                            }
                            return typeof(CollectionDeserializer<,,>).MakeGenericType(type, type, genericArgs[0]);
                        }
                        break;
                    //case 2:
                    //    if (typeof(IDictionary<,>).MakeGenericType(genericArgs).IsAssignableFrom(type)) {
                    //        return typeof(DictionarySerializer<,,>).MakeGenericType(type, genericArgs[0], genericArgs[1]);
                    //    }
                    //    if (typeof(ICollection<>).MakeGenericType(genericArgs[0]).IsAssignableFrom(type)) {
                    //        return typeof(CollectionSerializer<,>).MakeGenericType(type, genericArgs[0]);
                    //    }
                    //    if (typeof(ICollection<>).MakeGenericType(genericArgs[1]).IsAssignableFrom(type)) {
                    //        return typeof(CollectionSerializer<,>).MakeGenericType(type, genericArgs[1]);
                    //    }
                    //    break;
                }
            }
            //if (type.IsEnum) {
            //    return typeof(EnumSerializer<>).MakeGenericType(type);
            //}
            //if (type.IsGenericType) {
            //    Type genericType = type.GetGenericTypeDefinition();
            //    if (genericType.Name.StartsWith("ValueTuple`")) {
            //        return typeof(ValueTupleSerializer<>).MakeGenericType(type);
            //    }
            //    if (genericType.Name.StartsWith("Tuple`")) {
            //        return typeof(TupleSerializer<>).MakeGenericType(type);
            //    }
            //    Type[] genericArgs = type.GetGenericArguments();
            //    if (genericType == typeof(Nullable<>)) {
            //        return typeof(NullableSerializer<>).MakeGenericType(genericArgs);
            //    }
            //    if (genericType == typeof(NullableKey<>)) {
            //        return typeof(NullableKeySerializer<>).MakeGenericType(genericArgs);
            //    }
            //    switch (genericArgs.Length) {
            //        case 1:
            //            bool isGenericCollection = typeof(ICollection<>).MakeGenericType(genericArgs).IsAssignableFrom(type);
            //            bool isGenericIEnumerable = typeof(IEnumerable<>).MakeGenericType(genericArgs).IsAssignableFrom(type);
            //            if (isGenericCollection) {
            //                if (genericArgs[0].IsGenericType) {
            //                    Type genType = genericArgs[0].GetGenericTypeDefinition();
            //                    if (genType == typeof(KeyValuePair<,>)) {
            //                        Type[] genArgs = genericArgs[0].GetGenericArguments();
            //                        return typeof(DictionarySerializer<,,>).MakeGenericType(type, genArgs[0], genArgs[1]);
            //                    }
            //                }
            //                return typeof(CollectionSerializer<,>).MakeGenericType(type, genericArgs[0]);
            //            }
            //            if (isGenericIEnumerable) {
            //                bool isCollection = typeof(ICollection).IsAssignableFrom(type);
            //                if (genericArgs[0].IsGenericType) {
            //                    Type genType = genericArgs[0].GetGenericTypeDefinition();
            //                    if (genType == typeof(KeyValuePair<,>)) {
            //                        Type[] genArgs = genericArgs[0].GetGenericArguments();
            //                        if (isCollection) {
            //                            return typeof(FastEnumerableSerializer<,,>).MakeGenericType(type, genArgs[0], genArgs[1]);
            //                        }
            //                        return typeof(EnumerableSerializer<,,>).MakeGenericType(type, genArgs[0], genArgs[1]);
            //                    }
            //                }
            //                if (isCollection) {
            //                    return typeof(FastEnumerableSerializer<,>).MakeGenericType(type, genericArgs[0]);
            //                }
            //                return typeof(EnumerableSerializer<,>).MakeGenericType(type, genericArgs[0]);
            //            }
            //            break;
            //        case 2:
            //            if (typeof(IDictionary<,>).MakeGenericType(genericArgs).IsAssignableFrom(type)) {
            //                return typeof(DictionarySerializer<,,>).MakeGenericType(type, genericArgs[0], genericArgs[1]);
            //            }
            //            if (typeof(ICollection<>).MakeGenericType(genericArgs[0]).IsAssignableFrom(type)) {
            //                return typeof(CollectionSerializer<,>).MakeGenericType(type, genericArgs[0]);
            //            }
            //            if (typeof(ICollection<>).MakeGenericType(genericArgs[1]).IsAssignableFrom(type)) {
            //                return typeof(CollectionSerializer<,>).MakeGenericType(type, genericArgs[1]);
            //            }
            //            break;
            //    }
            //}
            //if (typeof(IDictionary).IsAssignableFrom(type)) {
            //    return typeof(DictionarySerializer<>).MakeGenericType(type);
            //}
            //if (typeof(IEnumerable).IsAssignableFrom(type)) {
            //    return typeof(EnumerableSerializer<>).MakeGenericType(type);
            //}
            //if (type.IsGenericType && type.Name.StartsWith("<>f__AnonymousType")) {
            //    return typeof(AnonymousTypeSerializer<>).MakeGenericType(type);
            //}
            //if (typeof(Stream).IsAssignableFrom(type)) {
            //    return typeof(StreamSerializer<>).MakeGenericType(type);
            //}
            //if (typeof(DataTable).IsAssignableFrom(type)) {
            //    return typeof(DataTableSerializer<>).MakeGenericType(type);
            //}
            //if (typeof(DataSet).IsAssignableFrom(type)) {
            //    return typeof(DataSetSerializer<>).MakeGenericType(type);
            //}
            //return typeof(ObjectSerializer<>).MakeGenericType(type);
            return null;
        }

        internal static IDeserializer GetInstance(Type type) {
            return _deserializers.GetOrAdd(type, t => new Lazy<IDeserializer>(
                () => Activator.CreateInstance(GetDeserializerType(t)) as IDeserializer
            )).Value;
        }
    }
}
