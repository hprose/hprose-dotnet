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
 * DictionaryDeserializer.cs                              *
 *                                                        *
 * DictionaryDeserializer class for C#.                   *
 *                                                        *
 * LastModified: Apr 18, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.IO;
using System.Collections.Generic;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class DictionaryDeserializer<I, T, K, V> : Deserializer<I> where T : I, ICollection<KeyValuePair<K, V>>, new() {
        public static I Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T dict = new T();
            reader.SetRef(dict);
            var keyDeserializer = Deserializer<K>.Instance;
            var valueDeserializer = Deserializer<V>.Instance;
            for (int i = 0; i < count; ++i) {
                var k = keyDeserializer.Deserialize(reader);
                var v = valueDeserializer.Deserialize(reader);
                dict.Add(new KeyValuePair<K, V>(k, v));
            }
            stream.ReadByte();
            return dict;
        }
        public override I Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagEmpty:
                    return new T();
                case TagMap:
                    return Read(reader);
                default:
                    return base.Read(reader, tag);
            }
        }
    }
    class DictionaryDeserializer<T, K, V> : DictionaryDeserializer<T, T, K, V> where T : ICollection<KeyValuePair<K, V>>, new() { }
}
