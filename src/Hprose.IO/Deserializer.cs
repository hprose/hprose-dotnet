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
 * LastModified: Jan 11, 2019                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using Hprose.Collections.Generic;
using Hprose.IO.Deserializers;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization;

namespace Hprose.IO {
    using static Tags;

    public interface IDeserializer {
        object Read(Reader reader, int tag);
        object Deserialize(Reader reader);
    }

    public abstract class Deserializer<T> : IDeserializer {
        static Deserializer() => Deserializer.Initialize();
        private static volatile Deserializer<T> instance;
        public static Deserializer<T> Instance {
            get {
                if (instance == null) {
                    instance = Deserializer.GetInstance(typeof(T)) as Deserializer<T>;
                }
                return instance;
            }
        }
        public virtual T Read(Reader reader, int tag) {
            switch (tag) {
                case TagNull:
                    return default;
                case TagRef:
                    return reader.ReadReference<T>();
                case TagClass:
                    reader.ReadClass();
                    return Deserialize(reader);
                case TagError:
                    throw new SerializationException(reader.Deserialize<string>());
            }
            throw new InvalidCastException("Cannot convert " + Tags.ToString(tag) + " to " + typeof(T).ToString() + ".");
        }
        public virtual T Deserialize(Reader reader) => Read(reader, reader.Stream.ReadByte());
        object IDeserializer.Read(Reader reader, int tag) => Read(reader, tag);
        object IDeserializer.Deserialize(Reader reader) => Deserialize(reader);
    }

