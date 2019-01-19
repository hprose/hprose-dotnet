/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ByteDeserializer.cs                                     |
|                                                          |
|  ByteDeserializer class for C#.                          |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    class ByteDeserializer : Deserializer<byte> {
        public override byte Read(Reader reader, int tag) {
            if (tag >= '0' && tag <= '9') {
                return (byte)(tag - '0');
            }
            var stream = reader.Stream;
            switch (tag) {
                case TagInteger:
                    return (byte)ValueReader.ReadInt(stream);
                case TagLong:
                    return (byte)ValueReader.ReadLong(stream);
                case TagDouble:
                    return (byte)ValueReader.ReadDouble(stream);
                case TagTrue:
                    return 1;
                case TagFalse:
                case TagEmpty:
                    return 0;
                case TagUTF8Char:
                    return Converter<byte>.Convert(ValueReader.ReadUTF8Char(stream));
                case TagString:
                    return Converter<byte>.Convert(ReferenceReader.ReadString(reader));
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
