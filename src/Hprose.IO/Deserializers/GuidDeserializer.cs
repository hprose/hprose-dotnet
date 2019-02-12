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
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class GuidDeserializer : Deserializer<Guid> {
        public override Guid Read(Reader reader, int tag) {
            switch (tag) {
                case TagGuid:
                    return ReferenceReader.ReadGuid(reader);
                case TagBytes:
                    return Converter<Guid>.Convert(ReferenceReader.ReadBytes(reader));
                case TagString:
                    return Converter<Guid>.Convert(ReferenceReader.ReadString(reader));
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
