/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  StringConverter.cs                                      |
|                                                          |
|  hprose StringConverter class for C#.                    |
|                                                          |
|  LastModified: Jun 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.Text;

namespace Hprose.IO.Converters {
    internal static class StringConverter {
        internal static string Convert<TInput>(TInput value) => value.ToString();
        static StringConverter() {
            Converter<char[], string>.convert = (value) => new string(value);
            Converter<byte[], string>.convert = (value) => {
                try {
                    return Encoding.UTF8.GetString(value, 0, value.Length);
                }
                catch (Exception) {
                    return Encoding.Default.GetString(value, 0, value.Length);
                }
            };
            Converter<List<char>, string>.convert = (value) => new string(value.ToArray());
            Converter<List<byte>, string>.convert = (value) => Converter<byte[], string>.convert(value.ToArray());
            Converter<object, string>.convert = (value) => value switch {
                string s => s,
                char[] chars => new string(chars),
                StringBuilder sb => sb.ToString(),
                List<char> charList => new string(charList.ToArray()),
                List<byte> byteList => Converter<List<byte>, string>.convert(byteList),
                byte[] bytes => Converter<byte[], string>.convert(bytes),
                _ => value.ToString(),
            };
        }
        internal static void Initialize() { }
    }
}
