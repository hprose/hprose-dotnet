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
 * DictionarySerializer.cs                                *
 *                                                        *
 * DictionarySerializer class for C#.                     *
 *                                                        *
 * LastModified: Apr 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.Collections;
using System.Collections.Generic;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Serializers {
    class DictionarySerializer<T, K, V> : ReferenceSerializer<T> where T : ICollection<KeyValuePair<K, V>> {
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
    class DictionarySerializer<T> : ReferenceSerializer<T> where T : IDictionary {
        public override void Write(Writer writer, T obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            int length = obj.Count;
            stream.WriteByte(TagMap);
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagOpenbrace);
            var serializer = Serializer.Instance;
            foreach (DictionaryEntry pair in obj) {
                serializer.Serialize(writer, pair.Key);
                serializer.Serialize(writer, pair.Value);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
}
