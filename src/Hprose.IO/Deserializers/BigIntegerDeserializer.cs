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
 * BigIntegerDeserializer.cs                              *
 *                                                        *
 * BigIntegerDeserializer class for C#.                   *
 *                                                        *
 * LastModified: Apr 13, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.Numerics;
using Hprose.IO.Converters;

using static Hprose.IO.Tags;

namespace Hprose.IO.Deserializers {
    class BigIntegerDeserializer : Deserializer<BigInteger> {
        public override BigInteger Read(Reader reader, int tag) {
            if (tag >= '0' && tag <= '9') {
                return (tag - '0');
            }
            var stream = reader.Stream;
            switch (tag) {
                case TagInteger:
                    return ValueReader.ReadInt(stream);
                case TagLong:
                    return ValueReader.ReadBigInteger(stream);
                case TagDouble:
                    return (BigInteger)ValueReader.ReadDouble(stream);
                case TagTrue:
                    return 1;
                case TagFalse:
                case TagEmpty:
                    return 0;
                case TagUTF8Char:
                    return Converter<BigInteger>.Convert(ValueReader.ReadUTF8Char(stream));
                case TagString:
                    return Converter<BigInteger>.Convert(ReferenceReader.ReadString(reader));
                case TagDate:
                    return Converter<BigInteger>.Convert(ReferenceReader.ReadDateTime(reader));
                case TagTime:
                    return Converter<BigInteger>.Convert(ReferenceReader.ReadTime(reader));
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
