/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  BytesConverter.cs                                       |
|                                                          |
|  hprose BytesConverter class for C#.                     |
|                                                          |
|  LastModified: Feb 8, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Hprose.IO.Converters {
    internal static class BytesConverter {
        static BytesConverter() {
            Converter<string, byte[]>.convert = (value) => Encoding.UTF8.GetBytes(value);
            Converter<StringBuilder, byte[]>.convert = (value) => Encoding.UTF8.GetBytes(value.ToString());
            Converter<char[], byte[]>.convert = (value) => Encoding.UTF8.GetBytes(value);
            Converter<List<char>, byte[]>.convert = (value) => Encoding.UTF8.GetBytes(value.ToArray());
            Converter<List<byte>, byte[]>.convert = (value) => value.ToArray();
            Converter<Guid, byte[]>.convert = (value) => value.ToByteArray();
            Converter<BigInteger, byte[]>.convert = (value) => value.ToByteArray();
            Converter<object, byte[]>.convert = (value) => {
                switch (value) {
                    case byte[] bytes:
                        return bytes;
                    case string s:
                        return Encoding.UTF8.GetBytes(s);
                    case StringBuilder sb:
                        return Encoding.UTF8.GetBytes(sb.ToString());
                    case char[] chars:
                        return Encoding.UTF8.GetBytes(chars);
                    case List<char> charList:
                        return Encoding.UTF8.GetBytes(charList.ToArray());
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
