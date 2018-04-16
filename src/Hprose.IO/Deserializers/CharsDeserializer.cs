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
 * CharsDeserializer.cs                                   *
 *                                                        *
 * CharsDeserializer class for C#.                        *
 *                                                        *
 * LastModified: Apr 16, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using Hprose.IO.Converters;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class CharsDeserializer : Deserializer<char[]> {
        private static readonly char[] EmptyChars = new char[0] { };
        public override char[] Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagString:
                    return ReferenceReader.ReadChars(reader);
                case TagUTF8Char:
                    return new char[] { ValueReader.ReadChar(stream) };
                case TagList:
                    return ReferenceReader.ReadArray<char>(reader);
                case TagEmpty:
                    return EmptyChars;
                case TagTrue:
                    return bool.TrueString.ToCharArray();
                case TagFalse:
                    return bool.FalseString.ToCharArray();
                case TagNaN:
                    return double.NaN.ToString().ToCharArray();
                case TagInfinity:
                    return ValueReader.ReadInfinity(stream).ToString().ToCharArray();
                case TagInteger:
                case TagLong:
                case TagDouble:
                    return Converter<char[]>.Instance.Convert(ValueReader.ReadUntil(stream, TagSemicolon));
                case TagDate:
                    return Converter<char[]>.Instance.Convert(ReferenceReader.ReadDateTime(reader));
                case TagTime:
                    return Converter<char[]>.Instance.Convert(ReferenceReader.ReadTime(reader));
                case TagGuid:
                    return Converter<char[]>.Instance.Convert(ReferenceReader.ReadGuid(reader));
                case TagBytes:
                    return Converter<char[]>.Instance.Convert(ReferenceReader.ReadBytes(reader));
                default:
                    if (tag >= '0' && tag <= '9') {
                        return new char[] { (char)tag };
                    }
                    return base.Read(reader, tag);
            }
        }
    }
}
