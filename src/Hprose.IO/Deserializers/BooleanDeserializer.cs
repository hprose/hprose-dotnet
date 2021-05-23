/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  BooleanDeserializer.cs                                  |
|                                                          |
|  BooleanDeserializer class for C#.                       |
|                                                          |
|  LastModified: Jun 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class BooleanDeserializer : Deserializer<bool> {
        public override bool Read(Reader reader, int tag) => tag switch {
            TagTrue => true,
            TagFalse => false,
            TagEmpty => false,
            TagNaN => false,
            '0' => false,
            '1' => true,
            '2' => true,
            '3' => true,
            '4' => true,
            '5' => true,
            '6' => true,
            '7' => true,
            '8' => true,
            '9' => true,
            TagInteger => ValueReader.ReadInt(reader.Stream) != 0,
            TagLong => !ValueReader.ReadBigInteger(reader.Stream).IsZero,
            TagDouble => ValueReader.ReadDouble(reader.Stream) != 0,
            TagUTF8Char => "0\0".IndexOf(ValueReader.ReadChar(reader.Stream)) == -1,
            TagString => Converter<bool>.Convert(ReferenceReader.ReadString(reader)),
            TagInfinity => reader.Stream.ReadByte() != -1,
            _ => base.Read(reader, tag),
        };
    }
}
