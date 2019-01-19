/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  DictionarySerializer.cs                                 |
|                                                          |
|  DictionarySerializer class for C#.                      |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections;
using System.Collections.Generic;

namespace Hprose.IO.Serializers {
    using static Tags;

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
