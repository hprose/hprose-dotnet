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
 * LastModified: Apr 1, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.Collections;
using System.Collections.Generic;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Serializers {
    class FastEnumerableSerializer<T, V> : ReferenceSerializer<T> where T : IEnumerable<V>, ICollection {
        public override void Serialize(Writer writer, T obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            int length = obj.Count;
            stream.WriteByte(TagList);
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagOpenbrace);
            var serializer = Serializer<V>.Instance;
            foreach (V value in obj) {
                serializer.Write(writer, value);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
    class EnumerableSerializer<T, V> : ReferenceSerializer<T> where T : IEnumerable<V> {
        public override void Serialize(Writer writer, T obj) {
            base.Serialize(writer, obj);
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
                serializer.Write(writer, value);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
    class FastEnumerableSerializer<T, K, V> : ReferenceSerializer<T> where T : IEnumerable<KeyValuePair<K, V>>, ICollection {
        public override void Serialize(Writer writer, T obj) {
            base.Serialize(writer, obj);
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
                serializerK.Write(writer, pair.Key);
                serializerV.Write(writer, pair.Value);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
    class EnumerableSerializer<T, K, V> : ReferenceSerializer<T> where T : IEnumerable<KeyValuePair<K, V>> {
        public override void Serialize(Writer writer, T obj) {
            base.Serialize(writer, obj);
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
                serializerK.Write(writer, pair.Key);
                serializerV.Write(writer, pair.Value);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
    class EnumerableSerializer<T> : ReferenceSerializer<T> where T : IEnumerable {
        public override void Serialize(Writer writer, T obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            int length = 0;
            bool isMap = true;
            foreach (object value in obj) {
                ++length;
                if (!(value is DictionaryEntry)) {
                    isMap = false;
                }
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
                    serializer.Write(writer, pair.Key);
                    serializer.Write(writer, pair.Value);
                }
                else {
                    serializer.Write(writer, value);
                }
            }
            stream.WriteByte(TagClosebrace);
        }
    }
}
