/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  StreamDeserializer.cs                                   |
|                                                          |
|  StreamDeserializer class for C#.                        |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class StreamDeserializer<T> : Deserializer<T> where T : Stream {
        private static readonly byte[] empty = new byte[0];
        public override T Read(Reader reader, int tag) => tag switch
        {
            TagBytes => Converter<T>.Convert(ReferenceReader.ReadBytes(reader)),
            TagEmpty => Converter<T>.Convert(empty),
            TagList => Converter<T>.Convert(ReferenceReader.ReadArray<byte>(reader)),
            TagUTF8Char => Converter<T>.Convert(ValueReader.ReadUTF8Char(reader.Stream)),
            TagString => Converter<T>.Convert(ReferenceReader.ReadChars(reader)),
            _ => base.Read(reader, tag),
        };
    }
}
