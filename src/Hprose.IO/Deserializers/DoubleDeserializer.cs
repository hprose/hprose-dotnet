/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  DoubleDeserializer.cs                                   |
|                                                          |
|  DoubleDeserializer class for C#.                        |
|                                                          |
|  LastModified: Jun 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class DoubleDeserializer : Deserializer<double> {
        public override double Read(Reader reader, int tag) => tag switch {
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
            TagDouble => ValueReader.ReadDouble(reader.Stream),
            TagInteger => ValueReader.ReadIntAsDouble(reader.Stream),
            TagLong => ValueReader.ReadIntAsDouble(reader.Stream),
            TagNaN => double.NaN,
            TagInfinity => ValueReader.ReadInfinity(reader.Stream),
            TagTrue => 1,
            TagFalse => 0,
            TagEmpty => 0,
            TagUTF8Char => Converter<double>.Convert(ValueReader.ReadUTF8Char(reader.Stream)),
            TagString => Converter<double>.Convert(ReferenceReader.ReadString(reader)),
            _ => base.Read(reader, tag),
        };
    }
}
