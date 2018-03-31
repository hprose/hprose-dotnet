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
 * ValueTupleSerializer.cs                                *
 *                                                        *
 * ValueTupleSerializer class for C#.                     *
 *                                                        *
 * LastModified: Mar 31, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using static Hprose.IO.HproseTags;

namespace Hprose.IO.Serializers {
    class ValueTupleSerializer : ReferenceSerializer<ValueTuple> {
        public override void Serialize(Writer writer, ValueTuple obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagList);
            stream.WriteByte(TagOpenbrace);
            stream.WriteByte(TagClosebrace);
        }
    }
    class ValueTupleSerializer<T1> : ReferenceSerializer<ValueTuple<T1>> {
        public override void Serialize(Writer writer, ValueTuple<T1> obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagList);
            stream.WriteByte((byte)'1');
            stream.WriteByte(TagOpenbrace);
            Serializer<T1>.Instance.Write(writer, obj.Item1);
            stream.WriteByte(TagClosebrace);
        }
    }
    class ValueTupleSerializer<T1, T2> : ReferenceSerializer<ValueTuple<T1, T2>> {
        public override void Serialize(Writer writer, ValueTuple<T1, T2> obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagList);
            stream.WriteByte((byte)'2');
            stream.WriteByte(TagOpenbrace);
            Serializer<T1>.Instance.Write(writer, obj.Item1);
            Serializer<T2>.Instance.Write(writer, obj.Item2);
            stream.WriteByte(TagClosebrace);
        }
    }
    class ValueTupleSerializer<T1, T2, T3> : ReferenceSerializer<ValueTuple<T1, T2, T3>> {
        public override void Serialize(Writer writer, ValueTuple<T1, T2, T3> obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagList);
            stream.WriteByte((byte)'3');
            stream.WriteByte(TagOpenbrace);
            Serializer<T1>.Instance.Write(writer, obj.Item1);
            Serializer<T2>.Instance.Write(writer, obj.Item2);
            Serializer<T3>.Instance.Write(writer, obj.Item3);
            stream.WriteByte(TagClosebrace);
        }
    }
    class ValueTupleSerializer<T1, T2, T3, T4> : ReferenceSerializer<ValueTuple<T1, T2, T3, T4>> {
        public override void Serialize(Writer writer, ValueTuple<T1, T2, T3, T4> obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagList);
            stream.WriteByte((byte)'4');
            stream.WriteByte(TagOpenbrace);
            Serializer<T1>.Instance.Write(writer, obj.Item1);
            Serializer<T2>.Instance.Write(writer, obj.Item2);
            Serializer<T3>.Instance.Write(writer, obj.Item3);
            Serializer<T4>.Instance.Write(writer, obj.Item4);
            stream.WriteByte(TagClosebrace);
        }
    }
    class ValueTupleSerializer<T1, T2, T3, T4, T5> : ReferenceSerializer<ValueTuple<T1, T2, T3, T4, T5>> {
        public override void Serialize(Writer writer, ValueTuple<T1, T2, T3, T4, T5> obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagList);
            stream.WriteByte((byte)'5');
            stream.WriteByte(TagOpenbrace);
            Serializer<T1>.Instance.Write(writer, obj.Item1);
            Serializer<T2>.Instance.Write(writer, obj.Item2);
            Serializer<T3>.Instance.Write(writer, obj.Item3);
            Serializer<T4>.Instance.Write(writer, obj.Item4);
            Serializer<T5>.Instance.Write(writer, obj.Item5);
            stream.WriteByte(TagClosebrace);
        }
    }
    class ValueTupleSerializer<T1, T2, T3, T4, T5, T6> : ReferenceSerializer<ValueTuple<T1, T2, T3, T4, T5, T6>> {
        public override void Serialize(Writer writer, ValueTuple<T1, T2, T3, T4, T5, T6> obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagList);
            stream.WriteByte((byte)'6');
            stream.WriteByte(TagOpenbrace);
            Serializer<T1>.Instance.Write(writer, obj.Item1);
            Serializer<T2>.Instance.Write(writer, obj.Item2);
            Serializer<T3>.Instance.Write(writer, obj.Item3);
            Serializer<T4>.Instance.Write(writer, obj.Item4);
            Serializer<T5>.Instance.Write(writer, obj.Item5);
            Serializer<T6>.Instance.Write(writer, obj.Item6);
            stream.WriteByte(TagClosebrace);
        }
    }
    class ValueTupleSerializer<T1, T2, T3, T4, T5, T6, T7> : ReferenceSerializer<ValueTuple<T1, T2, T3, T4, T5, T6, T7>> {
        public override void Serialize(Writer writer, ValueTuple<T1, T2, T3, T4, T5, T6, T7> obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagList);
            stream.WriteByte((byte)'7');
            stream.WriteByte(TagOpenbrace);
            Serializer<T1>.Instance.Write(writer, obj.Item1);
            Serializer<T2>.Instance.Write(writer, obj.Item2);
            Serializer<T3>.Instance.Write(writer, obj.Item3);
            Serializer<T4>.Instance.Write(writer, obj.Item4);
            Serializer<T5>.Instance.Write(writer, obj.Item5);
            Serializer<T6>.Instance.Write(writer, obj.Item6);
            Serializer<T7>.Instance.Write(writer, obj.Item7);
            stream.WriteByte(TagClosebrace);
        }
    }
    class ValueTupleSerializer<T1, T2, T3, T4, T5, T6, T7, TRest> : ReferenceSerializer<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>> where TRest : struct {
        private static int length = 7 + GetLength(typeof(TRest));
        public override void Serialize(Writer writer, ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagList);
            ValueWriter.WriteInt(stream, length);
            stream.WriteByte(TagOpenbrace);
            Serializer<T1>.Instance.Write(writer, obj.Item1);
            Serializer<T2>.Instance.Write(writer, obj.Item2);
            Serializer<T3>.Instance.Write(writer, obj.Item3);
            Serializer<T4>.Instance.Write(writer, obj.Item4);
            Serializer<T5>.Instance.Write(writer, obj.Item5);
            Serializer<T6>.Instance.Write(writer, obj.Item6);
            Serializer<T7>.Instance.Write(writer, obj.Item7);
            WriteRest(writer, obj.Rest);
            stream.WriteByte(TagClosebrace);
        }
        private static int GetLength(Type type) {
            if (type == typeof(ValueTuple)) {
                return 0;
            }
            if (type.IsGenericType) {
                var t = type.GetGenericTypeDefinition();
                if (t == typeof(ValueTuple<>)) {
                    return 1;
                }
                if (t == typeof(ValueTuple<,>)) {
                    return 2;
                }
                if (t == typeof(ValueTuple<,,>)) {
                    return 3;
                }
                if (t == typeof(ValueTuple<,,,>)) {
                    return 4;
                }
                if (t == typeof(ValueTuple<,,,,>)) {
                    return 5;
                }
                if (t == typeof(ValueTuple<,,,,,>)) {
                    return 6;
                }
                if (t == typeof(ValueTuple<,,,,,,>)) {
                    return 7;
                }
                if (t == typeof(ValueTuple<,,,,,,,>)) {
                    return 7 + GetLength(type.GetGenericArguments()[7]);
                }
            }
            return 1;
        }
        private static void WriteRest(Writer writer, object obj) {
            var type = obj.GetType();
            if (type == typeof(ValueTuple)) {
                return;
            }
            if (type.IsGenericType) {
                var t = type.GetGenericTypeDefinition();
                if (t == typeof(ValueTuple<>)) {
                    WriteElements(writer, obj, type, 1);
                    return;
                }
                if (t == typeof(ValueTuple<,>)) {
                    WriteElements(writer, obj, type, 2);
                    return;
                }
                if (t == typeof(ValueTuple<,,>)) {
                    WriteElements(writer, obj, type, 3);
                    return;
                }
                if (t == typeof(ValueTuple<,,,>)) {
                    WriteElements(writer, obj, type, 4);
                    return;
                }
                if (t == typeof(ValueTuple<,,,,>)) {
                    WriteElements(writer, obj, type, 5);
                    return;
                }
                if (t == typeof(ValueTuple<,,,,,>)) {
                    WriteElements(writer, obj, type, 6);
                    return;
                }
                if (t == typeof(ValueTuple<,,,,,,>)) {
                    WriteElements(writer, obj, type, 7);
                    return;
                }
                if (t == typeof(ValueTuple<,,,,,,,>)) {
                    WriteElements(writer, obj, type, 7);
                    WriteRest(writer, type.GetField("Rest").GetValue(obj));
                    return;
                }
            }
            Serializer.Instance.Write(writer, obj);
        }
        private static void WriteElements(Writer writer, object obj, Type type, int n) {
            var serializer = Serializer.Instance;
            for (int i = 1; i <= n; ++i) {
                serializer.Write(writer, type.GetField($"Item{i}").GetValue(obj));
            }
        }
    }
}