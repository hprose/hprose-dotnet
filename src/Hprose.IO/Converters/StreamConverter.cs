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
|  LastModified: May 2, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hprose.IO.Converters {
    internal static class StreamConverter {
        static StreamConverter() {
            Converter<string, Stream>.convert = Converter<string, MemoryStream>.convert;
            Converter<StringBuilder, Stream>.convert = Converter<StringBuilder, MemoryStream>.convert;
            Converter<char[], Stream>.convert = Converter<char[], MemoryStream>.convert;
            Converter<List<char>, Stream>.convert = Converter<List<char>, MemoryStream>.convert;
            Converter<List<byte>, Stream>.convert = Converter<List<byte>, MemoryStream>.convert;
            Converter<object, Stream>.convert = (value) => {
                switch (value) {
                    case Stream stream:
                        return stream;
                    default:
                        return Converter<object, MemoryStream>.convert(value);
                }
            };
        }
        internal static void Initialize() { }
    }
}
