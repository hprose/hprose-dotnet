/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  TimeSpanDeserializer.cs                                 |
|                                                          |
|  TimeSpanDeserializer class for C#.                      |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class TimeSpanDeserializer : Deserializer<TimeSpan> {
        public override TimeSpan Read(Reader reader, int tag) => tag switch
        {
            TagInteger => new TimeSpan(ValueReader.ReadInt(reader.Stream)),
            TagLong => new TimeSpan(ValueReader.ReadLong(reader.Stream)),
            TagDouble => new TimeSpan((long)ValueReader.ReadDouble(reader.Stream)),
            TagEmpty => TimeSpan.Zero,
            TagFalse => TimeSpan.Zero,
            TagTrue => new TimeSpan(1),
            '0' => TimeSpan.Zero,
            '1' => new TimeSpan(1),
            '2' => new TimeSpan(2),
            '3' => new TimeSpan(3),
            '4' => new TimeSpan(4),
            '5' => new TimeSpan(5),
            '6' => new TimeSpan(6),
            '7' => new TimeSpan(7),
            '8' => new TimeSpan(8),
            '9' => new TimeSpan(9),
            TagUTF8Char => Converter<TimeSpan>.Convert(ValueReader.ReadUTF8Char(reader.Stream)),
            TagString => Converter<TimeSpan>.Convert(ReferenceReader.ReadString(reader)),
            TagDate => Converter<TimeSpan>.Convert(ReferenceReader.ReadDateTime(reader)),
            TagTime => Converter<TimeSpan>.Convert(ReferenceReader.ReadTime(reader)),
            _ => base.Read(reader, tag),
        };
    }
}
