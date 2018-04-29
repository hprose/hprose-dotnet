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
 * DictionaryDeserializer.cs                              *
 *                                                        *
 * DictionaryDeserializer class for C#.                   *
 *                                                        *
 * LastModified: Apr 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;

using Hprose.IO.Accessors;
using Hprose.IO.Converters;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class DictionaryDeserializer<I, T, K, V> : Deserializer<I> where T : I, ICollection<KeyValuePair<K, V>> {
        public static I Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T dict = Factory<T>.New();
            reader.SetRef(dict);
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
            reader.SetRef(dict);
            var deserializer = Deserializer<V>.Instance;
            for (int i = 0; i < count; ++i) {
                dict.Add(new KeyValuePair<K, V>(Converter<K>.Convert(i), deserializer.Deserialize(reader)));
            }
            stream.ReadByte();
            return dict;
        }
        public override I Read(Reader reader, int tag) {
            switch (tag) {
                case TagMap:
                    return Read(reader);
                case TagList:
                    return ReadListAsMap(reader);
                case TagEmpty:
                    return Factory<T>.New();
                default:
                    return base.Read(reader, tag);
            }
        }
    }
    class DictionaryDeserializer<T, K, V> : DictionaryDeserializer<T, T, K, V> where T : ICollection<KeyValuePair<K, V>> { }

    class StringObjectDictionaryDeserializer<I, T> : DictionaryDeserializer<I, T, string, object> where T : I, ICollection<KeyValuePair<string, object>> {
        public static I ReadObjectAsMap(Reader reader) {
            Stream stream = reader.Stream;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            ClassInfo classInfo = reader.GetClassInfo(index);
            T dict = Factory<T>.New();
            reader.SetRef(dict);
            var deserializer = Deserializer.Instance;
            string[] names = classInfo.names;
            int count = names.Length;
            if (classInfo.type != null) {
                var members = Accessor.GetMembers(classInfo.type, reader.Mode);
                for (int i = 0; i < count; ++i) {
                    var name = names[i];
                    var member = members[name];
                    if (member != null) {
                        dict.Add(new KeyValuePair<string, object>(member.Name, reader.Deserialize(Accessor.GetMemberType(member))));
                    }
                    else {
                        dict.Add(new KeyValuePair<string, object>(name, deserializer.Deserialize(reader)));
                    }
                }
            }
            else {
                for (int i = 0; i < count; ++i) {
                    dict.Add(new KeyValuePair<string, object>(names[i], deserializer.Deserialize(reader)));
                }
            }
            stream.ReadByte();
            return dict;
        }
        public override I Read(Reader reader, int tag) {
            switch (tag) {
                case TagObject:
                    return ReadObjectAsMap(reader);
                default:
                    return base.Read(reader, tag);
            }
        }
    }
    class StringObjectDictionaryDeserializer<T> : StringObjectDictionaryDeserializer<T, T> where T : ICollection<KeyValuePair<string, object>> { }

    class DictionaryDeserializer<I, T> : Deserializer<I> where T : I, IDictionary {
        public static I Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T dict = Factory<T>.New();
            reader.SetRef(dict);
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
            reader.SetRef(dict);
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
            ClassInfo classInfo = reader.GetClassInfo(index);
            T dict = Factory<T>.New();
            reader.SetRef(dict);
            var deserializer = Deserializer.Instance;
            var names = classInfo.names;
            int count = names.Length;
            if (classInfo.type != null) {
                var members = Accessor.GetMembers(classInfo.type, reader.Mode);
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
        public override I Read(Reader reader, int tag) {
            switch (tag) {
                case TagMap:
                    return Read(reader);
                case TagList:
                    return ReadListAsMap(reader);
                case TagObject:
                    return ReadObjectAsMap(reader);
                case TagEmpty:
                    return Factory<T>.New();
                default:
                    return base.Read(reader, tag);
            }
        }
    }

    class DictionaryDeserializer<T> : DictionaryDeserializer<T, T> where T : IDictionary { }
}