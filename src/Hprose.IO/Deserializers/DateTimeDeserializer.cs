/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  DateTimeDeserializer.cs                                 |
|                                                          |
|  DateTimeDeserializer class for C#.                      |
|                                                          |
|  LastModified: Jun 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class DateTimeDeserializer : Deserializer<DateTime> {
        public override DateTime Read(Reader reader, int tag) => tag switch {
            TagDate => ReferenceReader.ReadDateTime(reader),
            TagTime => ReferenceReader.ReadTime(reader),
            TagInteger => new DateTime(ValueReader.ReadInt(reader.Stream)),
            TagLong => new DateTime(ValueReader.ReadLong(reader.Stream)),
            TagDouble => new DateTime((long)ValueReader.ReadDouble(reader.Stream)),
            '0' => new DateTime(0),
            '1' => new DateTime(1),
            '2' => new DateTime(2),
            '3' => new DateTime(3),
            '4' => new DateTime(4),
            '5' => new DateTime(5),
            '6' => new DateTime(6),
            '7' => new DateTime(7),
            '8' => new DateTime(8),
            '9' => new DateTime(9),
            TagEmpty => new DateTime(0),
            TagFalse => new DateTime(0),
            TagTrue => new DateTime(1),
            TagString => Converter<DateTime>.Convert(ReferenceReader.ReadString(reader)),
            _ => base.Read(reader, tag),
        };
    }
}
