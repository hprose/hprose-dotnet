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
 * UInt16Deserializer.cs                                  *
 *                                                        *
 * UInt16Deserializer class for C#.                       *
 *                                                        *
 * LastModified: Jan 11, 2019                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Deserializers {
    using static Tags;

    class UInt16Deserializer : Deserializer<ushort> {
        public override ushort Read(Reader reader, int tag) {
            if (tag >= '0' && tag <= '9') {
                return (ushort)(tag - '0');
            }
            var stream = reader.Stream;
            switch (tag) {
                case TagInteger:
                    return (ushort)ValueReader.ReadInt(stream);
                case TagLong:
                    return (ushort)ValueReader.ReadLong(stream);
                case TagDouble:
                    return (ushort)ValueReader.ReadDouble(stream);
                case TagTrue:
                    return 1;
                case TagFalse:
                case TagEmpty:
                    return 0;
                case TagUTF8Char:
                    return Converter<ushort>.Convert(ValueReader.ReadUTF8Char(stream));
                case TagString:
                    return Converter<ushort>.Convert(ReferenceReader.ReadString(reader));
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
