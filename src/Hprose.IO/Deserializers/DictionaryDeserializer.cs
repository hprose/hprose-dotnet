/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  DictionaryDeserializer.cs                               |
|                                                          |
|  DictionaryDeserializer class for C#.                    |
|                                                          |
|  LastModified: Jun 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class DictionaryDeserializer<I, T, K, V> : Deserializer<I> where T : I, ICollection<KeyValuePair<K, V>> {
        public static I Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T dict = Factory<T>.New();
            reader.AddReference(dict);
            var keyDeserializer = Deserializer<K>.Instance;
            var valueDeserializer = Deserializer<V>.Instance;
            for (int i = 0; i < count; ++i) {
                var k = keyDeserializer.Deserialize(reader);
                var v = valueDeserializer.Deserialize(reader);
                dict.Add(new KeyValuePair<K, V>(k, v));
            }
            stream.ReadByte();
            return dict;
        }
        public static I ReadListAsMap(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T dict = Factory<T>.New();
            reader.AddReference(dict);
            var deserializer = Deserializer<V>.Instance;
            for (int i = 0; i < count; ++i) {
                dict.Add(new KeyValuePair<K, V>(Converter<K>.Convert(i), deserializer.Deserialize(reader)));
            }
            stream.ReadByte();
            return dict;
        }
        public override I Read(Reader reader, int tag) => tag switch {
            TagMap => Read(reader),
            TagList => ReadListAsMap(reader),
            TagEmpty => Factory<T>.New(),
            _ => base.Read(reader, tag),
        };
    }

    internal class DictionaryDeserializer<T, K, V> : DictionaryDeserializer<T, T, K, V> where T : ICollection<KeyValuePair<K, V>> { }

    internal class DictionaryDeserializer<I, T> : Deserializer<I> where T : I, IDictionary {
        public static I Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T dict = Factory<T>.New();
            reader.AddReference(dict);
            var deserializer = Deserializer.Instance;
            for (int i = 0; i < count; ++i) {
                var k = deserializer.Deserialize(reader);
                var v = deserializer.Deserialize(reader);
                dict.Add(k, v);
            }
            stream.ReadByte();
            return dict;
        }
        public static I ReadListAsMap(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T dict = Factory<T>.New();
            reader.AddReference(dict);
            var deserializer = Deserializer.Instance;
            for (int i = 0; i < count; ++i) {
                var v = deserializer.Deserialize(reader);
                dict.Add(i, v);
            }
            stream.ReadByte();
            return dict;
        }
        public static I ReadObjectAsMap(Reader reader) {
            Stream stream = reader.Stream;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            TypeInfo typeInfo = reader.GetTypeInfo(index);
            T dict = Factory<T>.New();
            reader.AddReference(dict);
            var deserializer = Deserializer.Instance;
            var names = typeInfo.names;
            int count = names.Length;
            if (typeInfo.type != null) {
                var members = Accessor.GetMembers(typeInfo.type, reader.Mode);
                for (int i = 0; i < count; ++i) {
                    var member = members[names[i]];
                    if (member != null) {
                        dict.Add(member.Name, reader.Deserialize(Accessor.GetMemberType(member)));
                    }
                    else {
                        dict.Add(names[i], deserializer.Deserialize(reader));
                    }
                }
            }
            else {
                for (int i = 0; i < count; ++i) {
                    dict.Add(names[i], deserializer.Deserialize(reader));
                }
            }
            stream.ReadByte();
            return dict;
        }
        public override I Read(Reader reader, int tag) => tag switch {
            TagMap => Read(reader),
            TagList => ReadListAsMap(reader),
            TagObject => ReadObjectAsMap(reader),
            TagEmpty => Factory<T>.New(),
            _ => base.Read(reader, tag),
        };
    }

    internal class DictionaryDeserializer<T> : DictionaryDeserializer<T, T> where T : IDictionary { }
}