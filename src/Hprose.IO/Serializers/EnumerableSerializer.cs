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
 * EnumerableSerializer.cs                                *
 *                                                        *
 * EnumerableSerializer class for C#.                     *
 *                                                        *
 * LastModified: Apr 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.Collections;
using System.Collections.Generic;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Serializers {
    class FastEnumerableSerializer<T, V> : ReferenceSerializer<T> where T : IEnumerable<V>, ICollection {
        public override void Write(Writer writer, T obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            int length = obj.Count;
            stream.WriteByte(TagList);
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagOpenbrace);
            var serializer = Serializer<V>.Instance;
            foreach (V value in obj) {
                serializer.Serialize(writer, value);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
    class EnumerableSerializer<T, V> : ReferenceSerializer<T> where T : IEnumerable<V> {
        public override void Write(Writer writer, T obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            int length = 0;
            foreach (V value in obj) { ++length; }
            stream.WriteByte(TagList);
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagOpenbrace);
            var serializer = Serializer<V>.Instance;
            foreach (V value in obj) {
                serializer.Serialize(writer, value);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
    class FastEnumerableSerializer<T, K, V> : ReferenceSerializer<T> where T : IEnumerable<KeyValuePair<K, V>>, ICollection {
        public override void Write(Writer writer, T obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            int length = obj.Count;
            stream.WriteByte(TagMap);
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagOpenbrace);
            var serializerK = Serializer<K>.Instance;
            var serializerV = Serializer<V>.Instance;
            foreach (var pair in obj) {
                serializerK.Serialize(writer, pair.Key);
                serializerV.Serialize(writer, pair.Value);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
    class EnumerableSerializer<T, K, V> : ReferenceSerializer<T> where T : IEnumerable<KeyValuePair<K, V>> {
        public override void Write(Writer writer, T obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            int length = 0;
            foreach (var pair in obj) { ++length; }
            stream.WriteByte(TagMap);
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagOpenbrace);
            var serializerK = Serializer<K>.Instance;
            var serializerV = Serializer<V>.Instance;
            foreach (var pair in obj) {
                serializerK.Serialize(writer, pair.Key);
                serializerV.Serialize(writer, pair.Value);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
    class EnumerableSerializer<T> : ReferenceSerializer<T> where T : IEnumerable {
        public override void Write(Writer writer, T obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            int length = 0;
            bool isMap = true;
            foreach (object value in obj) {
                ++length;
                if (!(value is DictionaryEntry)) {
                    isMap = false;
                }
            }
            if (length == 0) {
                isMap = false;
            }
            stream.WriteByte(isMap ? TagMap : TagList);
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagOpenbrace);
            var serializer = Serializer.Instance;
            foreach (object value in obj) {
                if (isMap) {
                    var pair = (DictionaryEntry)value;
                    serializer.Serialize(writer, pair.Key);
                    serializer.Serialize(writer, pair.Value);
                }
                else {
                    serializer.Serialize(writer, value);
                }
            }
            stream.WriteByte(TagClosebrace);
        }
    }
}
