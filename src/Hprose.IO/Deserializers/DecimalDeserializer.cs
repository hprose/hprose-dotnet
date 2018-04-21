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
 * DecimalDeserializer.cs                                 *
 *                                                        *
 * DecimalDeserializer class for C#.                      *
 *                                                        *
 * LastModified: Apr 18, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using Hprose.IO.Converters;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class DecimalDeserializer : Deserializer<decimal> {
        public override decimal Read(Reader reader, int tag) {
            var stream = reader.Stream;
            if (tag >= '0' && tag <= '9') {
                return (tag - '0');
            }
            switch (tag) {
                case TagInteger:
                case TagLong:
                    return ValueReader.ReadIntAsDecimal(stream);
                case TagDouble:
                    return ValueReader.ReadDecimal(stream);
                case TagTrue:
                    return 1;
                case TagFalse:
                case TagEmpty:
                    return 0;
                case TagUTF8Char:
                    return Converter<decimal>.Convert(ValueReader.ReadUTF8Char(stream));
                case TagString:
                    return Converter<decimal>.Convert(ReferenceReader.ReadString(reader));
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
