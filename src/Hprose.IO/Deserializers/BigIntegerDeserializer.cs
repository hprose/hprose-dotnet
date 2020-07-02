/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  BigIntegerDeserializer.cs                               |
|                                                          |
|  BigIntegerDeserializer class for C#.                    |
|                                                          |
|  LastModified: Jun 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Numerics;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class BigIntegerDeserializer : Deserializer<BigInteger> {
        public override BigInteger Read(Reader reader, int tag) => tag switch
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
            TagInteger => ValueReader.ReadInt(reader.Stream),
            TagLong => ValueReader.ReadBigInteger(reader.Stream),
            TagDouble => (BigInteger)ValueReader.ReadDouble(reader.Stream),
            TagTrue => 1,
            TagFalse => 0,
            TagEmpty => 0,
            TagUTF8Char => Converter<BigInteger>.Convert(ValueReader.ReadUTF8Char(reader.Stream)),
            TagString => Converter<BigInteger>.Convert(ReferenceReader.ReadString(reader)),
            TagDate => Converter<BigInteger>.Convert(ReferenceReader.ReadDateTime(reader)),
            TagTime => Converter<BigInteger>.Convert(ReferenceReader.ReadTime(reader)),
            _ => base.Read(reader, tag),
        };
    }
}
