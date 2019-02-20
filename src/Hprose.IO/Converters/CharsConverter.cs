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
|  LastModified: Feb 21, 2019                              |
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
            Converter<List<byte>, char[]>.convert = (value) => {
                var bytes = value.ToArray();
                try {
                    return Encoding.UTF8.GetChars(bytes);
                }
                catch (Exception) {
                    return Encoding.Default.GetChars(bytes);
                }
            };
            Converter<object, char[]>.convert = (value) => {
                switch (value) {
                    case char[] chars:
                        return chars;
                    case string s:
                        return s.ToCharArray();
                    case StringBuilder sb:
                        char[] result = new char[sb.Length];
#if !NET35_CF
                        sb.CopyTo(0, result, 0, sb.Length);
#else
                        sb.ToString().CopyTo(0, result, 0, sb.Length);
#endif
                        return result;
                    case List<char> charList:
                        return charList.ToArray();
                    case List<byte> byteList:
                        var byteArray = byteList.ToArray();
                        try {
                            return Encoding.UTF8.GetChars(byteArray);
                        }
                        catch (Exception) {
                            return Encoding.Default.GetChars(byteArray);
                        }
                    case byte[] bytes:
                        try {
                            return Encoding.UTF8.GetChars(bytes);
                        }
                        catch (Exception) {
                            return Encoding.Default.GetChars(bytes);
                        }
                    default:
                        return value.ToString().ToCharArray();
                }
            };
        }
        internal static void Initialize() { }
    }
}
