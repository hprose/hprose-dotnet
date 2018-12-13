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
 * LastModified: Dec 13, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.IO;
using System.Collections.Specialized;

using static Hprose.IO.Tags;

namespace Hprose.IO.Deserializers {
    class StringCollectionDeserializer : Deserializer<StringCollection> {
        public static StringCollection Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            StringCollection collection = new StringCollection();
            reader.AddReference(collection);
            var deserializer = Deserializer<string>.Instance;
            for (int i = 0; i < count; ++i) {
                collection.Add(deserializer.Deserialize(reader));
            }
            stream.ReadByte();
            return collection;
        }
        public override StringCollection Read(Reader reader, int tag) {
            switch (tag) {
                case TagList:
                    return Read(reader);
                case TagEmpty:
                    return new StringCollection();
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
