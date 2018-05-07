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
 * BooleanDeserializer.cs                                 *
 *                                                        *
 * BooleanDeserializer class for C#.                      *
 *                                                        *
 * LastModified: Apr 10, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using Hprose.IO.Converters;

using static Hprose.IO.Tags;

namespace Hprose.IO.Deserializers {
    class BooleanDeserializer : Deserializer<bool> {
        public override bool Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagTrue:
                case TagNaN:
                    return true;
                case TagFalse:
                case TagEmpty:
                case '0':
                    return false;
                case TagInteger:
                    return ValueReader.ReadInt(stream) != 0;
                case TagLong:
                    return !ValueReader.ReadBigInteger(stream).IsZero;
                case TagDouble:
                    return ValueReader.ReadDouble(stream) != 0;
                case TagUTF8Char:
                    return "0\0".IndexOf(ValueReader.ReadChar(stream)) == -1;
                case TagString:
                    return Converter<bool>.Convert(ReferenceReader.ReadString(reader));
                case TagInfinity:
                    stream.ReadByte();
                    return true;
                default:
                    if (tag >= '1' && tag <= '9') {
                        return true;
                    }
                    return base.Read(reader, tag);
            }
        }
    }
}
