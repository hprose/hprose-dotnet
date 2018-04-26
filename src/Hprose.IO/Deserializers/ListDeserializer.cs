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
 * ListDeserializer.cs                                    *
 *                                                        *
 * ListDeserializer class for C#.                         *
 *                                                        *
 * LastModified: Apr 18, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.IO;
using System.Collections;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class ListDeserializer<I, T> : Deserializer<I> where T : I, IList, new() {
        public static I Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T collection = new T();
            reader.SetRef(collection);
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
                    return new T();
                default:
                    return base.Read(reader, tag);
            }
        }
    }
    class ListDeserializer<T> : ListDeserializer<T, T> where T : IList, new() { }
}