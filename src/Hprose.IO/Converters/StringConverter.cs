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
|  LastModified: Feb 21, 2019                              |
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
            Converter<List<byte>, string>.convert = (value) => {
                var bytes = value.ToArray();
                try {
                    return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                }
                catch (Exception) {
                    return Encoding.Default.GetString(bytes, 0, bytes.Length);
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
                            return Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
                        }
                        catch (Exception) {
                            return Encoding.Default.GetString(byteArray, 0, byteArray.Length);
                        }
                    case byte[] bytes:
                        try {
                            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                        }
                        catch (Exception) {
                            return Encoding.Default.GetString(bytes, 0, bytes.Length);
                        }
                    default:
                        return value.ToString();
                }
            };
        }
        internal static void Initialize() { }
    }
}
