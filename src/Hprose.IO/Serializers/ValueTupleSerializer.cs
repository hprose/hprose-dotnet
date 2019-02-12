/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ValueTupleSerializer.cs                                 |
|                                                          |
|  ValueTupleSerializer class for C#.                      |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Serializers {
    using static Tags;

    internal static class ValueTupleHelper<T> {
        public static volatile int length;
        public static volatile Action<Writer, T> write;
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
            write = Serializer<T>.Instance.Serialize;
            length = 1;
        }
    }

    internal static class ValueTupleHelper {
        public static void Initialize1<T1>() {
            ValueTupleHelper<ValueTuple<T1>>.length = 1;
            ValueTupleHelper<ValueTuple<T1>>.write = (writer, obj) => {
                Serializer<T1>.Instance.Serialize(writer, obj.Item1);
            };
        }
        public static void Initialize2<T1, T2>() {
            ValueTupleHelper<ValueTuple<T1, T2>>.length = 2;
            ValueTupleHelper<ValueTuple<T1, T2>>.write = (writer, obj) => {
                Serializer<T1>.Instance.Serialize(writer, obj.Item1);
                Serializer<T2>.Instance.Serialize(writer, obj.Item2);
            };
        }
        public static void Initialize3<T1, T2, T3>() {
            ValueTupleHelper<ValueTuple<T1, T2, T3>>.length = 3;
            ValueTupleHelper<ValueTuple<T1, T2, T3>>.write = (writer, obj) => {
                Serializer<T1>.Instance.Serialize(writer, obj.Item1);
                Serializer<T2>.Instance.Serialize(writer, obj.Item2);
                Serializer<T3>.Instance.Serialize(writer, obj.Item3);
            };
        }
        public static void Initialize4<T1, T2, T3, T4>() {
            ValueTupleHelper<ValueTuple<T1, T2, T3, T4>>.length = 4;
            ValueTupleHelper<ValueTuple<T1, T2, T3, T4>>.write = (writer, obj) => {
                Serializer<T1>.Instance.Serialize(writer, obj.Item1);
                Serializer<T2>.Instance.Serialize(writer, obj.Item2);
                Serializer<T3>.Instance.Serialize(writer, obj.Item3);
                Serializer<T4>.Instance.Serialize(writer, obj.Item4);
            };
        }
        public static void Initialize5<T1, T2, T3, T4, T5>() {
            ValueTupleHelper<ValueTuple<T1, T2, T3, T4, T5>>.length = 5;
            ValueTupleHelper<ValueTuple<T1, T2, T3, T4, T5>>.write = (writer, obj) => {
                Serializer<T1>.Instance.Serialize(writer, obj.Item1);
                Serializer<T2>.Instance.Serialize(writer, obj.Item2);
                Serializer<T3>.Instance.Serialize(writer, obj.Item3);
                Serializer<T4>.Instance.Serialize(writer, obj.Item4);
                Serializer<T5>.Instance.Serialize(writer, obj.Item5);
            };
        }
        public static void Initialize6<T1, T2, T3, T4, T5, T6>() {
            ValueTupleHelper<ValueTuple<T1, T2, T3, T4, T5, T6>>.length = 6;
            ValueTupleHelper<ValueTuple<T1, T2, T3, T4, T5, T6>>.write = (writer, obj) => {
                Serializer<T1>.Instance.Serialize(writer, obj.Item1);
                Serializer<T2>.Instance.Serialize(writer, obj.Item2);
                Serializer<T3>.Instance.Serialize(writer, obj.Item3);
                Serializer<T4>.Instance.Serialize(writer, obj.Item4);
                Serializer<T5>.Instance.Serialize(writer, obj.Item5);
                Serializer<T6>.Instance.Serialize(writer, obj.Item6);
            };
        }
        public static void Initialize7<T1, T2, T3, T4, T5, T6, T7>() {
            ValueTupleHelper<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>.length = 7;
            ValueTupleHelper<ValueTuple<T1, T2, T3, T4, T5, T6, T7>>.write = (writer, obj) => {
                Serializer<T1>.Instance.Serialize(writer, obj.Item1);
                Serializer<T2>.Instance.Serialize(writer, obj.Item2);
                Serializer<T3>.Instance.Serialize(writer, obj.Item3);
                Serializer<T4>.Instance.Serialize(writer, obj.Item4);
                Serializer<T5>.Instance.Serialize(writer, obj.Item5);
                Serializer<T6>.Instance.Serialize(writer, obj.Item6);
                Serializer<T7>.Instance.Serialize(writer, obj.Item7);
            };
        }
        public static void Initialize8<T1, T2, T3, T4, T5, T6, T7, TRest>() where TRest : struct {
            ValueTupleHelper<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>>.length = 7 + ValueTupleHelper<TRest>.length;
            ValueTupleHelper<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>>.write = (writer, obj) => {
                Serializer<T1>.Instance.Serialize(writer, obj.Item1);
                Serializer<T2>.Instance.Serialize(writer, obj.Item2);
                Serializer<T3>.Instance.Serialize(writer, obj.Item3);
                Serializer<T4>.Instance.Serialize(writer, obj.Item4);
                Serializer<T5>.Instance.Serialize(writer, obj.Item5);
                Serializer<T6>.Instance.Serialize(writer, obj.Item6);
                Serializer<T7>.Instance.Serialize(writer, obj.Item7);
                ValueTupleHelper<TRest>.write(writer, obj.Rest);
            };
        }
    }

    internal class ValueTupleSerializer : ReferenceSerializer<ValueTuple> {
        public override void Write(Writer writer, ValueTuple obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagList);
            stream.WriteByte(TagOpenbrace);
            stream.WriteByte(TagClosebrace);
        }
    }

    internal class ValueTupleSerializer<T> : ReferenceSerializer<T> {
        public override void Write(Writer writer, T obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagList);
            ValueWriter.WriteInt(stream, ValueTupleHelper<T>.length);
            stream.WriteByte(TagOpenbrace);
            ValueTupleHelper<T>.write(writer, obj);
            stream.WriteByte(TagClosebrace);
        }
    }
}