/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  BlockingCollectionDeserializer.cs                       |
|                                                          |
|  BlockingCollectionDeserializer class for C#.            |
|                                                          |
|  LastModified: Jun 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.IO;
using System.Collections.Concurrent;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class BlockingCollectionDeserializer<T> : Deserializer<BlockingCollection<T>> {
        public static BlockingCollection<T> Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            BlockingCollection<T> collection = new BlockingCollection<T>();
            reader.AddReference(collection);
            var deserializer = Deserializer<T>.Instance;
            for (int i = 0; i < count; ++i) {
                collection.Add(deserializer.Deserialize(reader));
            }
            stream.ReadByte();
            return collection;
        }
        public override BlockingCollection<T> Read(Reader reader, int tag) => tag switch {
            TagList => Read(reader),
            TagEmpty => new BlockingCollection<T>(),
            _ => base.Read(reader, tag),
        };
    }
}
