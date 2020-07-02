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
|  LastModified: Jun 28, 2020                              |
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
            Converter<object, byte[]>.convert = (value) => value switch {
                byte[] bytes => bytes,
                string s => Encoding.UTF8.GetBytes(s),
                StringBuilder sb => Encoding.UTF8.GetBytes(sb.ToString()),
                char[] chars => Encoding.UTF8.GetBytes(chars),
                List<char> charList => Encoding.UTF8.GetBytes(charList.ToArray()),
                List<byte> byteList => byteList.ToArray(),
                Guid guid => guid.ToByteArray(),
                BigInteger bi => bi.ToByteArray(),
                _ => Converter<byte[]>.ConvertFromObject(value),
            };
        }
        internal static void Initialize() { }
    }
}
