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
 * Int64Deserializer.cs                                   *
 *                                                        *
 * Int64Deserializer class for C#.                        *
 *                                                        *
 * LastModified: Apr 10, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using Hprose.IO.Converters;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class Int64Deserializer : Deserializer<long> {
        public override long Read(Reader reader, int tag) {
            if (tag >= '0' && tag <= '9') {
                return (tag - '0');
            }
            var stream = reader.Stream;
            switch (tag) {
                case TagInteger:
                    return ValueReader.ReadInt(stream);
                case TagLong:
                    return ValueReader.ReadLong(stream);
                case TagDouble:
                    return (long)ValueReader.ReadDouble(stream);
                case TagTrue:
                    return 1;
                case TagFalse:
                case TagEmpty:
                    return 0;
                case TagUTF8Char:
                    return Converter<long>.Instance.Convert(ValueReader.ReadUTF8Char(stream));
                case TagString:
                    return Converter<long>.Instance.Convert(ReferenceReader.ReadString(reader));
                case TagDate:
                    return Converter<long>.Instance.Convert(ReferenceReader.ReadDateTime(reader));
                case TagTime:
                    return Converter<long>.Instance.Convert(ReferenceReader.ReadTime(reader));
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
