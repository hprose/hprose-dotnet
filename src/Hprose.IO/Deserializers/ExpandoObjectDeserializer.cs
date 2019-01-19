/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ExpandoObjectDeserializer.cs                            |
|                                                          |
|  ExpandoObjectDeserializer class for C#.                 |
|                                                          |
|  LastModified: Jan 19, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    class ExpandoObjectDeserializer : Deserializer<ExpandoObject> {
        public static ExpandoObject Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            var obj = new ExpandoObject();
            reader.AddReference(obj);
            var dict = (IDictionary<string, object>)obj;
            var strDeserializer = StringDeserializer.Instance;
            var deserializer = Deserializer.Instance;
            for (int i = 0; i < count; ++i) {
                var k = strDeserializer.Deserialize(reader);
                var v = deserializer.Deserialize(reader);
                dict.Add(Accessor.TitleCaseName(k), v);
            }
            stream.ReadByte();
            return obj;
        }
        private static ExpandoObject ReadObject(Reader reader) {
            Stream stream = reader.Stream;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            TypeInfo typeInfo = reader.GetTypeInfo(index);
            var obj = new ExpandoObject();
            reader.AddReference(obj);
            var dict = (IDictionary<string, object>)obj;
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
                        dict.Add(Accessor.TitleCaseName(names[i]), deserializer.Deserialize(reader));
                    }
                }
            }
            else {
                for (int i = 0; i < count; ++i) {
                    dict.Add(Accessor.TitleCaseName(names[i]), deserializer.Deserialize(reader));
                }
            }
            stream.ReadByte();
            return obj;
        }
        public override ExpandoObject Read(Reader reader, int tag) {
            switch (tag) {
                case TagMap:
                    return Read(reader);
                case TagObject:
                    return ReadObject(reader);
                case TagEmpty:
                    return new ExpandoObject();
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}