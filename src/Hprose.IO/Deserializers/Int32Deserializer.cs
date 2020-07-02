/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Int32Deserializer.cs                                    |
|                                                          |
|  Int32Deserializer class for C#.                         |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class Int32Deserializer : Deserializer<int> {
        public override int Read(Reader reader, int tag) => tag switch
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
            TagLong => (int)ValueReader.ReadLong(reader.Stream),
            TagDouble => (int)ValueReader.ReadDouble(reader.Stream),
            TagTrue => 1,
            TagFalse => 0,
            TagEmpty => 0,
            TagUTF8Char => Converter<int>.Convert(ValueReader.ReadUTF8Char(reader.Stream)),
            TagString => Converter<int>.Convert(ReferenceReader.ReadString(reader)),
            _ => base.Read(reader, tag),
        };
    }
}
