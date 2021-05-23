/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  DecimalDeserializer.cs                                  |
|                                                          |
|  DecimalDeserializer class for C#.                       |
|                                                          |
|  LastModified: Jun 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class DecimalDeserializer : Deserializer<decimal> {
        public override decimal Read(Reader reader, int tag) => tag switch {
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
            TagInteger => ValueReader.ReadIntAsDecimal(reader.Stream),
            TagLong => ValueReader.ReadIntAsDecimal(reader.Stream),
            TagDouble => ValueReader.ReadDecimal(reader.Stream),
            TagTrue => 1,
            TagFalse => 0,
            TagEmpty => 0,
            TagUTF8Char => Converter<decimal>.Convert(ValueReader.ReadUTF8Char(reader.Stream)),
            TagString => Converter<decimal>.Convert(ReferenceReader.ReadString(reader)),
            _ => base.Read(reader, tag),
        };
    }
}
