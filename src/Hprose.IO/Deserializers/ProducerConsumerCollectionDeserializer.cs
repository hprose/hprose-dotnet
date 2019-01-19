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
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections.Concurrent;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    class ProducerConsumerCollectionDeserializer<T, E> : Deserializer<T> where T : IProducerConsumerCollection<E> {
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
            switch (tag) {
                case TagList:
                    return Read(reader);
                case TagEmpty:
                    return Factory<T>.New();
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
