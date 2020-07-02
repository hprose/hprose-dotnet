/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  GuidConverter.cs                                        |
|                                                          |
|  hprose GuidConverter class for C#.                      |
|                                                          |
|  LastModified: Jun 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Text;

namespace Hprose.IO.Converters {
    internal static class GuidConverter {
        static GuidConverter() {
            Converter<byte[], Guid>.convert = (value) => new Guid(value);
            Converter<string, Guid>.convert = (value) => new Guid(value);
            Converter<StringBuilder, Guid>.convert = (value) => new Guid(value.ToString());
            Converter<char[], Guid>.convert = (value) => new Guid(new string(value));
            Converter<object, Guid>.convert = (value) => value switch {
                Guid guid => guid,
                byte[] bytes => new Guid(bytes),
                string s => new Guid(s),
                char[] chars => new Guid(new string(chars)),
                StringBuilder sb => new Guid(sb.ToString()),
                _ => Converter<Guid>.ConvertFromObject(value),
            };
        }
        internal static void Initialize() { }
    }
}
