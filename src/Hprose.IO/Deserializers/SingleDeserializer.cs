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
 * SingleDeserializer.cs                                  *
 *                                                        *
 * SingleDeserializer class for C#.                       *
 *                                                        *
 * LastModified: Apr 10, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using Hprose.IO.Converters;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class SingleDeserializer : Deserializer<float> {
        public override float Read(Reader reader, int tag) {
            var stream = reader.Stream;
            if (tag >= '0' && tag <= '9') {
                return (tag - '0');
            }
            switch (tag) {
                case TagDouble:
                    return ValueReader.ReadSingle(stream);
                case TagInteger:
                case TagLong:
                    return ValueReader.ReadIntAsSingle(stream);
                case TagNaN:
                    return float.NaN;
                case TagInfinity:
                    return ValueReader.ReadSingleInfinity(stream);
                case TagTrue:
                    return 1;
                case TagFalse:
                case TagEmpty:
                    return 0;
                case TagUTF8Char:
                    return Converter<float>.Convert(ValueReader.ReadUTF8Char(stream));
                case TagString:
                    return Converter<float>.Convert(ReferenceReader.ReadString(reader));
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
