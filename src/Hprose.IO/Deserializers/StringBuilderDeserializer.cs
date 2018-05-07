/**********************************************************\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: http://www.hprose.com/                 |
|                   http://www.hprose.org/                 |
|                                                          |
\**********************************************************/
/**********************************************************\
 *                                                        *
 * StringBuilderDeserializer.cs                           *
 *                                                        *
 * StringBuilderDeserializer class for C#.                *
 *                                                        *
 * LastModified: Apr 14, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Text;

using Hprose.IO.Converters;

using static Hprose.IO.Tags;

namespace Hprose.IO.Deserializers {
    class StringBuilderDeserializer : Deserializer<StringBuilder> {
        public override StringBuilder Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagString:
                    return Converter<StringBuilder>.Convert(ReferenceReader.ReadString(reader));
                case TagUTF8Char:
                    return new StringBuilder(1).Append(ValueReader.ReadChar(stream));
                case TagList:
                    return Converter<StringBuilder>.Convert(ReferenceReader.ReadArray<char>(reader));
                case TagInteger:
                case TagLong:
                case TagDouble:
                    return ValueReader.ReadUntil(stream, TagSemicolon);
                case TagEmpty:
                    return new StringBuilder();
                case TagTrue:
                    return new StringBuilder(bool.TrueString);
                case TagFalse:
                    return new StringBuilder(bool.FalseString);
                case TagNaN:
                    return new StringBuilder(double.NaN.ToString());
                case TagInfinity:
                    return new StringBuilder(ValueReader.ReadInfinity(stream).ToString());
                case TagDate:
                    return Converter<StringBuilder>.Convert(ReferenceReader.ReadDateTime(reader));
                case TagTime:
                    return Converter<StringBuilder>.Convert(ReferenceReader.ReadTime(reader));
                case TagGuid:
                    return Converter<StringBuilder>.Convert(ReferenceReader.ReadGuid(reader));
                case TagBytes:
                    return Converter<StringBuilder>.Convert(ReferenceReader.ReadBytes(reader));
                default:
                    if (tag >= '0' && tag <= '9') {
                        return new StringBuilder(1).Append((char)tag);
                    }
                    return base.Read(reader, tag);
            }
        }
    }
}
