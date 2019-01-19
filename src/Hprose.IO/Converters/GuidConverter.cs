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
|  LastModified: Apr 21, 2018                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Text;

namespace Hprose.IO.Converters {
    static class GuidConverter {
        static GuidConverter() {
            Converter<byte[], Guid>.convert = (value) => new Guid(value);
            Converter<string, Guid>.convert = (value) => new Guid(value);
            Converter<StringBuilder, Guid>.convert = (value) => new Guid(value.ToString());
            Converter<char[], Guid>.convert = (value) => new Guid(new string(value));
            Converter<object, Guid>.convert = (value) => {
                switch (value) {
                    case Guid guid:
                        return guid;
                    case byte[] bytes:
                        return new Guid(bytes);
                    case string s:
                        return new Guid(s);
                    case char[] chars:
                        return new Guid(new string(chars));
                    case StringBuilder sb:
                        return new Guid(sb.ToString());
                    default:
                        return Converter<Guid>.ConvertFromObject(value);
                }
            };
        }
        internal static void Initialize() { }
    }
}
