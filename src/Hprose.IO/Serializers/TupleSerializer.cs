/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  TupleSerializer.cs                                      |
|                                                          |
|  TupleSerializer class for C#.                           |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Serializers {
    using static Tags;

    internal static class TupleHelper<T> {
        public static volatile int length;
        public static volatile Action<Writer, T> write;
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
            write = Serializer<T>.Instance.Serialize;
            length = 1;
        }
    }

    internal static class TupleHelper {
        public static void Initialize1<T1>() {
            TupleHelper<Tuple<T1>>.length = 1;
            TupleHelper<Tuple<T1>>.write = (writer, obj) => {
                Serializer<T1>.Instance.Serialize(writer, obj.Item1);
            };
        }
        public static void Initialize2<T1, T2>() {
            TupleHelper<Tuple<T1, T2>>.length = 2;
            TupleHelper<Tuple<T1, T2>>.write = (writer, obj) => {
                Serializer<T1>.Instance.Serialize(writer, obj.Item1);
                Serializer<T2>.Instance.Serialize(writer, obj.Item2);
            };
        }
        public static void Initialize3<T1, T2, T3>() {
            TupleHelper<Tuple<T1, T2, T3>>.length = 3;
            TupleHelper<Tuple<T1, T2, T3>>.write = (writer, obj) => {
                Serializer<T1>.Instance.Serialize(writer, obj.Item1);
                Serializer<T2>.Instance.Serialize(writer, obj.Item2);
                Serializer<T3>.Instance.Serialize(writer, obj.Item3);
            };
        }
        public static void Initialize4<T1, T2, T3, T4>() {
            TupleHelper<Tuple<T1, T2, T3, T4>>.length = 4;
            TupleHelper<Tuple<T1, T2, T3, T4>>.write = (writer, obj) => {
                Serializer<T1>.Instance.Serialize(writer, obj.Item1);
                Serializer<T2>.Instance.Serialize(writer, obj.Item2);
                Serializer<T3>.Instance.Serialize(writer, obj.Item3);
                Serializer<T4>.Instance.Serialize(writer, obj.Item4);
            };
        }
        public static void Initialize5<T1, T2, T3, T4, T5>() {
            TupleHelper<Tuple<T1, T2, T3, T4, T5>>.length = 5;
            TupleHelper<Tuple<T1, T2, T3, T4, T5>>.write = (writer, obj) => {
                Serializer<T1>.Instance.Serialize(writer, obj.Item1);
                Serializer<T2>.Instance.Serialize(writer, obj.Item2);
                Serializer<T3>.Instance.Serialize(writer, obj.Item3);
                Serializer<T4>.Instance.Serialize(writer, obj.Item4);
                Serializer<T5>.Instance.Serialize(writer, obj.Item5);
            };
        }
        public static void Initialize6<T1, T2, T3, T4, T5, T6>() {
            TupleHelper<Tuple<T1, T2, T3, T4, T5, T6>>.length = 6;
            TupleHelper<Tuple<T1, T2, T3, T4, T5, T6>>.write = (writer, obj) => {
                Serializer<T1>.Instance.Serialize(writer, obj.Item1);
                Serializer<T2>.Instance.Serialize(writer, obj.Item2);
                Serializer<T3>.Instance.Serialize(writer, obj.Item3);
                Serializer<T4>.Instance.Serialize(writer, obj.Item4);
                Serializer<T5>.Instance.Serialize(writer, obj.Item5);
                Serializer<T6>.Instance.Serialize(writer, obj.Item6);
            };
        }
        public static void Initialize7<T1, T2, T3, T4, T5, T6, T7>() {
            TupleHelper<Tuple<T1, T2, T3, T4, T5, T6, T7>>.length = 7;
            TupleHelper<Tuple<T1, T2, T3, T4, T5, T6, T7>>.write = (writer, obj) => {
                Serializer<T1>.Instance.Serialize(writer, obj.Item1);
                Serializer<T2>.Instance.Serialize(writer, obj.Item2);
                Serializer<T3>.Instance.Serialize(writer, obj.Item3);
                Serializer<T4>.Instance.Serialize(writer, obj.Item4);
                Serializer<T5>.Instance.Serialize(writer, obj.Item5);
                Serializer<T6>.Instance.Serialize(writer, obj.Item6);
                Serializer<T7>.Instance.Serialize(writer, obj.Item7);
            };
        }
        public static void Initialize8<T1, T2, T3, T4, T5, T6, T7, TRest>() {
            TupleHelper<Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>>.length = 7 + TupleHelper<TRest>.length;
            TupleHelper<Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>>.write = (writer, obj) => {
                Serializer<T1>.Instance.Serialize(writer, obj.Item1);
                Serializer<T2>.Instance.Serialize(writer, obj.Item2);
                Serializer<T3>.Instance.Serialize(writer, obj.Item3);
                Serializer<T4>.Instance.Serialize(writer, obj.Item4);
                Serializer<T5>.Instance.Serialize(writer, obj.Item5);
                Serializer<T6>.Instance.Serialize(writer, obj.Item6);
                Serializer<T7>.Instance.Serialize(writer, obj.Item7);
                TupleHelper<TRest>.write(writer, obj.Rest);
            };
        }
    }

    internal class TupleSerializer<T> : ReferenceSerializer<T> {
        public override void Write(Writer writer, T obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagList);
            ValueWriter.WriteInt(stream, TupleHelper<T>.length);
            stream.WriteByte(TagOpenbrace);
            TupleHelper<T>.write(writer, obj);
            stream.WriteByte(TagClosebrace);
        }
    }
}