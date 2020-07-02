/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  StringDeserializer.cs                                   |
|                                                          |
|  StringDeserializer class for C#.                        |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class StringDeserializer : Deserializer<string> {
        public override string Read(Reader reader, int tag) => tag switch
        {
            TagString => ReferenceReader.ReadString(reader),
            TagUTF8Char => ValueReader.ReadUTF8Char(reader.Stream),
            '0' => "0",
            '1' => "1",
            '2' => "2",
            '3' => "3",
            '4' => "4",
            '5' => "5",
            '6' => "6",
            '7' => "7",
            '8' => "8",
            '9' => "9",
            TagInteger => ValueReader.ReadUntil(reader.Stream, TagSemicolon).ToString(),
            TagLong => ValueReader.ReadUntil(reader.Stream, TagSemicolon).ToString(),
            TagDouble => ValueReader.ReadUntil(reader.Stream, TagSemicolon).ToString(),
            TagEmpty => "",
            TagTrue => bool.TrueString,
            TagFalse => bool.FalseString,
            TagNaN => double.NaN.ToString(),
            TagInfinity => ValueReader.ReadInfinity(reader.Stream).ToString(),
            TagDate => ReferenceReader.ReadDateTime(reader).ToString(),
            TagTime => ReferenceReader.ReadTime(reader).ToString(),
            TagGuid => ReferenceReader.ReadGuid(reader).ToString(),
            TagBytes => Converter<string>.Convert(ReferenceReader.ReadBytes(reader)),
            TagList => Converter<string>.Convert(ReferenceReader.ReadArray<char>(reader)),
            _ => base.Read(reader, tag),
        };
    }
}
