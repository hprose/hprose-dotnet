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
 * ObjectDeserializer.cs                                  *
 *                                                        *
 * ObjectDeserializer class for C#.                       *
 *                                                        *
 * LastModified: Dec 13, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.IO;

using static Hprose.IO.Tags;

namespace Hprose.IO.Deserializers {
    interface IObjectDeserializer {
        object ReadObject(Reader reader, string key);
    }

    class ObjectDeserializer<T> : Deserializer<T>, IObjectDeserializer where T : class {
        private static T Read(Reader reader, string key) {
            Stream stream = reader.Stream;
            T obj = Factory<T>.New();
            reader.AddReference(obj);
            MembersReader.ReadAllMembers(reader, key, ref obj);
            stream.ReadByte();
            return obj;
        }
        private static T Read(Reader reader) {
            Stream stream = reader.Stream;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            return Read(reader, reader.GetTypeInfo(index).key);
        }
        private static T ReadMapAsObject(Reader reader) {
            Stream stream = reader.Stream;
            T obj = Factory<T>.New();
            reader.AddReference(obj);
            int count = ValueReader.ReadCount(stream);
            var strDeserializer = Deserializer<string>.Instance;
            for (int i = 0; i < count; ++i) {
                var name = strDeserializer.Deserialize(reader);
                MembersReader.ReadMember(reader, name, ref obj);
            }
            stream.ReadByte();
            return obj;
        }
        public override T Read(Reader reader, int tag) {
            switch (tag) {
                case TagObject:
                    return Read(reader);
                case TagMap:
                    return ReadMapAsObject(reader);
                case TagEmpty:
                    return Factory<T>.New();
                default:
                    return base.Read(reader, tag);
            }
        }
        public object ReadObject(Reader reader, string key) {
            return Read(reader, key);
        }
    }
    class StructDeserializer<T> : Deserializer<T>, IObjectDeserializer where T : struct {
        private static T Read(Reader reader, string key) {
            Stream stream = reader.Stream;
            T obj = Factory<T>.New();
            reader.AddReference(null);
            int refIndex = reader.LastReferenceIndex;
            MembersReader.ReadAllMembers(reader, key, ref obj);
            reader.SetReference(refIndex, obj);
            stream.ReadByte();
            return obj;
        }
        private static T Read(Reader reader) {
            Stream stream = reader.Stream;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            return Read(reader, reader.GetTypeInfo(index).key);
        }
        private static T ReadMapAsObject(Reader reader) {
            Stream stream = reader.Stream;
            T obj = Factory<T>.New();
            reader.AddReference(null);
            int refIndex = reader.LastReferenceIndex;
            int count = ValueReader.ReadCount(stream);
            var strDeserializer = Deserializer<string>.Instance;
            for (int i = 0; i < count; ++i) {
                var name = strDeserializer.Deserialize(reader);
                MembersReader.ReadMember(reader, name, ref obj);
            }
            reader.SetReference(refIndex, obj);
            stream.ReadByte();
            return obj;
        }
        public override T Read(Reader reader, int tag) {
            switch (tag) {
                case TagObject:
                    return Read(reader);
                case TagMap:
                    return ReadMapAsObject(reader);
                case TagEmpty:
                    return Factory<T>.New();
                default:
                    return base.Read(reader, tag);
            }
        }
        public object ReadObject(Reader reader, string key) {
            return Read(reader, key);
        }
    }
}