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
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class InterfaceDeserializer<I> : Deserializer<I> where I : class {
        private static I Read(Reader reader) {
            Stream stream = reader.Stream;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            var typeInfo = reader.GetTypeInfo(index);
            var type = typeInfo.type;
            if (type == null) {
                type = TypeManager.GetType<I>();
                if (type == null) {
                    throw new InvalidCastException("Cannot convert " + typeInfo.name + " to " + typeof(I).ToString() + ".");
                }
            }
            return (I)((IObjectDeserializer)Deserializer.GetInstance(type)).ReadObject(reader, typeInfo.key);
        }
        public override I Read(Reader reader, int tag) {
            if (tag == TagMap) {
                var type = TypeManager.GetType<I>();
                if (type != null) {
                    return (I)Deserializer.GetInstance(type).Read(reader, tag);
                }
            }
            return tag switch
            {
                TagObject => Read(reader),
                TagEmpty => null,
                _ => base.Read(reader, tag),
            };
        }
    }
}