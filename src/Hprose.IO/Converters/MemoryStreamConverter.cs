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
|  LastModified: Jun 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hprose.IO.Converters {
    internal static class MemoryStreamConverter {
        static MemoryStreamConverter() {
            Converter<byte[], MemoryStream>.convert = (value) => new MemoryStream(value, 0, value.Length, false, true);
            Converter<string, MemoryStream>.convert = (value) => Converter<byte[], MemoryStream>.convert(Encoding.UTF8.GetBytes(value));
            Converter<StringBuilder, MemoryStream>.convert = (value) => Converter<string, MemoryStream>.convert(value.ToString());
            Converter<char[], MemoryStream>.convert = (value) => Converter<byte[], MemoryStream>.convert(Encoding.UTF8.GetBytes(value));
            Converter<List<char>, MemoryStream>.convert = (value) => Converter<char[], MemoryStream>.convert(value.ToArray());
            Converter<List<byte>, MemoryStream>.convert = (value) => Converter<byte[], MemoryStream>.convert(value.ToArray());
            Converter<object, MemoryStream>.convert = (value) => value switch {
                MemoryStream stream => stream,
                byte[] bytes => Converter<byte[], MemoryStream>.convert(bytes),
                string s => Converter<string, MemoryStream>.convert(s),
                StringBuilder sb => Converter<StringBuilder, MemoryStream>.convert(sb),
                char[] chars => Converter<char[], MemoryStream>.convert(chars),
                List<char> charList => Converter<List<char>, MemoryStream>.convert(charList),
                List<byte> byteList => Converter<List<byte>, MemoryStream>.convert(byteList),
                _ => Converter<MemoryStream>.ConvertFromObject(value),
            };
        }
        internal static void Initialize() { }
    }
}
