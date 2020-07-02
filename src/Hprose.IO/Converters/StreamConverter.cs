/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  StreamConverter.cs                                      |
|                                                          |
|  hprose StreamConverter class for C#.                    |
|                                                          |
|  LastModified: Jun 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hprose.IO.Converters {
    internal static class StreamConverter {
        static StreamConverter() {
            Converter<string, Stream>.convert = (value) => Converter<string, MemoryStream>.convert(value);
            Converter<StringBuilder, Stream>.convert = (value) => Converter<StringBuilder, MemoryStream>.convert(value);
            Converter<char[], Stream>.convert = (value) => Converter<char[], MemoryStream>.convert(value);
            Converter<List<char>, Stream>.convert = (value) => Converter<List<char>, MemoryStream>.convert(value);
            Converter<List<byte>, Stream>.convert = (value) => Converter<List<byte>, MemoryStream>.convert(value);
            Converter<object, Stream>.convert = (value) => value switch {
                Stream stream => stream,
                _ => Converter<object, MemoryStream>.convert(value),
            };
        }
        internal static void Initialize() { }
    }
}
