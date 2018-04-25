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
 * BitArrayDeserializer.cs                                *
 *                                                        *
 * BitArrayDeserializer class for C#.                     *
 *                                                        *
 * LastModified: Apr 22, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.IO;
using System.Collections;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class BitArrayDeserializer : Deserializer<BitArray> {
        public static BitArray Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            BitArray array = new BitArray(count);
            reader.SetRef(array);
            var deserializer = Deserializer<bool>.Instance;
            for (int i = 0; i < count; ++i) {
                array[i] = deserializer.Deserialize(reader);
            }
            stream.ReadByte();
            return array;
        }
        public override BitArray Read(Reader reader, int tag) {
            switch (tag) {
                case TagEmpty:
                    return new BitArray(0);
                case TagList:
                    return Read(reader);
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}