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
 * BytesConverter.cs                                      *
 *                                                        *
 * hprose BytesConverter class for C#.                    *
 *                                                        *
 * LastModified: Apr 25, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Hprose.IO.Converters {
    static class BytesConverter {
        private static readonly UTF8Encoding UTF8 = new UTF8Encoding();
        static BytesConverter() {
            Converter<string, byte[]>.convert = (value) => UTF8.GetBytes(value);
            Converter<StringBuilder, byte[]>.convert = (value) => UTF8.GetBytes(value.ToString());
            Converter<char[], byte[]>.convert = (value) => UTF8.GetBytes(value);
            Converter<List<char>, byte[]>.convert = (value) => UTF8.GetBytes(value.ToArray());
            Converter<List<byte>, byte[]>.convert = (value) => value.ToArray();
            Converter<Guid, byte[]>.convert = (value) => value.ToByteArray();
            Converter<BigInteger, byte[]>.convert = (value) => value.ToByteArray();
            Converter<object, byte[]>.convert = (value) => {
                switch (value) {
                    case byte[] bytes:
                        return bytes;
                    case string s:
                        return UTF8.GetBytes(s);
                    case StringBuilder sb:
                        return UTF8.GetBytes(sb.ToString());
                    case char[] chars:
                        return UTF8.GetBytes(chars);
                    case List<char> charList:
                        return UTF8.GetBytes(charList.ToArray());
                    case List<byte> byteList:
                        return byteList.ToArray();
                    case Guid guid:
                        return guid.ToByteArray();
                    case BigInteger bi:
                        return bi.ToByteArray();
                    default:
                        return Converter<byte[]>.ConvertFromObject(value);
                }
            };
        }
        internal static void Initialize() { }
    }
}
