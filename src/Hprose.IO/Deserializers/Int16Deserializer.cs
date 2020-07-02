/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Int16Deserializer.cs                                    |
|                                                          |
|  Int16Deserializer class for C#.                         |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class Int16Deserializer : Deserializer<short> {
        public override short Read(Reader reader, int tag) {
            return tag switch
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
                TagInteger => (short)ValueReader.ReadInt(reader.Stream),
                TagLong => (short)ValueReader.ReadLong(reader.Stream),
                TagDouble => (short)ValueReader.ReadDouble(reader.Stream),
                TagTrue => 1,
                TagFalse => 0,
                TagEmpty => 0,
                TagUTF8Char => Converter<short>.Convert(ValueReader.ReadUTF8Char(reader.Stream)),
                TagString => Converter<short>.Convert(ReferenceReader.ReadString(reader)),
                _ => base.Read(reader, tag),
            };
        }
    }
}
