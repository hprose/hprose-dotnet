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
 * ProducerConsumerCollectionDeserializer.cs              *
 *                                                        *
 * ProducerConsumerCollectionDeserializer class for C#.   *
 *                                                        *
 * LastModified: Apr 17, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.IO;
using System.Collections.Concurrent;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class ProducerConsumerCollectionDeserializer<T, E> : Deserializer<T> where T : IProducerConsumerCollection<E>, new() {
        public static T Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T collection = new T();
            reader.SetRef(collection);
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
                    return new T();
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
