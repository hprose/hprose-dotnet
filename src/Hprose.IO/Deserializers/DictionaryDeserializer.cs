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
 * LastModified: Apr 19, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Hprose.IO.Accessors;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class DictionaryDeserializer<I, T, K, V> : Deserializer<I> where T : I, ICollection<KeyValuePair<K, V>>, new() {
        public static I Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T dict = new T();
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
        public override I Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagEmpty:
                    return new T();
                case TagMap:
                    return Read(reader);
                default:
                    return base.Read(reader, tag);
            }
        }
    }
    class DictionaryDeserializer<T, K, V> : DictionaryDeserializer<T, T, K, V> where T : ICollection<KeyValuePair<K, V>>, new() { }

    class StringObjectDictionaryDeserializer<I, T> : DictionaryDeserializer<I, T, string, object> where T : I, ICollection<KeyValuePair<string, object>>, new() {
        public static I ReadObjectAsMap(Reader reader) {
            Stream stream = reader.Stream;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            ClassInfo classInfo = reader[index];
            T dict = new T();
            reader.SetRef(dict);
            var deserializer = Deserializer.Instance;
            if (classInfo.Type != null) {
                var members = Accessor.GetMembers(classInfo.Type, reader.Mode);
                int count = classInfo.Members.Length;
                for (int i = 0; i < count; ++i) {
                    var member = members[classInfo.Members[i]];
                    if (member != null) {
                        var v = reader.Deserialize(member is FieldInfo ? ((FieldInfo)member).FieldType : ((PropertyInfo)member).PropertyType);
                        dict.Add(new KeyValuePair<string, object>(member.Name, v));
                    }
                    else {
                        var v = deserializer.Deserialize(reader);
                        dict.Add(new KeyValuePair<string, object>(classInfo.Members[i], v));
                    }
                }
            }
            else {
                string[] members = classInfo.Members;
                int count = members.Length;
                for (int i = 0; i < count; ++i) {
                    var v = deserializer.Deserialize(reader);
                    dict.Add(new KeyValuePair<string, object>(members[i], v));
                }
            }
            stream.ReadByte();
            return dict;
        }
        public override I Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagObject:
                    return ReadObjectAsMap(reader);
                default:
                    return base.Read(reader, tag);
            }
        }
    }
    class StringObjectDictionaryDeserializer<T> : StringObjectDictionaryDeserializer<T, T> where T : ICollection<KeyValuePair<string, object>>, new() { }

    class DictionaryDeserializer<I, T> : Deserializer<I> where T : I, IDictionary, new() {
        public static I Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T dict = new T();
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
        public static I ReadObjectAsMap(Reader reader) {
            Stream stream = reader.Stream;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            ClassInfo classInfo = reader[index];
            T dict = new T();
            reader.SetRef(dict);
            var deserializer = Deserializer.Instance;
            if (classInfo.Type != null) {
                var members = Accessor.GetMembers(classInfo.Type, reader.Mode);
                int count = classInfo.Members.Length;
                for (int i = 0; i < count; ++i) {
                    var member = members[classInfo.Members[i]];
                    if (member != null) {
                        var v = reader.Deserialize(member is FieldInfo ? ((FieldInfo)member).FieldType : ((PropertyInfo)member).PropertyType);
                        dict.Add(member.Name, v);
                    }
                    else {
                        var v = deserializer.Deserialize(reader);
                        dict.Add(classInfo.Members[i], v);
                    }
                }
            }
            else {
                string[] members = classInfo.Members;
                int count = members.Length;
                for (int i = 0; i < count; ++i) {
                    var v = deserializer.Deserialize(reader);
                    dict.Add(members[i], v);
                }
            }
            stream.ReadByte();
            return dict;
        }
        public override I Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagEmpty:
                    return new T();
                case TagMap:
                    return Read(reader);
                case TagObject:
                    return ReadObjectAsMap(reader);
                default:
                    return base.Read(reader, tag);
            }
        }
    }

    class DictionaryDeserializer<T> : DictionaryDeserializer<T, T> where T : IDictionary, new() { }
}