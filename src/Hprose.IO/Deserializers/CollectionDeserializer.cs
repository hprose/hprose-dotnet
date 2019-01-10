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
 * CollectionDeserializer.cs                              *
 *                                                        *
 * CollectionDeserializer class for C#.                   *
 *                                                        *
 * LastModified: Jan 11, 2019                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.Collections.Generic;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    class CollectionDeserializer<I, T, E> : Deserializer<I> where T : I, ICollection<E> {
        public static I Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T collection = Factory<T>.New();
            reader.AddReference(collection);
            var deserializer = Deserializer<E>.Instance;
            for (int i = 0; i < count; ++i) {
                collection.Add(deserializer.Deserialize(reader));
            }
            stream.ReadByte();
            return collection;
        }
        public override I Read(Reader reader, int tag) {
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
    class CollectionDeserializer<T, E> : CollectionDeserializer<T, T, E> where T : ICollection<E> { }
}