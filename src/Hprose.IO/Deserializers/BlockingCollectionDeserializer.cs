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
 * BlockingCollectionDeserializer.cs                      *
 *                                                        *
 * BlockingCollectionDeserializer class for C#.           *
 *                                                        *
 * LastModified: Apr 16, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.IO;
using System.Collections.Concurrent;

using static Hprose.IO.Tags;

namespace Hprose.IO.Deserializers {
    class BlockingCollectionDeserializer<T> : Deserializer<BlockingCollection<T>> {
        public static BlockingCollection<T> Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            BlockingCollection<T> collection = new BlockingCollection<T>();
            reader.SetRef(collection);
            var deserializer = Deserializer<T>.Instance;
            for (int i = 0; i < count; ++i) {
                collection.Add(deserializer.Deserialize(reader));
            }
            stream.ReadByte();
            return collection;
        }
        public override BlockingCollection<T> Read(Reader reader, int tag) {
            switch (tag) {
                case TagList:
                    return Read(reader);
                case TagEmpty:
                    return new BlockingCollection<T>();
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
