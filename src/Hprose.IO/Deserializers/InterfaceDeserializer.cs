/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  InterfaceDeserializer.cs                                |
|                                                          |
|  InterfaceDeserializer class for C#.                     |
|                                                          |
|  LastModified: Jan 20, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    class InterfaceDeserializer<I> : Deserializer<I> where I : class {
        private static I Read(Reader reader) {
            Stream stream = reader.Stream;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            var typeInfo = reader.GetTypeInfo(index);
            var type = typeInfo.type;
            if (type == null) {
                type = TypeManager.GetType<I>();
            }
            if (type != null) {
                return (I)((IObjectDeserializer)Deserializer.GetInstance(type)).ReadObject(reader, typeInfo.key);
            }
            throw new InvalidCastException("Cannot convert " + typeInfo.name + " to " + typeof(I).ToString() + ".");
        }
        public override I Read(Reader reader, int tag) {
            if (tag == TagMap) {
                var type = TypeManager.GetType<I>();
                if (type != null) {
                    return (I)Deserializer.GetInstance(type).Read(reader, tag);
                }
            }
            switch (tag) {
                case TagObject:
                    return Read(reader);
                case TagEmpty:
                    return null;
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}