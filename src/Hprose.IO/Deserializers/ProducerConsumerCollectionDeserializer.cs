/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ProducerConsumerCollectionDeserializer.cs               |
|                                                          |
|  ProducerConsumerCollectionDeserializer class for C#.    |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections.Concurrent;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class ProducerConsumerCollectionDeserializer<T, E> : Deserializer<T> where T : IProducerConsumerCollection<E> {
        public static T Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T collection = Factory<T>.New();
            reader.AddReference(collection);
            var deserializer = Deserializer<E>.Instance;
            for (int i = 0; i < count; ++i) {
                collection.TryAdd(deserializer.Deserialize(reader));
            }
            stream.ReadByte();
            return collection;
        }
        public override T Read(Reader reader, int tag) {
            return tag switch
            {
                TagList => Read(reader),
                TagEmpty => Factory<T>.New(),
                _ => base.Read(reader, tag),
            };
        }
    }
}
