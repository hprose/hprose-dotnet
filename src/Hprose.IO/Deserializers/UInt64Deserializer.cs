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
 * UInt64Deserializer.cs                                  *
 *                                                        *
 * UInt64Deserializer class for C#.                       *
 *                                                        *
 * LastModified: Apr 9, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using Hprose.IO.Converters;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class UInt64Deserializer : Deserializer<ulong> {
        public override ulong Read(Reader reader, int tag) {
            if (tag >= '0' && tag <= '9') {
                return (ulong)(tag - '0');
            }
            var stream = reader.Stream;
            switch (tag) {
                case TagInteger:
                    return (ulong)ValueReader.ReadInt(stream);
                case TagLong:
                    return (ulong)ValueReader.ReadLong(stream);
                case TagDouble:
                    return (ulong)ValueReader.ReadDouble(stream);
                case TagTrue:
                    return 1;
                case TagFalse:
                case TagEmpty:
                    return 0;
                case TagUTF8Char:
                    return Converter<ulong>.Convert(ValueReader.ReadUTF8Char(stream));
                case TagString:
                    return Converter<ulong>.Convert(ReferenceReader.ReadString(reader));
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
