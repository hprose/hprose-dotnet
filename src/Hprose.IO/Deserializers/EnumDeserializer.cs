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
 * EnumDeserializer.cs                                    *
 *                                                        *
 * EnumDeserializer class for C#.                         *
 *                                                        *
 * LastModified: Jan 11, 2019                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;

namespace Hprose.IO.Deserializers {
    using static Tags;

    class EnumDeserializer<T> : Deserializer<T> where T : struct, IComparable, IConvertible, IFormattable {
        public override T Read(Reader reader, int tag) {
            Type type = typeof(T);
            if (tag >= '0' && tag <= '9') {
                return (T)Enum.ToObject(type, tag - '0');
            }
            var stream = reader.Stream;
            switch (tag) {
                case TagInteger:
                    return (T)Enum.ToObject(type, ValueReader.ReadInt(stream));
                case TagLong:
                    return (T)Enum.ToObject(type, ValueReader.ReadLong(stream));
                case TagDouble:
                    return (T)Enum.ToObject(type, (long)ValueReader.ReadDouble(stream));
                case TagTrue:
                    return (T)Enum.ToObject(type, 1);
                case TagFalse:
                case TagEmpty:
                    return (T)Enum.ToObject(type, 0);
                case TagUTF8Char:
                    return (T)Enum.ToObject(type, ValueReader.ReadChar(stream));
                case TagString:
                    return Converter<T>.Convert(ReferenceReader.ReadString(reader));
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
