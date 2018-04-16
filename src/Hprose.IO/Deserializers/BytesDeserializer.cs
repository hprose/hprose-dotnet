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
 * BytesDeserializer.cs                                   *
 *                                                        *
 * BytesDeserializer class for C#.                        *
 *                                                        *
 * LastModified: Apr 16, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using Hprose.IO.Converters;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class BytesDeserializer : Deserializer<byte[]> {
        private static readonly byte[] EmptyBytes = new byte[0] { };
        public override byte[] Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagEmpty:
                    return EmptyBytes;
                case TagBytes:
                    return ReferenceReader.ReadBytes(reader);
                case TagList:
                    return ReferenceReader.ReadArray<byte>(reader);
                case TagUTF8Char:
                    return Converter<byte[]>.Instance.Convert(ValueReader.ReadUTF8Char(stream));
                case TagString:
                    return Converter<byte[]>.Instance.Convert(ReferenceReader.ReadChars(reader));
                case TagGuid:
                    return Converter<byte[]>.Instance.Convert(ReferenceReader.ReadGuid(reader));
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