    public class Deserializer : Deserializer<object> {
        private static readonly object trueObject = true;
        private static readonly object falseObject = false;
        private static readonly object[] digitObject = new object[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        static readonly ConcurrentDictionary<Type, Lazy<IDeserializer>> deserializers = new ConcurrentDictionary<Type, Lazy<IDeserializer>>();

        static Deserializer() {
            Register(() => new Deserializer());
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
            Register(() => new StringCollectionDeserializer());
            Register(() => new ValueTupleDeserializer());
            Register(() => new BitArrayDeserializer());
            Register(() => new StringObjectDictionaryDeserializer<ExpandoObject>());
            Register(() => new DataTableDeserializer());
            Register(() => new DataSetDeserializer());
            Register(() => new StreamDeserializer<Stream>());
            Register(() => new StreamDeserializer<MemoryStream>());
        }

        public static void Initialize() { }

        public static void Register<T>(Func<Deserializer<T>> ctor) => deserializers[typeof(T)] = new Lazy<IDeserializer>(ctor);

        private static Type GetDeserializerType(Type type) {
            if (type.IsEnum) {
                return typeof(EnumDeserializer<>).MakeGenericType(type);
            }
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
                        if (genericType == typeof(Nullable<>)) {
                            return typeof(NullableDeserializer<>).MakeGenericType(genericArgs);
                        }
                        if (genericType == typeof(NullableKey<>)) {
                            return typeof(NullableKeyDeserializer<>).MakeGenericType(genericArgs);
                        }
                        if (genericType == typeof(ArraySegment<>)) {
                            return typeof(ArraySegmentDeserializer<>).MakeGenericType(genericArgs);
                        }
                        if (type.IsInterface) {
                            var pairType = typeof(KeyValuePair<,>);
                            if (typeof(ISet<>) == genericType) {
                                var arg = genericArgs[0];
                                if (arg.IsGenericType && arg.GetGenericTypeDefinition() == pairType) {
                                    Type[] genArgs = arg.GetGenericArguments();
                                    if (genArgs[0] == typeof(string) && genArgs[1] == typeof(object)) {
                                        return typeof(StringObjectDictionaryDeserializer<,>).MakeGenericType(type, typeof(HashSet<>).MakeGenericType(genericArgs));
                                    }
                                    return typeof(DictionaryDeserializer<,,,>).MakeGenericType(type, typeof(HashSet<>).MakeGenericType(genericArgs), genArgs[0], genArgs[1]);
                                }
                                return typeof(CollectionDeserializer<,,>).MakeGenericType(type, typeof(HashSet<>).MakeGenericType(genericArgs), genericArgs[0]);
                            }
                            if (typeof(IList<>).MakeGenericType(genericArgs).IsAssignableFrom(type)
#if !NET40
                                || typeof(IReadOnlyList<>).MakeGenericType(genericArgs).IsAssignableFrom(type)
#endif
                                ) {
                                var arg = genericArgs[0];
                                if (arg.IsGenericType && arg.GetGenericTypeDefinition() == pairType) {
                                    Type[] genArgs = arg.GetGenericArguments();
                                    if (genArgs[0] == typeof(string) && genArgs[1] == typeof(object)) {
                                        return typeof(StringObjectDictionaryDeserializer<,>).MakeGenericType(type, typeof(List<>).MakeGenericType(genericArgs));
                                    }
                                    return typeof(DictionaryDeserializer<,,,>).MakeGenericType(type, typeof(List<>).MakeGenericType(genericArgs), genArgs[0], genArgs[1]);
                                }
                                return typeof(CollectionDeserializer<,,>).MakeGenericType(type, typeof(List<>).MakeGenericType(genericArgs), genericArgs[0]);
                            }
                            if (typeof(IEnumerable<>).MakeGenericType(genericArgs).IsAssignableFrom(type)) {
                                var arg = genericArgs[0];
                                if (arg.IsGenericType && arg.GetGenericTypeDefinition() == pairType) {
                                    Type[] genArgs = arg.GetGenericArguments();
                                    if (genArgs[0] == typeof(string) && genArgs[1] == typeof(object)) {
                                        return typeof(StringObjectDictionaryDeserializer<,>).MakeGenericType(type, typeof(Dictionary<,>).MakeGenericType(genArgs));
                                    }
                                    return typeof(DictionaryDeserializer<,,,>).MakeGenericType(type, typeof(Dictionary<,>).MakeGenericType(genArgs), genArgs[0], genArgs[1]);
                                }
                                return typeof(CollectionDeserializer<,,>).MakeGenericType(type, typeof(List<>).MakeGenericType(genericArgs), genericArgs[0]);
                            }
                        }
                        if (typeof(Queue<>) == genericType) {
                            return typeof(QueueDeserializer<>).MakeGenericType(genericArgs);
                        }
                        if (typeof(Stack<>) == genericType) {
                            return typeof(StackDeserializer<>).MakeGenericType(genericArgs);
                        }
                        if (typeof(ConcurrentStack<>) == genericType) {
                            return typeof(ConcurrentStackDeserializer<>).MakeGenericType(genericArgs);
                        }
                        if (typeof(BlockingCollection<>) == genericType) {
                            return typeof(BlockingCollectionDeserializer<>).MakeGenericType(genericArgs);
                        }
                        if (typeof(ICollection<>).MakeGenericType(genericArgs).IsAssignableFrom(type)) {
                            return typeof(CollectionDeserializer<,>).MakeGenericType(type, genericArgs[0]);
                        }
                        if (typeof(IProducerConsumerCollection<>).MakeGenericType(genericArgs).IsAssignableFrom(type)) {
                            return typeof(ProducerConsumerCollectionDeserializer<,>).MakeGenericType(type, genericArgs[0]);
                        }
                        break;
                    case 2:
                        if (typeof(IDictionary<,>) == genericType
#if !NET40
                            || typeof(IReadOnlyDictionary<,>) == genericType
#endif
                            ) {
                            if (genericArgs[0] == typeof(string) && genericArgs[1] == typeof(object)) {
                                return typeof(StringObjectDictionaryDeserializer<,>).MakeGenericType(type, typeof(Dictionary<,>).MakeGenericType(genericArgs));
                            }
                            return typeof(DictionaryDeserializer<,,,>).MakeGenericType(type, typeof(Dictionary<,>).MakeGenericType(genericArgs), genericArgs[0], genericArgs[1]);
                        }
                        if (typeof(IDictionary<,>).MakeGenericType(genericArgs).IsAssignableFrom(type)) {
                            if (genericArgs[0] == typeof(string) && genericArgs[1] == typeof(object)) {
                                return typeof(StringObjectDictionaryDeserializer<>).MakeGenericType(type);
                            }
                            return typeof(DictionaryDeserializer<,,>).MakeGenericType(type, genericArgs[0], genericArgs[1]);
                        }
                        break;
                }
            }
            if (type.IsGenericType) {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType.Name.StartsWith("ValueTuple`")) {
                    return typeof(ValueTupleDeserializer<>).MakeGenericType(type);
                }
                if (genericType.Name.StartsWith("Tuple`")) {
                    return typeof(TupleDeserializer<>).MakeGenericType(type);
                }
            }
            if (typeof(IDictionary).IsAssignableFrom(type)) {
                if (type.IsInterface) {
                    return typeof(DictionaryDeserializer<,>).MakeGenericType(type, typeof(Hashtable));
                }
                return typeof(DictionaryDeserializer<>).MakeGenericType(type);
            }
            if (typeof(IEnumerable).IsAssignableFrom(type)) {
                if (type.IsInterface) {
                    return typeof(ListDeserializer<,>).MakeGenericType(type, typeof(ArrayList));
                }
                if (typeof(IList).IsAssignableFrom(type)) {
                    return typeof(ListDeserializer<>).MakeGenericType(type);
                }
            }
            if (type.IsValueType) {
                return typeof(StructDeserializer<>).MakeGenericType(type);
            }
            return typeof(ObjectDeserializer<>).MakeGenericType(type);
        }

        private static readonly Func<Type, Lazy<IDeserializer>> deserializerFactory = type => new Lazy<IDeserializer>(
                () => Activator.CreateInstance(GetDeserializerType(type)) as IDeserializer
            );

        public static IDeserializer GetInstance(Type type) => type == null ? Instance : deserializers.GetOrAdd(type, deserializerFactory).Value;

        public override object Read(Reader reader, int tag) {
            if (tag >= '0' && tag <= '9') {
                return digitObject[tag - '0'];
            }
            var stream = reader.Stream;
            switch (tag) {
                case TagInteger:
                    return ValueReader.ReadInt(stream);
                case TagString:
                    return ReferenceReader.ReadString(reader);
                case TagBytes:
                    return ReferenceReader.ReadBytes(reader);
                case TagTrue:
                    return trueObject;
                case TagFalse:
                    return falseObject;
                case TagEmpty:
                    return "";
                case TagObject:
                    return Read(reader);
                case TagRef:
                    return reader.ReadReference();
                case TagDate:
                    return ReferenceReader.ReadDateTime(reader);
                case TagTime:
                    return ReferenceReader.ReadTime(reader);
                case TagGuid:
                    return ReferenceReader.ReadGuid(reader);
                case TagLong:
                    switch (reader.DefaultLongType) {
                        case LongType.Int64:
                            return ValueReader.ReadLong(stream);
                        case LongType.UInt64:
                            return (ulong)ValueReader.ReadLong(stream);
                        default:
                            return ValueReader.ReadBigInteger(stream);
                    }
                case TagDouble:
                    switch (reader.DefaultRealType) {
                        case RealType.Single:
                            return ValueReader.ReadSingle(stream);
                        case RealType.Decimal:
                            return ValueReader.ReadDecimal(stream);
                        default:
                            return ValueReader.ReadDouble(stream);
                    }
                case TagNaN:
                    switch (reader.DefaultRealType) {
                        case RealType.Single:
                            return float.NaN;
                        default:
                            return double.NaN;
                    }
                case TagInfinity:
                    switch (reader.DefaultRealType) {
                        case RealType.Single:
                            return ValueReader.ReadSingleInfinity(stream);
                        default:
                            return ValueReader.ReadInfinity(stream);
                    }
                case TagUTF8Char:
                    switch (reader.DefaultCharType) {
                        case CharType.Char:
                            return ValueReader.ReadChar(stream);
                        default:
                            return ValueReader.ReadUTF8Char(stream);
                    }
                case TagList:
                    switch (reader.DefaultListType) {
                        case ListType.Array:
                            return ReferenceReader.ReadArray<object>(reader);
                        case ListType.ArrayList:
                            return ListDeserializer<ArrayList>.Read(reader);
                        default:
                            return CollectionDeserializer<List<object>, object>.Read(reader);
                    }
                case TagMap:
                    switch (reader.DefaultDictType) {
                        case DictType.Dictionary:
                            return DictionaryDeserializer<Dictionary<object, object>, object, object>.Read(reader);
                        case DictType.ExpandoObject:
                            return StringObjectDictionaryDeserializer<ExpandoObject>.Read(reader);
                        case DictType.Hashtable:
                            return DictionaryDeserializer<Hashtable>.Read(reader);
                        default:
                            return DictionaryDeserializer<NullableKeyDictionary<object, object>, object, object>.Read(reader);
                    }
                default:
                    return base.Read(reader, tag);
            }
        }

        private object Read(Reader reader) {
            Stream stream = reader.Stream;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            var typeInfo = reader.GetTypeInfo(index);
            var type = typeInfo.type;
            if (type != null) {
                return ((IObjectDeserializer)GetInstance(type)).ReadObject(reader, typeInfo.key);
            }
            var obj = new ExpandoObject();
            reader.AddReference(obj);
            var dict = (IDictionary<string, object>)obj;
            string[] names = typeInfo.names;
            int count = names.Length;
            for (int i = 0; i < count; ++i) {
                dict.Add(names[i], Deserialize(reader));
            }
            stream.ReadByte();
            return obj;
        }
    }
}
