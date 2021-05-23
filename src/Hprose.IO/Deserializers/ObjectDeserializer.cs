﻿/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ObjectDeserializer.cs                                   |
|                                                          |
|  ObjectDeserializer class for C#.                        |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal interface IObjectDeserializer {
        object ReadObject(Reader reader, string key);
    }

    internal class ObjectDeserializer<T> : Deserializer<T>, IObjectDeserializer where T : class {
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
            var typeInfo = reader.GetTypeInfo(index);
            var type = typeof(T);
            if (typeInfo.type == type) {
                return Read(reader, typeInfo.key);
            }
            if (typeInfo.type.IsSubclassOf(type)) {
                return (T)((IObjectDeserializer)Deserializer.GetInstance(typeInfo.type)).ReadObject(reader, typeInfo.key);
            }
            throw new InvalidCastException("Cannot convert " + typeInfo.type.ToString() + " to " + type.ToString() + ".");
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
        public override T Read(Reader reader, int tag) => tag switch {
            TagObject => Read(reader),
            TagMap => ReadMapAsObject(reader),
            TagEmpty => Factory<T>.New(),
            _ => base.Read(reader, tag),
        };
        public object ReadObject(Reader reader, string key) => Read(reader, key);
    }

    internal class StructDeserializer<T> : Deserializer<T>, IObjectDeserializer where T : struct {
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
            var typeInfo = reader.GetTypeInfo(index);
            var type = typeof(T);
            if (typeInfo.type == type) {
                return Read(reader, typeInfo.key);
            }
            if (typeInfo.type.IsSubclassOf(type)) {
                return (T)((IObjectDeserializer)Deserializer.GetInstance(typeInfo.type)).ReadObject(reader, typeInfo.key);
            }
            throw new InvalidCastException("Cannot convert " + typeInfo.type.ToString() + " to " + type.ToString() + ".");
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
        public override T Read(Reader reader, int tag) => tag switch {
            TagObject => Read(reader),
            TagMap => ReadMapAsObject(reader),
            TagEmpty => Factory<T>.New(),
            _ => base.Read(reader, tag),
        };

        public object ReadObject(Reader reader, string key) => Read(reader, key);
    }
}