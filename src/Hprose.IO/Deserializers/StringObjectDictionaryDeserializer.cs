/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  StringObjectDictionaryDeserializer.cs                   |
|                                                          |
|  StringObjectDictionaryDeserializer class for C#.        |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections.Generic;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class StringObjectDictionaryDeserializer<I, T> : DictionaryDeserializer<I, T, string, object> where T : I, ICollection<KeyValuePair<string, object>> {
        public static I ReadObjectAsMap(Reader reader) {
            Stream stream = reader.Stream;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            TypeInfo typeInfo = reader.GetTypeInfo(index);
            T dict = Factory<T>.New();
            reader.AddReference(dict);
            var deserializer = Deserializer.Instance;
            string[] names = typeInfo.names;
            int count = names.Length;
            if (typeInfo.type != null) {
                var members = Accessor.GetMembers(typeInfo.type, reader.Mode);
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
        public override I Read(Reader reader, int tag) => tag switch {
            TagObject => ReadObjectAsMap(reader),
            _ => base.Read(reader, tag),
        };
    }

    internal class StringObjectDictionaryDeserializer<T> : StringObjectDictionaryDeserializer<T, T> where T : ICollection<KeyValuePair<string, object>> { }
}