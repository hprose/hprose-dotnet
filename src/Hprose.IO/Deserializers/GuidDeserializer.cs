/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  GuidDeserializer.cs                                     |
|                                                          |
|  GuidDeserializer class for C#.                          |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class GuidDeserializer : Deserializer<Guid> {
        public override Guid Read(Reader reader, int tag) => tag switch {
            TagGuid => ReferenceReader.ReadGuid(reader),
            TagBytes => Converter<Guid>.Convert(ReferenceReader.ReadBytes(reader)),
            TagString => Converter<Guid>.Convert(ReferenceReader.ReadString(reader)),
            _ => base.Read(reader, tag),
        };
    }
}
