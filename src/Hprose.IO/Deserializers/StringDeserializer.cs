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
 * StringDeserializer.cs                                  *
 *                                                        *
 * StringDeserializer class for C#.                       *
 *                                                        *
 * LastModified: Apr 14, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;

using Hprose.IO.Converters;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class StringDeserializer : Deserializer<string> {
        public override string Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagString:
                    return ReferenceReader.ReadString(reader);
                case TagUTF8Char:
                    return ValueReader.ReadUTF8Char(stream);
                case '0': return "0";
                case '1': return "1";
                case '2': return "2";
                case '3': return "3";
                case '4': return "4";
                case '5': return "5";
                case '6': return "6";
                case '7': return "7";
                case '8': return "8";
                case '9': return "9";
                case TagInteger:
                case TagLong:
                case TagDouble:
                    return ValueReader.ReadUntil(stream, TagSemicolon).ToString();
                case TagEmpty:
                    return "";
                case TagTrue:
                    return bool.TrueString;
                case TagFalse:
                    return bool.FalseString;
                case TagNaN:
                    return double.NaN.ToString();
                case TagInfinity:
                    return ValueReader.ReadInfinity(stream).ToString();
                case TagDate:
                    return ReferenceReader.ReadDateTime(reader).ToString();
                case TagTime:
                    return ReferenceReader.ReadTime(reader).ToString();
                case TagGuid:
                    return ReferenceReader.ReadGuid(reader).ToString();
                case TagBytes:
                    return Converter<string>.Instance.Convert(ReferenceReader.ReadBytes(reader));
                case TagList:
                    return Converter<string>.Instance.Convert(ReferenceReader.ReadArray<char>(reader));
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
