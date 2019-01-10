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
 * LastModified: Jan 11, 2019                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    class MultiDimArrayDeserializer<T, E> : Deserializer<T> {
        private static readonly T empty = (T)(object)(Array.CreateInstance(typeof(E), new int[typeof(T).GetArrayRank()]));
        private static T Read(Reader reader) {
            Stream stream = reader.Stream;
            Type type = typeof(T);
            int rank = type.GetArrayRank();
            int[] loc = new int[rank];
            int[] len = new int[rank];
            len[0] = ValueReader.ReadCount(stream);
            for (int i = 1; i < rank; ++i) {
                stream.ReadByte();
                len[i] = ValueReader.ReadCount(stream);
            }
            var a = Array.CreateInstance(typeof(E), len);
            reader.AddReference(a);
            for (int i = 1; i < rank; ++i) {
                reader.AddReference(null);
            }
            int maxrank = rank - 1;
            var deserializer = Deserializer<E>.Instance;
            while (true) {
                for (loc[maxrank] = 0;
                     loc[maxrank] < len[maxrank];
                     loc[maxrank]++) {
                    a.SetValue(deserializer.Deserialize(reader), loc);
                }
                for (int i = maxrank; i > 0; i--) {
                    if (loc[i] >= len[i]) {
                        loc[i] = 0;
                        loc[i - 1]++;
                        stream.ReadByte();
                    }
                }
                if (loc[0] >= len[0]) {
                    break;
                }
                int n = 0;
                for (int i = maxrank; i > 0; i--) {
                    if (loc[i] == 0) {
                        n++;
                    }
                    else {
                        break;
                    }
                }
                for (int i = rank - n; i < rank; ++i) {
                    stream.ReadByte();
                    reader.AddReference(null);
                    ValueReader.SkipUntil(stream, TagOpenbrace);
                }
            }
            stream.ReadByte();
            return (T)(object)a;
        }
        public override T Read(Reader reader, int tag) {
            switch (tag) {
                case TagList:
                    return Read(reader);
                case TagEmpty:
                    return empty;
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
