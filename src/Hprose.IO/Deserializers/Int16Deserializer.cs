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
 * Int16Deserializer.cs                                   *
 *                                                        *
 * Int16Deserializer class for C#.                        *
 *                                                        *
 * LastModified: Apr 9, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using Hprose.IO.Converters;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class Int16Deserializer : Deserializer<short> {
        public override short Read(Reader reader, int tag) {
            if (tag >= '0' && tag <= '9') {
                return (short)(tag - '0');
            }
            var stream = reader.Stream;
            switch (tag) {
                case TagInteger:
                    return (short)ValueReader.ReadInt(stream);
                case TagLong:
                    return (short)ValueReader.ReadLong(stream);
                case TagDouble:
                    return (short)ValueReader.ReadDouble(stream);
                case TagTrue:
                    return 1;
                case TagFalse:
                case TagEmpty:
                    return 0;
                case TagUTF8Char:
                    return Converter<short>.Convert(ValueReader.ReadUTF8Char(stream));
                case TagString:
                    return Converter<short>.Convert(ReferenceReader.ReadString(reader));
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
