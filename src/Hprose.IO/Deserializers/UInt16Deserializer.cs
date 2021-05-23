/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  UInt16Deserializer.cs                                   |
|                                                          |
|  UInt16Deserializer class for C#.                        |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class UInt16Deserializer : Deserializer<ushort> {
        public override ushort Read(Reader reader, int tag) => tag switch {
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
            TagInteger => (ushort)ValueReader.ReadInt(reader.Stream),
            TagLong => (ushort)ValueReader.ReadLong(reader.Stream),
            TagDouble => (ushort)ValueReader.ReadDouble(reader.Stream),
            TagTrue => 1,
            TagFalse => 0,
            TagEmpty => 0,
            TagUTF8Char => Converter<ushort>.Convert(ValueReader.ReadUTF8Char(reader.Stream)),
            TagString => Converter<ushort>.Convert(ReferenceReader.ReadString(reader)),
            _ => base.Read(reader, tag),
        };
    }
}
