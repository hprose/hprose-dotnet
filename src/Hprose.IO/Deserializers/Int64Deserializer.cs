/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Int64Deserializer.cs                                    |
|                                                          |
|  Int64Deserializer class for C#.                         |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    class Int64Deserializer : Deserializer<long> {
        public override long Read(Reader reader, int tag) {
            if (tag >= '0' && tag <= '9') {
                return (tag - '0');
            }
            var stream = reader.Stream;
            switch (tag) {
                case TagInteger:
                    return ValueReader.ReadInt(stream);
                case TagLong:
                    return ValueReader.ReadLong(stream);
                case TagDouble:
                    return (long)ValueReader.ReadDouble(stream);
                case TagTrue:
                    return 1;
                case TagFalse:
                case TagEmpty:
                    return 0;
                case TagUTF8Char:
                    return Converter<long>.Convert(ValueReader.ReadUTF8Char(stream));
                case TagString:
                    return Converter<long>.Convert(ReferenceReader.ReadString(reader));
                case TagDate:
                    return Converter<long>.Convert(ReferenceReader.ReadDateTime(reader));
                case TagTime:
                    return Converter<long>.Convert(ReferenceReader.ReadTime(reader));
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
