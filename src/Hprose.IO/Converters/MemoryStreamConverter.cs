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
    static class MemoryStreamConverter {
        private static readonly UTF8Encoding UTF8 = new UTF8Encoding();
        static MemoryStreamConverter() {
            Converter<string, MemoryStream>.convert = (value) => new MemoryStream(UTF8.GetBytes(value));
            Converter<StringBuilder, MemoryStream>.convert = (value) => new MemoryStream(UTF8.GetBytes(value.ToString()));
            Converter<char[], MemoryStream>.convert = (value) => new MemoryStream(UTF8.GetBytes(value));
            Converter<List<char>, MemoryStream>.convert = (value) => new MemoryStream(UTF8.GetBytes(value.ToArray()));
            Converter<List<byte>, MemoryStream>.convert = (value) => new MemoryStream(value.ToArray());
            Converter<object, MemoryStream>.convert = (value) => {
                switch (value) {
                    case MemoryStream stream:
                        return stream;
                    case string s:
                        return new MemoryStream(UTF8.GetBytes(s));
                    case StringBuilder sb:
                        return new MemoryStream(UTF8.GetBytes(sb.ToString()));
                    case char[] chars:
                        return new MemoryStream(UTF8.GetBytes(chars));
                    case List<char> charList:
                        return new MemoryStream(UTF8.GetBytes(charList.ToArray()));
                    case List<byte> byteList:
                        return new MemoryStream(byteList.ToArray());
                    default:
                        return Converter<MemoryStream>.ConvertFromObject(value);
                }
            };
        }
        internal static void Initialize() { }
    }
}
