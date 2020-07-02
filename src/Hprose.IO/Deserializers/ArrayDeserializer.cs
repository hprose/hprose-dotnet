/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ArrayDeserializer.cs                                    |
|                                                          |
|  ArrayDeserializer class for C#.                         |
|                                                          |
|  LastModified: Jun 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class ArrayDeserializer<T> : Deserializer<T[]> {
        private static readonly T[] empty = new T[0];
        public override T[] Read(Reader reader, int tag) => tag switch
        {
            TagList => ReferenceReader.ReadArray<T>(reader),
            TagEmpty => empty,
            _ => base.Read(reader, tag),
        };
    }

    internal class Array2Deserializer<T> : Deserializer<T[,]> {
        private static readonly T[,] empty = new T[0, 0];
        private static T[,] Read(Reader reader) {
            Stream stream = reader.Stream;
            int count1 = ValueReader.ReadCount(stream);
            if (stream.ReadByte() != TagList) {
                throw new RankException();
            }
            int count2 = ValueReader.ReadCount(stream);
            T[,] a = new T[count1, count2];
            reader.AddReference(a);
            reader.AddReference(null);
            var deserializer = Deserializer<T>.Instance;
            for (int i = 0; i < count1; ++i) {
                for (int j = 0; j < count2; ++j) {
                    a[i, j] = deserializer.Deserialize(reader);
                }
                stream.ReadByte();
                if (i < count1 - 1) {
                    if (stream.ReadByte() != TagList) {
                        throw new RankException();
                    }
                    ValueReader.SkipUntil(stream, TagOpenbrace);
                    reader.AddReference(null);
                }
            }
            stream.ReadByte();
            return a;
        }
        public override T[,] Read(Reader reader, int tag) => tag switch
        {
            TagList => Read(reader),
            TagEmpty => empty,
            _ => base.Read(reader, tag),
        };
    }
}
