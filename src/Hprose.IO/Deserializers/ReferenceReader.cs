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
 * ReferenceReader.cs                                     *
 *                                                        *
 * ReferenceReader class for C#.                          *
 *                                                        *
 * LastModified: Apr 15, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.IO;
using System.Numerics;
using System.Text;

using static System.DateTimeKind;
using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    public static class ReferenceReader {
        public static byte[] ReadBytes(Reader reader) {
            var result = ValueReader.ReadBytes(reader.Stream);
            reader.SetRef(result);
            return result;
        }
        public static char[] ReadChars(Reader reader) {
            var result = ValueReader.ReadChars(reader.Stream);
            reader.SetRef(result);
            return result;
        }
        public static string ReadString(Reader reader) {
            var result = ValueReader.ReadString(reader.Stream);
            reader.SetRef(result);
            return result;
        }
        public static Guid ReadGuid(Reader reader) {
            var result = ValueReader.ReadGuid(reader.Stream);
            reader.SetRef(result);
            return result;
        }
        public static DateTime ReadDateTime(Reader reader) {
            var result = ValueReader.ReadDateTime(reader.Stream);
            reader.SetRef(result);
            return result;
        }
        public static DateTime ReadTime(Reader reader) {
            var result = ValueReader.ReadTime(reader.Stream);
            reader.SetRef(result);
            return result;
        }
        public static T[] ReadArray<T>(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T[] a = new T[count];
            reader.SetRef(a);
            var deserializer = Deserializer<T>.Instance;
            for (int i = 0; i < count; ++i) {
                a[i] = deserializer.Deserialize(reader);
            }
            stream.ReadByte();
            return a;
        }
        public static T[,] ReadArray2<T>(Reader reader) {
            Stream stream = reader.Stream;
            int count1 = ValueReader.ReadCount(stream);
            if (stream.ReadByte() != TagList) {
                throw new RankException();
            }
            int count2 = ValueReader.ReadCount(stream);
            T[,] a = new T[count1, count2];
            reader.SetRef(a);
            reader.SetRef(null);
            var deserializer = Deserializer<T>.Instance;
            for (int i = 0; i < count1; ++i) {
                for (int j = 0; j < count2; ++j) {
                    a[i,j] = deserializer.Deserialize(reader);
                }
                stream.ReadByte();
                if (i < count1 - 1) {
                    if (stream.ReadByte() != TagList) {
                        throw new RankException();
                    }
                    ValueReader.SkipUntil(stream, TagOpenbrace);
                    reader.SetRef(null);
                }
            }
            stream.ReadByte();
            return a;
        }
    }
}
