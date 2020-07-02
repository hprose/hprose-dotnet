/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  EnumDeserializer.cs                                     |
|                                                          |
|  EnumDeserializer class for C#.                          |
|                                                          |
|  LastModified: Jun 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class EnumDeserializer<T> : Deserializer<T> where T : struct, IComparable, IConvertible, IFormattable {
        public override T Read(Reader reader, int tag) => tag switch
        {
            '0' => (T)Enum.ToObject(typeof(T), 0),
            '1' => (T)Enum.ToObject(typeof(T), 1),
            '2' => (T)Enum.ToObject(typeof(T), 2),
            '3' => (T)Enum.ToObject(typeof(T), 3),
            '4' => (T)Enum.ToObject(typeof(T), 4),
            '5' => (T)Enum.ToObject(typeof(T), 5),
            '6' => (T)Enum.ToObject(typeof(T), 6),
            '7' => (T)Enum.ToObject(typeof(T), 7),
            '8' => (T)Enum.ToObject(typeof(T), 8),
            '9' => (T)Enum.ToObject(typeof(T), 9),
            TagInteger => (T)Enum.ToObject(typeof(T), ValueReader.ReadInt(reader.Stream)),
            TagLong => (T)Enum.ToObject(typeof(T), ValueReader.ReadLong(reader.Stream)),
            TagDouble => (T)Enum.ToObject(typeof(T), (long)ValueReader.ReadDouble(reader.Stream)),
            TagTrue => (T)Enum.ToObject(typeof(T), 1),
            TagFalse => (T)Enum.ToObject(typeof(T), 0),
            TagEmpty => (T)Enum.ToObject(typeof(T), 0),
            TagUTF8Char => (T)Enum.ToObject(typeof(T), ValueReader.ReadChar(reader.Stream)),
            TagString => Converter<T>.Convert(ReferenceReader.ReadString(reader)),
            _ => base.Read(reader, tag),
        };
    }
}
