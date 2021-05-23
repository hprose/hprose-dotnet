﻿/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  BitArrayDeserializer.cs                                 |
|                                                          |
|  BitArrayDeserializer class for C#.                      |
|                                                          |
|  LastModified: Jun 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.IO;
using System.Collections;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class BitArrayDeserializer : Deserializer<BitArray> {
        public static BitArray Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            BitArray array = new BitArray(count);
            reader.AddReference(array);
            var deserializer = Deserializer<bool>.Instance;
            for (int i = 0; i < count; ++i) {
                array[i] = deserializer.Deserialize(reader);
            }
            stream.ReadByte();
            return array;
        }
        public override BitArray Read(Reader reader, int tag) => tag switch {
            TagList => Read(reader),
            TagEmpty => new BitArray(0),
            _ => base.Read(reader, tag),
        };
    }
}