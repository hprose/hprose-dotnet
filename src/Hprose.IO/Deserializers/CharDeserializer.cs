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
 * CharDeserializer.cs                                    *
 *                                                        *
 * CharDeserializer class for C#.                         *
 *                                                        *
 * LastModified: Apr 13, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using Hprose.IO.Converters;

using static Hprose.IO.Tags;

namespace Hprose.IO.Deserializers {
    class CharDeserializer : Deserializer<char> {
        public override char Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagUTF8Char:
                    return ValueReader.ReadChar(stream);
                case TagEmpty:
                    return '\0';
                case TagInteger:
                    return (char)ValueReader.ReadInt(stream);
                case TagLong:
                    return (char)ValueReader.ReadLong(stream);
                case TagDouble:
                    return (char)ValueReader.ReadDouble(stream);
                case TagString:
                    return Converter<char>.Convert(ReferenceReader.ReadString(reader));
                default:
                    if (tag >= '0' && tag <= '9') {
                        return (char)(tag - '0');
                    }
                    return base.Read(reader, tag);
            }
        }
    }
}
