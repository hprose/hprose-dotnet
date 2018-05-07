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
 * DoubleDeserializer.cs                                  *
 *                                                        *
 * DoubleDeserializer class for C#.                       *
 *                                                        *
 * LastModified: Apr 10, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using Hprose.IO.Converters;

using static Hprose.IO.Tags;

namespace Hprose.IO.Deserializers {
    class DoubleDeserializer : Deserializer<double> {
        public override double Read(Reader reader, int tag) {
            var stream = reader.Stream;
            if (tag >= '0' && tag <= '9') {
                return (tag - '0');
            }
            switch (tag) {
                case TagDouble:
                    return ValueReader.ReadDouble(stream);
                case TagInteger:
                case TagLong:
                    return ValueReader.ReadIntAsDouble(stream);
                case TagNaN:
                    return double.NaN;
                case TagInfinity:
                    return ValueReader.ReadInfinity(stream);
                case TagTrue:
                    return 1;
                case TagFalse:
                case TagEmpty:
                    return 0;
                case TagUTF8Char:
                    return Converter<double>.Convert(ValueReader.ReadUTF8Char(stream));
                case TagString:
                    return Converter<double>.Convert(ReferenceReader.ReadString(reader));
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
