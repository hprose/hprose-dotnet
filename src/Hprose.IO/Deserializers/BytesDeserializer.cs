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
|  LastModified: Jun 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class BytesDeserializer : Deserializer<byte[]> {
        private static readonly byte[] empty = new byte[0];
        public override byte[] Read(Reader reader, int tag) => tag switch
        {
            TagBytes => ReferenceReader.ReadBytes(reader),
            TagEmpty => empty,
            TagList => ReferenceReader.ReadArray<byte>(reader),
            TagUTF8Char => Converter<byte[]>.Convert(ValueReader.ReadUTF8Char(reader.Stream)),
            TagString => Converter<byte[]>.Convert(ReferenceReader.ReadChars(reader)),
            TagGuid => Converter<byte[]>.Convert(ReferenceReader.ReadGuid(reader)),
            _ => base.Read(reader, tag),
        };
    }
}
