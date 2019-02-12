/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  SByteDeserializer.cs                                    |
|                                                          |
|  SByteDeserializer class for C#.                         |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class SByteDeserializer : Deserializer<sbyte> {
        public override sbyte Read(Reader reader, int tag) {
            if (tag >= '0' && tag <= '9') {
                return (sbyte)(tag - '0');
            }
            var stream = reader.Stream;
            switch (tag) {
                case TagInteger:
                    return (sbyte)ValueReader.ReadInt(stream);
                case TagLong:
                    return (sbyte)ValueReader.ReadLong(stream);
                case TagDouble:
                    return (sbyte)ValueReader.ReadDouble(stream);
                case TagTrue:
                    return 1;
                case TagFalse:
                case TagEmpty:
                    return 0;
                case TagUTF8Char:
                    return Converter<sbyte>.Convert(ValueReader.ReadUTF8Char(stream));
                case TagString:
                    return Converter<sbyte>.Convert(ReferenceReader.ReadString(reader));
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
