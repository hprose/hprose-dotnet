/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  CharsDeserializer.cs                                    |
|                                                          |
|  CharsDeserializer class for C#.                         |
|                                                          |
|  LastModified: Jun 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class CharsDeserializer : Deserializer<char[]> {
        private static readonly char[] empty = new char[0];
        public override char[] Read(Reader reader, int tag) => tag switch
        {
            TagString => ReferenceReader.ReadChars(reader),
            TagUTF8Char => new char[] { ValueReader.ReadChar(reader.Stream) },
            TagList => ReferenceReader.ReadArray<char>(reader),
            TagEmpty => empty,
            TagTrue => bool.TrueString.ToCharArray(),
            TagFalse => bool.FalseString.ToCharArray(),
            TagNaN => double.NaN.ToString().ToCharArray(),
            TagInfinity => ValueReader.ReadInfinity(reader.Stream).ToString().ToCharArray(),
            TagInteger => Converter<char[]>.Convert(ValueReader.ReadUntil(reader.Stream, TagSemicolon)),
            TagLong => Converter<char[]>.Convert(ValueReader.ReadUntil(reader.Stream, TagSemicolon)),
            TagDouble => Converter<char[]>.Convert(ValueReader.ReadUntil(reader.Stream, TagSemicolon)),
            TagDate => Converter<char[]>.Convert(ReferenceReader.ReadDateTime(reader)),
            TagTime => Converter<char[]>.Convert(ReferenceReader.ReadTime(reader)),
            TagGuid => Converter<char[]>.Convert(ReferenceReader.ReadGuid(reader)),
            TagBytes => Converter<char[]>.Convert(ReferenceReader.ReadBytes(reader)),
            '0' => new char[] { '0' },
            '1' => new char[] { '1' },
            '2' => new char[] { '2' },
            '3' => new char[] { '3' },
            '4' => new char[] { '4' },
            '5' => new char[] { '5' },
            '6' => new char[] { '6' },
            '7' => new char[] { '7' },
            '8' => new char[] { '8' },
            '9' => new char[] { '9' },
            _ => base.Read(reader, tag),
        };
    }
}
