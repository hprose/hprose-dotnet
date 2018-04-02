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
 * TupleSerializer.cs                                     *
 *                                                        *
 * TupleSerializer class for C#.                          *
 *                                                        *
 * LastModified: Apr 2, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Serializers {
    class TupleHelper<T> {
        public static volatile int Length;
        public static volatile Action<Writer, T> WriteElements;
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
            WriteElements = Serializer<T>.Instance.Write;
            Length = 1;
        }
    }

    class TupleHelper {
        public static void Initialize1<T1>() {
            TupleHelper<Tuple<T1>>.Length = 1;
            TupleHelper<Tuple<T1>>.WriteElements = (writer, obj) => {
                Serializer<T1>.Instance.Write(writer, obj.Item1);
            };
        }
        public static void Initialize2<T1, T2>() {
            TupleHelper<Tuple<T1, T2>>.Length = 2;
            TupleHelper<Tuple<T1, T2>>.WriteElements = (writer, obj) => {
                Serializer<T1>.Instance.Write(writer, obj.Item1);
                Serializer<T2>.Instance.Write(writer, obj.Item2);
            };
        }
        public static void Initialize3<T1, T2, T3>() {
            TupleHelper<Tuple<T1, T2, T3>>.Length = 3;
            TupleHelper<Tuple<T1, T2, T3>>.WriteElements = (writer, obj) => {
                Serializer<T1>.Instance.Write(writer, obj.Item1);
                Serializer<T2>.Instance.Write(writer, obj.Item2);
                Serializer<T3>.Instance.Write(writer, obj.Item3);
            };
        }
        public static void Initialize4<T1, T2, T3, T4>() {
            TupleHelper<Tuple<T1, T2, T3, T4>>.Length = 4;
            TupleHelper<Tuple<T1, T2, T3, T4>>.WriteElements = (writer, obj) => {
                Serializer<T1>.Instance.Write(writer, obj.Item1);
                Serializer<T2>.Instance.Write(writer, obj.Item2);
                Serializer<T3>.Instance.Write(writer, obj.Item3);
                Serializer<T4>.Instance.Write(writer, obj.Item4);
            };
        }
        public static void Initialize5<T1, T2, T3, T4, T5>() {
            TupleHelper<Tuple<T1, T2, T3, T4, T5>>.Length = 5;
            TupleHelper<Tuple<T1, T2, T3, T4, T5>>.WriteElements = (writer, obj) => {
                Serializer<T1>.Instance.Write(writer, obj.Item1);
                Serializer<T2>.Instance.Write(writer, obj.Item2);
                Serializer<T3>.Instance.Write(writer, obj.Item3);
                Serializer<T4>.Instance.Write(writer, obj.Item4);
                Serializer<T5>.Instance.Write(writer, obj.Item5);
            };
        }
        public static void Initialize6<T1, T2, T3, T4, T5, T6>() {
            TupleHelper<Tuple<T1, T2, T3, T4, T5, T6>>.Length = 6;
            TupleHelper<Tuple<T1, T2, T3, T4, T5, T6>>.WriteElements = (writer, obj) => {
                Serializer<T1>.Instance.Write(writer, obj.Item1);
                Serializer<T2>.Instance.Write(writer, obj.Item2);
                Serializer<T3>.Instance.Write(writer, obj.Item3);
                Serializer<T4>.Instance.Write(writer, obj.Item4);
                Serializer<T5>.Instance.Write(writer, obj.Item5);
                Serializer<T6>.Instance.Write(writer, obj.Item6);
            };
        }
        public static void Initialize7<T1, T2, T3, T4, T5, T6, T7>() {
            TupleHelper<Tuple<T1, T2, T3, T4, T5, T6, T7>>.Length = 7;
            TupleHelper<Tuple<T1, T2, T3, T4, T5, T6, T7>>.WriteElements = (writer, obj) => {
                Serializer<T1>.Instance.Write(writer, obj.Item1);
                Serializer<T2>.Instance.Write(writer, obj.Item2);
                Serializer<T3>.Instance.Write(writer, obj.Item3);
                Serializer<T4>.Instance.Write(writer, obj.Item4);
                Serializer<T5>.Instance.Write(writer, obj.Item5);
                Serializer<T6>.Instance.Write(writer, obj.Item6);
                Serializer<T7>.Instance.Write(writer, obj.Item7);
            };
        }
        public static void Initialize8<T1, T2, T3, T4, T5, T6, T7, TRest>() {
            TupleHelper<Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>>.Length = 7 + TupleHelper<TRest>.Length;
            TupleHelper<Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>>.WriteElements = (writer, obj) => {
                Serializer<T1>.Instance.Write(writer, obj.Item1);
                Serializer<T2>.Instance.Write(writer, obj.Item2);
                Serializer<T3>.Instance.Write(writer, obj.Item3);
                Serializer<T4>.Instance.Write(writer, obj.Item4);
                Serializer<T5>.Instance.Write(writer, obj.Item5);
                Serializer<T6>.Instance.Write(writer, obj.Item6);
                Serializer<T7>.Instance.Write(writer, obj.Item7);
                TupleHelper<TRest>.WriteElements(writer, obj.Rest);
            };
        }
    }

    class TupleSerializer<T> : ReferenceSerializer<T> {
        public override void Serialize(Writer writer, T obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagList);
            ValueWriter.WriteInt(stream, TupleHelper<T>.Length);
            stream.WriteByte(TagOpenbrace);
            TupleHelper<T>.WriteElements(writer, obj);
            stream.WriteByte(TagClosebrace);
        }
    }
}