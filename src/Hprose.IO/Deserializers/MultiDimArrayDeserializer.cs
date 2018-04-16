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
 * MultiDimArrayDeserializer.cs                           *
 *                                                        *
 * MultiDimArrayDeserializer class for C#.                *
 *                                                        *
 * LastModified: Apr 15, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class MultiDimArrayDeserializer<T, E> : Deserializer<T> {
        private static readonly T EmptyArray = (T)(object)(Array.CreateInstance(typeof(E), new int[typeof(T).GetArrayRank()]));
        public override T Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagEmpty:
                    return EmptyArray;
                case TagList:
                    return ReferenceReader.ReadMultiDimArray<T, E>(reader);
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
