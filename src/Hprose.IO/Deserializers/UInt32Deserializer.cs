/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  UInt32Deserializer.cs                                   |
|                                                          |
|  UInt32Deserializer class for C#.                        |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class UInt32Deserializer : Deserializer<uint> {
        public override uint Read(Reader reader, int tag) => tag switch {
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
            TagInteger => (uint)ValueReader.ReadInt(reader.Stream),
            TagLong => (uint)ValueReader.ReadLong(reader.Stream),
            TagDouble => (uint)ValueReader.ReadDouble(reader.Stream),
            TagTrue => 1,
            TagFalse => 0,
            TagEmpty => 0,
            TagUTF8Char => Converter<uint>.Convert(ValueReader.ReadUTF8Char(reader.Stream)),
            TagString => Converter<uint>.Convert(ReferenceReader.ReadString(reader)),
            _ => base.Read(reader, tag),
        };
    }
}
