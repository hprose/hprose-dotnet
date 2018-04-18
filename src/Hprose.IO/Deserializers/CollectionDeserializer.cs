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
 * LastModified: Apr 15, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.IO;
using System.Collections.Generic;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class CollectionDeserializer<I, T, E> : Deserializer<I> where T : I, ICollection<E>, new() {
        public static I Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T collection = new T();
            reader.SetRef(collection);
            var deserializer = Deserializer<E>.Instance;
            for (int i = 0; i < count; ++i) {
                collection.Add(deserializer.Deserialize(reader));
            }
            stream.ReadByte();
            return collection;
        }
        public override I Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagEmpty:
                    return new T();
                case TagList:
                    return Read(reader);
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
