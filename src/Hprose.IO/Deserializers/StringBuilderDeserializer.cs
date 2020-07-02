/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  StringBuilderDeserializer.cs                            |
|                                                          |
|  StringBuilderDeserializer class for C#.                 |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Text;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class StringBuilderDeserializer : Deserializer<StringBuilder> {
        public override StringBuilder Read(Reader reader, int tag) => tag switch
        {
            TagString => Converter<StringBuilder>.Convert(ReferenceReader.ReadString(reader)),
            TagUTF8Char => new StringBuilder(1).Append(ValueReader.ReadChar(reader.Stream)),
            TagList => Converter<StringBuilder>.Convert(ReferenceReader.ReadArray<char>(reader)),
            TagInteger => ValueReader.ReadUntil(reader.Stream, TagSemicolon),
            TagLong => ValueReader.ReadUntil(reader.Stream, TagSemicolon),
            TagDouble => ValueReader.ReadUntil(reader.Stream, TagSemicolon),
            TagEmpty => new StringBuilder(),
            TagTrue => new StringBuilder(bool.TrueString),
            TagFalse => new StringBuilder(bool.FalseString),
            TagNaN => new StringBuilder(double.NaN.ToString()),
            TagInfinity => new StringBuilder(ValueReader.ReadInfinity(reader.Stream).ToString()),
            TagDate => Converter<StringBuilder>.Convert(ReferenceReader.ReadDateTime(reader)),
            TagTime => Converter<StringBuilder>.Convert(ReferenceReader.ReadTime(reader)),
            TagGuid => Converter<StringBuilder>.Convert(ReferenceReader.ReadGuid(reader)),
            TagBytes => Converter<StringBuilder>.Convert(ReferenceReader.ReadBytes(reader)),
            '0' => new StringBuilder("0"),
            '1' => new StringBuilder("1"),
            '2' => new StringBuilder("2"),
            '3' => new StringBuilder("3"),
            '4' => new StringBuilder("4"),
            '5' => new StringBuilder("5"),
            '6' => new StringBuilder("6"),
            '7' => new StringBuilder("7"),
            '8' => new StringBuilder("8"),
            '9' => new StringBuilder("9"),
            _ => base.Read(reader, tag),
        };
    }
}
