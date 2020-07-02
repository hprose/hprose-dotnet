/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ByteDeserializer.cs                                     |
|                                                          |
|  ByteDeserializer class for C#.                          |
|                                                          |
|  LastModified: Jun 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class ByteDeserializer : Deserializer<byte> {
        public override byte Read(Reader reader, int tag) => tag switch
        {
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
            TagInteger => (byte)ValueReader.ReadInt(reader.Stream),
            TagLong => (byte)ValueReader.ReadLong(reader.Stream),
            TagDouble => (byte)ValueReader.ReadDouble(reader.Stream),
            TagTrue => 1,
            TagFalse => 0,
            TagEmpty => 0,
            TagUTF8Char => Converter<byte>.Convert(ValueReader.ReadUTF8Char(reader.Stream)),
            TagString => Converter<byte>.Convert(ReferenceReader.ReadString(reader)),
            _ => base.Read(reader, tag),
        };
    }
}
