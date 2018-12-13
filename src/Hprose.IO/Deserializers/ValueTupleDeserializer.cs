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
 * ValueTupleDeserializer.cs                              *
 *                                                        *
 * ValueTupleDeserializer class for C#.                   *
 *                                                        *
 * LastModified: Dec 13, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.IO;
using static Hprose.IO.Tags;

namespace Hprose.IO.Deserializers {
    static class ValueTupleHelper<T> {
        public static volatile Func<Reader, int, T> read;
        static ValueTupleHelper() {
            Type type = typeof(T);
            if (type.IsGenericType) {
                var t = type.GetGenericTypeDefinition();
                if (t.Name.StartsWith("ValueTuple`")) {
                    Type[] args = type.GetGenericArguments();
                    typeof(ValueTupleHelper).GetMethod($"Initialize{args.Length}").MakeGenericMethod(args).Invoke(null, null);
                    return;
                }
            }
            read = (reader, count) => {
                T result = --count >= 0 ? Deserializer<T>.Instance.Deserialize(reader) : default;
                for (int i = count; i > 0; --i) {
                    Deserializer.Instance.Deserialize(reader);
                }
                return result;
            };
        }
    }

    static class ValueTupleHelper {
        public static void Initialize1<T1>() {
            ValueTupleHelper<ValueTuple<T1>>.read = (reader, count) => {
                var result = new ValueTuple<T1>(
                    --count >= 0 ? Deserializer<T1>.Instance.Deserialize(reader) : default
                );
                for (int i = count; i > 0; --i) {
                    Deserializer.Instance.Deserialize(reader);
                }
                return result;
            };
        }
        public static void Initialize2<T1, T2>() {
            ValueTupleHelper<ValueTuple<T1, T2>>.read = (reader, count) => {
                var result = new ValueTuple<T1, T2>(
                    --count >= 0 ? Deserializer<T1>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T2>.Instance.Deserialize(reader) : default
                );
                for (int i = count; i > 0; --i) {
                    Deserializer.Instance.Deserialize(reader);
                }
                return result;
            };
        }
        public static void Initialize3<T1, T2, T3>() {
            ValueTupleHelper<ValueTuple<T1, T2, T3>>.read = (reader, count) => {
                var result = new ValueTuple<T1, T2, T3>(
                    --count >= 0 ? Deserializer<T1>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T2>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T3>.Instance.Deserialize(reader) : default
                );
                for (int i = count; i > 0; --i) {
                    Deserializer.Instance.Deserialize(reader);
                }
                return result;
            };
        }
        public static void Initialize4<T1, T2, T3, T4>() {
            ValueTupleHelper<ValueTuple<T1, T2, T3, T4>>.read = (reader, count) => {
                var result = new ValueTuple<T1, T2, T3, T4>(
                    --count >= 0 ? Deserializer<T1>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T2>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T3>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T4>.Instance.Deserialize(reader) : default
                );
                for (int i = count; i > 0; --i) {
                    Deserializer.Instance.Deserialize(reader);
                }
                return result;
            };
        }
        public static void Initialize5<T1, T2, T3, T4, T5>() {
            ValueTupleHelper<ValueTuple<T1, T2, T3, T4, T5>>.read = (reader, count) => {
                var result = new ValueTuple<T1, T2, T3, T4, T5>(
                    --count >= 0 ? Deserializer<T1>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T2>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T3>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T4>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T5>.Instance.Deserialize(reader) : default
                );
                for (int i = count; i > 0; --i) {
                    Deserializer.Instance.Deserialize(reader);
                }
                return result;
            };
        }
        public static void Initialize6<T1, T2, T3, T4, T5, T6>() {
            ValueTupleHelper<ValueTuple<T1, T2, T3, T4, T5, T6>>.read = (reader, count) => {
                var result = new ValueTuple<T1, T2, T3, T4, T5, T6>(
                    --count >= 0 ? Deserializer<T1>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T2>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T3>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T4>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T5>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T6>.Instance.Deserialize(reader) : default
                );
                for (int i = count; i > 0; --i) {
                    Deserializer.Instance.Deserialize(reader);
                }
                return result;
            };
        }
        public static void Initialize7<T1, T2, T3, T4, T5, T6, T7>() {
            ValueTupleHelper<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>.read = (reader, count) => {
                var result = new ValueTuple<T1, T2, T3, T4, T5, T6, T7>(
                    --count >= 0 ? Deserializer<T1>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T2>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T3>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T4>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T5>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T6>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T7>.Instance.Deserialize(reader) : default
                );
                for (int i = count; i > 0; --i) {
                    Deserializer.Instance.Deserialize(reader);
                }
                return result;
            };
        }
        public static void Initialize8<T1, T2, T3, T4, T5, T6, T7, TRest>() where TRest : struct {
            ValueTupleHelper<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>>.read = (reader, count) => {
                return new ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(
                    --count >= 0 ? Deserializer<T1>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T2>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T3>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T4>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T5>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T6>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T7>.Instance.Deserialize(reader) : default,
                    ValueTupleHelper<TRest>.read(reader, count)
                );
            };
        }
    }

    class ValueTupleDeserializer : Deserializer<ValueTuple> {
        public static ValueTuple Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            reader.AddReference(default(ValueTuple));
            for (int i = count; i > 0; --i) {
                Deserializer.Instance.Deserialize(reader);
            }
            stream.ReadByte();
            return default;
        }
        public override ValueTuple Read(Reader reader, int tag) {
            switch (tag) {
                case TagEmpty:
                    return default;
                case TagList:
                    return Read(reader);
                default:
                    return base.Read(reader, tag);
            }
        }
    }

    class ValueTupleDeserializer<T> : Deserializer<T> {
        public static T Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            reader.AddReference(null);
            int index = reader.LastRefIndex;
            T tuple = ValueTupleHelper<T>.read(reader, count);
            reader.SetReference(index, tuple);
            stream.ReadByte();
            return tuple;
        }
        public override T Read(Reader reader, int tag) {
            switch (tag) {
                case TagList:
                    return Read(reader);
                case TagEmpty:
                    return default;
                default:
                    return base.Read(reader, tag);
            }
        }
    }
 }