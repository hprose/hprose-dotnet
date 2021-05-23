﻿/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ValueTupleDeserializer.cs                               |
|                                                          |
|  ValueTupleDeserializer class for C#.                    |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal static class ValueTupleHelper<T> {
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

    internal static class ValueTupleHelper {
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

    internal class ValueTupleDeserializer : Deserializer<ValueTuple> {
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
        public override ValueTuple Read(Reader reader, int tag) => tag switch {
            TagEmpty => default,
            TagList => Read(reader),
            _ => base.Read(reader, tag),
        };
    }

    internal class ValueTupleDeserializer<T> : Deserializer<T> {
        public static T Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            reader.AddReference(null);
            int index = reader.LastReferenceIndex;
            T tuple = ValueTupleHelper<T>.read(reader, count);
            reader.SetReference(index, tuple);
            stream.ReadByte();
            return tuple;
        }
        public override T Read(Reader reader, int tag) => tag switch {
            TagList => Read(reader),
            TagEmpty => default,
            _ => base.Read(reader, tag),
        };
    }
}