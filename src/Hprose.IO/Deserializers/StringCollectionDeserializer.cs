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
 * StringCollectionDeserializer.cs                        *
 *                                                        *
 * StringCollectionDeserializer class for C#.             *
 *                                                        *
 * LastModified: Apr 17, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.IO;
using System.Collections.Specialized;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class StringCollectionDeserializer : Deserializer<StringCollection> {
        public static StringCollection ReadCollection(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            StringCollection collection = new StringCollection();
            reader.SetRef(collection);
            var deserializer = Deserializer<string>.Instance;
            for (int i = 0; i < count; ++i) {
                collection.Add(deserializer.Deserialize(reader));
            }
            stream.ReadByte();
            return collection;
        }
        public override StringCollection Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagEmpty:
                    return new StringCollection();
                case TagList:
                    return ReadCollection(reader);
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
