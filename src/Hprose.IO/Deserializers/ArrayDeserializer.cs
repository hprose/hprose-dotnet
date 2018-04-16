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
 * ArrayDeserializer.cs                                   *
 *                                                        *
 * ArrayDeserializer class for C#.                        *
 *                                                        *
 * LastModified: Apr 15, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class ArrayDeserializer<T> : Deserializer<T[]> {
        private static readonly T[] EmptyArray = new T[0] { };
        public override T[] Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagEmpty:
                    return EmptyArray;
                case TagList:
                    return ReferenceReader.ReadArray<T>(reader);
                default:
                    return base.Read(reader, tag);
            }
        }
    }
    class Array2Deserializer<T> : Deserializer<T[,]> {
        private static readonly T[,] EmptyArray = new T[0, 0] { };
        public override T[,] Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagEmpty:
                    return EmptyArray;
                case TagList:
                    return ReferenceReader.ReadArray2<T>(reader);
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
