/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Int64Deserializer.cs                                    |
|                                                          |
|  Int64Deserializer class for C#.                         |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class Int64Deserializer : Deserializer<long> {
        public override long Read(Reader reader, int tag) => tag switch
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
            TagLong => ValueReader.ReadLong(reader.Stream),
            TagDouble => (long)ValueReader.ReadDouble(reader.Stream),
            TagTrue => 1,
            TagFalse => 0,
            TagEmpty => 0,
            TagUTF8Char => Converter<long>.Convert(ValueReader.ReadUTF8Char(reader.Stream)),
            TagString => Converter<long>.Convert(ReferenceReader.ReadString(reader)),
            TagDate => Converter<long>.Convert(ReferenceReader.ReadDateTime(reader)),
            TagTime => Converter<long>.Convert(ReferenceReader.ReadTime(reader)),
            _ => base.Read(reader, tag),
        };
    }
}
