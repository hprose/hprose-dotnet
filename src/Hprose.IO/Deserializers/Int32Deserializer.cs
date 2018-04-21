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
 * Int32Deserializer.cs                                   *
 *                                                        *
 * Int32Deserializer class for C#.                        *
 *                                                        *
 * LastModified: Apr 9, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using Hprose.IO.Converters;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class Int32Deserializer : Deserializer<int> {
        public override int Read(Reader reader, int tag) {
            if (tag >= '0' && tag <= '9') {
                return (tag - '0');
            }
            var stream = reader.Stream;
            switch (tag) {
                case TagInteger:
                    return ValueReader.ReadInt(stream);
                case TagLong:
                    return (int)ValueReader.ReadLong(stream);
                case TagDouble:
                    return (int)ValueReader.ReadDouble(stream);
                case TagTrue:
                    return 1;
                case TagFalse:
                case TagEmpty:
                    return 0;
                case TagUTF8Char:
                    return Converter<int>.Convert(ValueReader.ReadUTF8Char(stream));
                case TagString:
                    return Converter<int>.Convert(ReferenceReader.ReadString(reader));
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
