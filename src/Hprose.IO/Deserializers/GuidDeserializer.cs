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
 * GuidDeserializer.cs                                    *
 *                                                        *
 * GuidDeserializer class for C#.                         *
 *                                                        *
 * LastModified: Apr 13, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;

using Hprose.IO.Converters;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class GuidDeserializer : Deserializer<Guid> {
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
