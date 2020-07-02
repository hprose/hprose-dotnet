/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  CharsConverter.cs                                       |
|                                                          |
|  hprose CharsConverter class for C#.                     |
|                                                          |
|  LastModified: Jun 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.Text;

namespace Hprose.IO.Converters {
    internal static class CharsConverter {
        internal static char[] Convert<TInput>(TInput value) => value.ToString().ToCharArray();
        static CharsConverter() {
            Converter<string, char[]>.convert = (value) => value.ToCharArray();
            Converter<StringBuilder, char[]>.convert = (value) => {
                char[] result = new char[value.Length];
#if !NET35_CF
                value.CopyTo(0, result, 0, value.Length);
#else
                value.ToString().CopyTo(0, result, 0, value.Length);
#endif
                return result;
            };
            Converter<byte[], char[]>.convert = (value) => {
                try {
                    return Encoding.UTF8.GetChars(value);
                }
                catch (Exception) {
                    return Encoding.Default.GetChars(value);
                }
            };
            Converter<List<char>, char[]>.convert = (value) => value.ToArray();
            Converter<List<byte>, char[]>.convert = (value) => Converter<byte[], char[]>.convert(value.ToArray());
            Converter<object, char[]>.convert = (value) => value switch {
                char[] chars => chars,
                string s => s.ToCharArray(),
                StringBuilder sb => Converter<StringBuilder, char[]>.convert(sb),
                List<char> charList => charList.ToArray(),
                List<byte> byteList => Converter<List<byte>, char[]>.convert(byteList),
                byte[] bytes => Converter<byte[], char[]>.convert(bytes),
                _ => value.ToString().ToCharArray(),
            };
        }
        internal static void Initialize() { }
    }
}
