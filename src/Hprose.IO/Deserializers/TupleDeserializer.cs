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
 * TupleDeserializer.cs                                   *
 *                                                        *
 * TupleDeserializer class for C#.                        *
 *                                                        *
 * LastModified: Apr 25, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.IO;
using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    static class TupleHelper<T> {
        public static volatile Func<Reader, int, T> read;
        static TupleHelper() {
            Type type = typeof(T);
            if (type.IsGenericType) {
                var t = type.GetGenericTypeDefinition();
                if (t.Name.StartsWith("Tuple`")) {
                    Type[] args = type.GetGenericArguments();
                    typeof(TupleHelper).GetMethod($"Initialize{args.Length}").MakeGenericMethod(args).Invoke(null, null);
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

    static class TupleHelper {
        public static void Initialize1<T1>() {
            TupleHelper<Tuple<T1>>.read = (reader, count) => {
                var result = new Tuple<T1>(
                    --count >= 0 ? Deserializer<T1>.Instance.Deserialize(reader) : default
                );
                for (int i = count; i > 0; --i) {
                    Deserializer.Instance.Deserialize(reader);
                }
                return result;
            };
        }
        public static void Initialize2<T1, T2>() {
            TupleHelper<Tuple<T1, T2>>.read = (reader, count) => {
                var result = new Tuple<T1, T2>(
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
            TupleHelper<Tuple<T1, T2, T3>>.read = (reader, count) => {
                var result = new Tuple<T1, T2, T3>(
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
            TupleHelper<Tuple<T1, T2, T3, T4>>.read = (reader, count) => {
                var result = new Tuple<T1, T2, T3, T4>(
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
            TupleHelper<Tuple<T1, T2, T3, T4, T5>>.read = (reader, count) => {
                var result = new Tuple<T1, T2, T3, T4, T5>(
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
            TupleHelper<Tuple<T1, T2, T3, T4, T5, T6>>.read = (reader, count) => {
                var result = new Tuple<T1, T2, T3, T4, T5, T6>(
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
            TupleHelper<Tuple<T1, T2, T3, T4, T5, T6, T7>>.read = (reader, count) => {
                var result = new Tuple<T1, T2, T3, T4, T5, T6, T7>(
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
        public static void Initialize8<T1, T2, T3, T4, T5, T6, T7, TRest>() {
            TupleHelper<Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>>.read = (reader, count) => {
                return new Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>(
                    --count >= 0 ? Deserializer<T1>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T2>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T3>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T4>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T5>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T6>.Instance.Deserialize(reader) : default,
                    --count >= 0 ? Deserializer<T7>.Instance.Deserialize(reader) : default,
                    TupleHelper<TRest>.read(reader, count)
                );
            };
        }
    }

    class TupleDeserializer<T> : Deserializer<T> {
        public static T Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            reader.SetRef(null);
            int index = reader.LastRefIndex;
            T tuple = TupleHelper<T>.read(reader, count);
            reader.SetRef(index, tuple);
            stream.ReadByte();
            return tuple;
        }
        public override T Read(Reader reader, int tag) {
            switch (tag) {
                case TagList:
                    return Read(reader);
                case TagEmpty:
                    return TupleHelper<T>.read(reader, 0);
                default:
                    return base.Read(reader, tag);
            }
        }
    }
 }