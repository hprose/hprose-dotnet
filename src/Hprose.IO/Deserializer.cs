/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Deserializer.cs                                         |
|                                                          |
|  hprose Deserializer class for C#.                       |
|                                                          |
|  LastModified: Jul 2, 2020                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.Collections.Generic;
using Hprose.IO.Deserializers;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
#if !NET35
using System.Dynamic;
#endif
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

        static readonly ConcurrentDictionary<Type, Lazy<IDeserializer>> deserializers = new();

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
            Register(() => new TimeSpanDeserializer());
            Register(() => new DateTimeDeserializer());
            Register(() => new GuidDeserializer());
            Register(() => new StringDeserializer());
            Register(() => new StringBuilderDeserializer());
            Register(() => new CharsDeserializer());
            Register(() => new BytesDeserializer());
            Register(() => new ValueTupleDeserializer());
            Register(() => new BitArrayDeserializer());
            Register(() => new BigIntegerDeserializer());
            Register(() => new DataTableDeserializer());
            Register(() => new DataSetDeserializer());
            Register(() => new StreamDeserializer<Stream>());
            Register(() => new StreamDeserializer<MemoryStream>());
            Register(() => new ExceptionDeserializer<Exception>());
#if !NET35
            Register(() => new ExpandoObjectDeserializer());
#endif
#if !NET35_CF
            Register(() => new StringCollectionDeserializer());
#endif
        }

        public static void Initialize() { }

#if !NET35
        public static void Register<T>(Func<Deserializer<T>> ctor) => deserializers[typeof(T)] = new Lazy<IDeserializer>(ctor);
#else
        public static void Register<T>(Func<Deserializer<T>> ctor) => deserializers[typeof(T)] = new Lazy<IDeserializer>(() => ctor() as IDeserializer);
#endif

        private static Type GetDeserializerType(Type type) {
            if (type.IsEnum) {
                return typeof(EnumDeserializer<>).MakeGenericType(type);
            }
            if (type.IsArray) {
                return (type.GetArrayRank()) switch {
                    1 => typeof(ArrayDeserializer<>).MakeGenericType(type.GetElementType()),
                    2 => typeof(Array2Deserializer<>).MakeGenericType(type.GetElementType()),
                    _ => typeof(MultiDimArrayDeserializer<,>).MakeGenericType(type, type.GetElementType()),
                };
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
#if !NET35
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
#endif
                            if (typeof(IList<>).MakeGenericType(genericArgs).IsAssignableFrom(type)
#if !NET40 && !NET35
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
#if !NET40 && !NET35
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
            if (typeof(Exception).IsAssignableFrom(type)) {
                return typeof(ExceptionDeserializer<>).MakeGenericType(type);
            }
            if (type.IsValueType) {
                return typeof(StructDeserializer<>).MakeGenericType(type);
            }
            if (type.IsInterface) {
                return typeof(InterfaceDeserializer<>).MakeGenericType(type);
            }
            return typeof(ObjectDeserializer<>).MakeGenericType(type);
        }

#if !NET35_CF
        private static readonly Func<Type, Lazy<IDeserializer>> deserializerFactory = type => new Lazy<IDeserializer>(
            () => Activator.CreateInstance(GetDeserializerType(type)) as IDeserializer
        );
#else
        private static readonly Func2<Type, Lazy<IDeserializer>> deserializerFactory = type => new Lazy<IDeserializer>(
            () => Activator.CreateInstance(GetDeserializerType(type)) as IDeserializer
        );
#endif

        public static IDeserializer GetInstance(Type type) => type == null ? Instance : deserializers.GetOrAdd(type, deserializerFactory).Value;

        public override object Read(Reader reader, int tag) => tag switch {
            '0' => digitObject[0],
            '1' => digitObject[1],
            '2' => digitObject[2],
            '3' => digitObject[3],
            '4' => digitObject[4],
            '5' => digitObject[5],
            '6' => digitObject[6],
            '7' => digitObject[7],
            '8' => digitObject[8],
            '9' => digitObject[9],
            TagInteger => ValueReader.ReadInt(reader.Stream),
            TagString => ReferenceReader.ReadString(reader),
            TagBytes => ReferenceReader.ReadBytes(reader),
            TagTrue => trueObject,
            TagFalse => falseObject,
            TagEmpty => "",
            TagObject => Read(reader),
            TagRef => reader.ReadReference(),
            TagDate => ReferenceReader.ReadDateTime(reader),
            TagTime => ReferenceReader.ReadTime(reader),
            TagGuid => ReferenceReader.ReadGuid(reader),
            TagLong => reader.LongType switch {
                LongType.Int64 => ValueReader.ReadLong(reader.Stream),
                LongType.UInt64 => (ulong)ValueReader.ReadLong(reader.Stream),
                _ => ValueReader.ReadBigInteger(reader.Stream),
            },
            TagDouble => reader.RealType switch {
                RealType.Single => ValueReader.ReadSingle(reader.Stream),
                RealType.Decimal => ValueReader.ReadDecimal(reader.Stream),
                _ => ValueReader.ReadDouble(reader.Stream),
            },
            TagNaN => reader.RealType switch {
                RealType.Single => float.NaN,
                _ => double.NaN,
            },
            TagInfinity => reader.RealType switch {
                RealType.Single => ValueReader.ReadSingleInfinity(reader.Stream),
                _ => ValueReader.ReadInfinity(reader.Stream),
            },
            TagUTF8Char => reader.CharType switch {
                CharType.Char => ValueReader.ReadChar(reader.Stream),
                _ => ValueReader.ReadUTF8Char(reader.Stream),
            },
            TagList => reader.ListType switch {
                ListType.Array => ReferenceReader.ReadArray<object>(reader),
                ListType.ArrayList => ListDeserializer<ArrayList>.Read(reader),
                _ => CollectionDeserializer<List<object>, object>.Read(reader),
            },
            TagMap => reader.DictType switch {
                DictType.Dictionary => DictionaryDeserializer<Dictionary<object, object>, object, object>.Read(reader),
#if !NET35
                DictType.ExpandoObject => ExpandoObjectDeserializer.Read(reader),
#endif
                DictType.Hashtable => DictionaryDeserializer<Hashtable>.Read(reader),
                _ => DictionaryDeserializer<NullableKeyDictionary<object, object>, object, object>.Read(reader),
            },
            TagError => new Exception(reader.Deserialize<string>()),
            _ => base.Read(reader, tag),
        };

        private object Read(Reader reader) {
            Stream stream = reader.Stream;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            var typeInfo = reader.GetTypeInfo(index);
            var type = typeInfo.type;
            if (type != null) {
                return ((IObjectDeserializer)GetInstance(type)).ReadObject(reader, typeInfo.key);
            }
#if !NET35
            var obj = new ExpandoObject();
#else
            var obj = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
#endif
            reader.AddReference(obj);
            var dict = (IDictionary<string, object>)obj;
            string[] names = typeInfo.names;
            int count = names.Length;
            for (int i = 0; i < count; ++i) {
                dict.Add(Accessor.TitleCaseName(names[i]), Deserialize(reader));
            }
            stream.ReadByte();
            return obj;
        }
    }
}
