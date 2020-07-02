/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  CollectionDeserializer.cs                               |
|                                                          |
|  CollectionDeserializer class for C#.                    |
|                                                          |
|  LastModified: Jun 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections.Generic;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class CollectionDeserializer<I, T, E> : Deserializer<I> where T : I, ICollection<E> {
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
        public override I Read(Reader reader, int tag) => tag switch
        {
            TagList => Read(reader),
            TagEmpty => Factory<T>.New(),
            _ => base.Read(reader, tag),
        };
    }
    class CollectionDeserializer<T, E> : CollectionDeserializer<T, T, E> where T : ICollection<E> { }
}