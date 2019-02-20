/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  StringBuilderConverter.cs                               |
|                                                          |
|  hprose StringBuilderConverter class for C#.             |
|                                                          |
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.Text;

namespace Hprose.IO.Converters {
    internal static class StringBuilderConverter {
        internal static StringBuilder Convert<TInput>(TInput value) => new StringBuilder(value.ToString());
        static StringBuilderConverter() {
            Converter<string, StringBuilder>.convert = (value) => new StringBuilder(value);
            Converter<char[], StringBuilder>.convert = (value) => new StringBuilder(value.Length).Append(value);
            Converter<byte[], StringBuilder>.convert = (value) => {
                try {
                    return new StringBuilder(Encoding.UTF8.GetString(value, 0, value.Length));
                }
                catch (Exception) {
                    return new StringBuilder(Encoding.Default.GetString(value, 0, value.Length));
                }
            };
            Converter<List<char>, StringBuilder>.convert = (value) => new StringBuilder(value.Count).Append(value.ToArray());
            Converter<List<byte>, StringBuilder>.convert = (value) => {
                var bytes = value.ToArray();
                try {
                    return new StringBuilder(Encoding.UTF8.GetString(bytes, 0, bytes.Length));
                }
                catch (Exception) {
                    return new StringBuilder(Encoding.Default.GetString(bytes, 0, bytes.Length));
                }
            };
            Converter<object, StringBuilder>.convert = (value) => {
                switch (value) {
                    case StringBuilder sb:
                        return sb;
                    case string s:
                        return new StringBuilder(s);
                    case char[] chars:
                        return new StringBuilder(chars.Length).Append(chars);
                    case List<char> charList:
                        return new StringBuilder(charList.Count).Append(charList.ToArray());
                    case List<byte> byteList:
                        var byteArray = byteList.ToArray();
                        try {
                            return new StringBuilder(Encoding.UTF8.GetString(byteArray, 0, byteArray.Length));
                        }
                        catch (Exception) {
                            return new StringBuilder(Encoding.Default.GetString(byteArray, 0, byteArray.Length));
                        }
                    case byte[] bytes:
                        try {
                            return new StringBuilder(Encoding.UTF8.GetString(bytes, 0, bytes.Length));
                        }
                        catch (Exception) {
                            return new StringBuilder(Encoding.Default.GetString(bytes, 0, bytes.Length));
                        }
                    default:
                        return new StringBuilder(value.ToString());
                }
            };
        }
        internal static void Initialize() { }
    }
}
