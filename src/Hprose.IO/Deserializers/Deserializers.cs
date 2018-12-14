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
 * Deserializers.cs                                       *
 *                                                        *
 * hprose Deserializers class for C#.                     *
 *                                                        *
 * LastModified: Dec 13, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

using Hprose.Collections.Generic;

namespace Hprose.IO.Deserializers {
    public static class Deserializers {
        static readonly ConcurrentDictionary<Type, Lazy<IDeserializer>> deserializers = new ConcurrentDictionary<Type, Lazy<IDeserializer>>();

        static Deserializers() {
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

        internal static IDeserializer GetInstance(Type type) => type == null ? Deserializer.Instance : deserializers.GetOrAdd(type, deserializerFactory).Value;
    }
}
