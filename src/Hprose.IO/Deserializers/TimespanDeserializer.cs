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
 * TimeSpanDeserializer.cs                                *
 *                                                        *
 * TimeSpanDeserializer class for C#.                     *
 *                                                        *
 * LastModified: Apr 14, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;

using Hprose.IO.Converters;

using static Hprose.IO.Tags;

namespace Hprose.IO.Deserializers {
    class TimeSpanDeserializer : Deserializer<TimeSpan> {
        public override TimeSpan Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagInteger:
                    return new TimeSpan(ValueReader.ReadInt(stream));
                case TagLong:
                    return new TimeSpan(ValueReader.ReadLong(stream));
                case TagDouble:
                    return new TimeSpan((long)ValueReader.ReadDouble(stream));
                case '0':
                case TagEmpty:
                case TagFalse:
                    return TimeSpan.Zero;
                case '1':
                case TagTrue:
                    return new TimeSpan(1);
                case TagUTF8Char:
                    return Converter<TimeSpan>.Convert(ValueReader.ReadUTF8Char(stream));
                case TagString:
                    return Converter<TimeSpan>.Convert(ReferenceReader.ReadString(reader));
                case TagDate:
                    return Converter<TimeSpan>.Convert(ReferenceReader.ReadDateTime(reader));
                case TagTime:
                    return Converter<TimeSpan>.Convert(ReferenceReader.ReadTime(reader));
                default:
                    if (tag >= '2' && tag <= '9') {
                        return new TimeSpan(tag - '0');
                    }
                    return base.Read(reader, tag);
            }
        }
    }
}
