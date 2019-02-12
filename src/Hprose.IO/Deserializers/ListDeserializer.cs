/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ListDeserializer.cs                                     |
|                                                          |
|  ListDeserializer class for C#.                          |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class ListDeserializer<I, T> : Deserializer<I> where T : I, IList {
        public static I Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T collection = Factory<T>.New();
            reader.AddReference(collection);
            var deserializer = Deserializer.Instance;
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

    internal class ListDeserializer<T> : ListDeserializer<T, T> where T : IList { }
}