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
|  LastModified: Apr 21, 2018                              |
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
                    return Encoding.UTF8.GetString(value);
                }
                catch (Exception) {
                    return Encoding.Default.GetString(value);
                }
            };
            Converter<List<char>, string>.convert = (value) => new string(value.ToArray());
            Converter<List<byte>, string>.convert = (value) => {
                var bytes = value.ToArray();
                try {
                    return Encoding.UTF8.GetString(bytes);
                }
                catch (Exception) {
                    return Encoding.Default.GetString(bytes);
                }
            };
            Converter<object, string>.convert = (value) => {
                switch (value) {
                    case string s:
                        return s;
                    case char[] chars:
                        return new string(chars);
                    case StringBuilder sb:
                        return sb.ToString();
                    case List<char> charList:
                        return new string(charList.ToArray());
                    case List<byte> byteList:
                        var byteArray = byteList.ToArray();
                        try {
                            return Encoding.UTF8.GetString(byteArray);
                        }
                        catch (Exception) {
                            return Encoding.Default.GetString(byteArray);
                        }
                    case byte[] bytes:
                        try {
                            return Encoding.UTF8.GetString(bytes);
                        }
                        catch (Exception) {
                            return Encoding.Default.GetString(bytes);
                        }
                    default:
                        return value.ToString();
                }
            };
        }
        internal static void Initialize() { }
    }
}
