/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  CharDeserializer.cs                                     |
|                                                          |
|  CharDeserializer class for C#.                          |
|                                                          |
|  LastModified: Jun 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class CharDeserializer : Deserializer<char> {
        public override char Read(Reader reader, int tag) => tag switch {
            TagUTF8Char => ValueReader.ReadChar(reader.Stream),
            TagEmpty => '\0',
            TagInteger => (char)ValueReader.ReadInt(reader.Stream),
            TagLong => (char)ValueReader.ReadLong(reader.Stream),
            TagDouble => (char)ValueReader.ReadDouble(reader.Stream),
            TagString => Converter<char>.Convert(ReferenceReader.ReadString(reader)),
            '0' => (char)0,
            '1' => (char)1,
            '2' => (char)2,
            '3' => (char)3,
            '4' => (char)4,
            '5' => (char)5,
            '6' => (char)6,
            '7' => (char)7,
            '8' => (char)8,
            '9' => (char)9,
            _ => base.Read(reader, tag),
        };
    }
}
