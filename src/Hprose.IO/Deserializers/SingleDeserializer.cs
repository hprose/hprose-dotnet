/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  SingleDeserializer.cs                                   |
|                                                          |
|  SingleDeserializer class for C#.                        |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class SingleDeserializer : Deserializer<float> {
        public override float Read(Reader reader, int tag) => tag switch {
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
            TagDouble => ValueReader.ReadSingle(reader.Stream),
            TagInteger => ValueReader.ReadIntAsSingle(reader.Stream),
            TagLong => ValueReader.ReadIntAsSingle(reader.Stream),
            TagNaN => float.NaN,
            TagInfinity => ValueReader.ReadSingleInfinity(reader.Stream),
            TagTrue => 1,
            TagFalse => 0,
            TagEmpty => 0,
            TagUTF8Char => Converter<float>.Convert(ValueReader.ReadUTF8Char(reader.Stream)),
            TagString => Converter<float>.Convert(ReferenceReader.ReadString(reader)),
            _ => base.Read(reader, tag),
        };
    }
}
