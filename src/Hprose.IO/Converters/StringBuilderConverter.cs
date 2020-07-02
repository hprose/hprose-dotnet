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
|  LastModified: Jun 28, 2020                              |
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
            Converter<List<byte>, StringBuilder>.convert = (value) => Converter<byte[], StringBuilder>.convert(value.ToArray());
            Converter<object, StringBuilder>.convert = (value) => value switch {
                StringBuilder sb => sb,
                string s => new StringBuilder(s),
                char[] chars => new StringBuilder(chars.Length).Append(chars),
                List<char> charList => new StringBuilder(charList.Count).Append(charList.ToArray()),
                List<byte> byteList => Converter<List<byte>, StringBuilder>.convert(byteList),
                byte[] bytes => Converter<byte[], StringBuilder>.convert(bytes),
                _ => new StringBuilder(value.ToString()),
            };
        }
        internal static void Initialize() { }
    }
}
