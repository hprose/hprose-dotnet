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
 * LastModified: Apr 16, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.IO;

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

    }
}
