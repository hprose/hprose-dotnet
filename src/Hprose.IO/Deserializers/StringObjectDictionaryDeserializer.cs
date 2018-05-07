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
 * StringObjectDictionaryDeserializer.cs                  *
 *                                                        *
 * StringObjectDictionaryDeserializer class for C#.       *
 *                                                        *
 * LastModified: Apr 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.Collections.Generic;
using System.IO;

using Hprose.IO.Accessors;

using static Hprose.IO.Tags;

namespace Hprose.IO.Deserializers {
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
}