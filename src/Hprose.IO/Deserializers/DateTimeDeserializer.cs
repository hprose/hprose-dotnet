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
 * DateTimeDeserializer.cs                                *
 *                                                        *
 * DateTimeDeserializer class for C#.                     *
 *                                                        *
 * LastModified: Apr 14, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;

using Hprose.IO.Converters;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class DateTimeDeserializer : Deserializer<DateTime> {
        public override DateTime Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagDate:
                    return ReferenceReader.ReadDateTime(reader);
                case TagTime:
                    return ReferenceReader.ReadTime(reader);
                case TagInteger:
                    return new DateTime(ValueReader.ReadInt(stream));
                case TagLong:
                    return new DateTime(ValueReader.ReadLong(stream));
                case TagDouble:
                    return new DateTime((long)ValueReader.ReadDouble(stream));
                case '0':
                case TagEmpty:
                case TagFalse:
                    return new DateTime(0);
                case '1':
                case TagTrue:
                    return new DateTime(1);
                case TagString:
                    return Converter<DateTime>.Convert(ReferenceReader.ReadString(reader));
                default:
                    if (tag >= '2' && tag <= '9') {
                        return new DateTime(tag - '0');
                    }
                    return base.Read(reader, tag);
            }
        }
    }
}
