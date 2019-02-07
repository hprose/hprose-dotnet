/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  MemoryStreamConverter.cs                                |
|                                                          |
|  hprose MemoryStreamConverter class for C#.              |
|                                                          |
|  LastModified: May 2, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hprose.IO.Converters {
    internal static class MemoryStreamConverter {
        static MemoryStreamConverter() {
            Converter<string, MemoryStream>.convert = (value) => {
                var bytes = Encoding.UTF8.GetBytes(value);
                return new MemoryStream(bytes, 0, bytes.Length, false, true);
            };
            Converter<StringBuilder, MemoryStream>.convert = (value) => {
                var bytes = Encoding.UTF8.GetBytes(value.ToString());
                return new MemoryStream(bytes, 0, bytes.Length, false, true);
            };
            Converter<char[], MemoryStream>.convert = (value) => {
                var bytes = Encoding.UTF8.GetBytes(value);
                return new MemoryStream(bytes, 0, bytes.Length, false, true);
            };
            Converter<List<char>, MemoryStream>.convert = (value) => {
                var bytes = Encoding.UTF8.GetBytes(value.ToArray());
                return new MemoryStream(bytes, 0, bytes.Length, false, true);
            };
            Converter<List<byte>, MemoryStream>.convert = (value) => {
                var bytes = value.ToArray();
                return new MemoryStream(bytes, 0, bytes.Length, false, true);
            };
            Converter<object, MemoryStream>.convert = (value) => {
                switch (value) {
                    case MemoryStream stream:
                        return stream;
                    case string s: {
                            var bytes = Encoding.UTF8.GetBytes(s);
                            return new MemoryStream(bytes, 0, bytes.Length, false, true);
                        }
                    case StringBuilder sb: {
                            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
                            return new MemoryStream(bytes, 0, bytes.Length, false, true);
                        }
                    case char[] chars: {
                            var bytes = Encoding.UTF8.GetBytes(chars);
                            return new MemoryStream(bytes, 0, bytes.Length, false, true);
                        }
                    case List<char> charList: {
                            var bytes = Encoding.UTF8.GetBytes(charList.ToArray());
                            return new MemoryStream(bytes, 0, bytes.Length, false, true);
                        }
                    case List<byte> byteList: {
                            var bytes = byteList.ToArray();
                            return new MemoryStream(bytes, 0, bytes.Length, false, true);
                        }
                    default:
                        return Converter<MemoryStream>.ConvertFromObject(value);
                }
            };
        }
        internal static void Initialize() { }
    }
}
