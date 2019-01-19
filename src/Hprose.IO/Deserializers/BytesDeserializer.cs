/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  BytesDeserializer.cs                                    |
|                                                          |
|  BytesDeserializer class for C#.                         |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    class BytesDeserializer : Deserializer<byte[]> {
        private static readonly byte[] empty = new byte[0] { };
        public override byte[] Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagBytes:
                    return ReferenceReader.ReadBytes(reader);
                case TagEmpty:
                    return empty;
                case TagList:
                    return ReferenceReader.ReadArray<byte>(reader);
                case TagUTF8Char:
                    return Converter<byte[]>.Convert(ValueReader.ReadUTF8Char(stream));
                case TagString:
                    return Converter<byte[]>.Convert(ReferenceReader.ReadChars(reader));
                case TagGuid:
                    return Converter<byte[]>.Convert(ReferenceReader.ReadGuid(reader));
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
