/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  SByteDeserializer.cs                                    |
|                                                          |
|  SByteDeserializer class for C#.                         |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class SByteDeserializer : Deserializer<sbyte> {
        public override sbyte Read(Reader reader, int tag) => tag switch {
            '0' => 0,
            '1' => 1,
            '2' => 2,
            '3' => 3,
            '4' => 4,
            '5' => 5,
            '6' => 6,
            '7' => 7,
            '8' => 8,
            '9' => 9,
            TagInteger => (sbyte)ValueReader.ReadInt(reader.Stream),
            TagLong => (sbyte)ValueReader.ReadLong(reader.Stream),
            TagDouble => (sbyte)ValueReader.ReadDouble(reader.Stream),
            TagTrue => 1,
            TagFalse => 0,
            TagEmpty => 0,
            TagUTF8Char => Converter<sbyte>.Convert(ValueReader.ReadUTF8Char(reader.Stream)),
            TagString => Converter<sbyte>.Convert(ReferenceReader.ReadString(reader)),
            _ => base.Read(reader, tag),
        };
    }
}
