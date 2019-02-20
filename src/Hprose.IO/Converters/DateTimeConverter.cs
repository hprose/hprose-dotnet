/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  DateTimeConverter.cs                                    |
|                                                          |
|  hprose DateTimeConverter class for C#.                  |
|                                                          |
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Text;

namespace Hprose.IO.Converters {
    internal static class DateTimeConverter {
        static DateTimeConverter() {
#if !NET35_CF
            Converter<bool, DateTime>.convert = Convert.ToDateTime;
            Converter<char, DateTime>.convert = Convert.ToDateTime;
            Converter<byte, DateTime>.convert = Convert.ToDateTime;
            Converter<sbyte, DateTime>.convert = Convert.ToDateTime;
            Converter<short, DateTime>.convert = Convert.ToDateTime;
            Converter<ushort, DateTime>.convert = Convert.ToDateTime;
            Converter<int, DateTime>.convert = Convert.ToDateTime;
            Converter<uint, DateTime>.convert = Convert.ToDateTime;
            Converter<ulong, DateTime>.convert = Convert.ToDateTime;
            Converter<float, DateTime>.convert = Convert.ToDateTime;
            Converter<double, DateTime>.convert = Convert.ToDateTime;
            Converter<decimal, DateTime>.convert = Convert.ToDateTime;
#endif
            Converter<long, DateTime>.convert = (value) => new DateTime(value);
            Converter<string, DateTime>.convert = (value) => DateTime.Parse(value);
            Converter<StringBuilder, DateTime>.convert = (value) => DateTime.Parse(value.ToString());
            Converter<char[], DateTime>.convert = (value) => DateTime.Parse(new string(value));
            Converter<object, DateTime>.convert = (value) => {
                switch (value) {
                    case DateTime dt:
                        return dt;
                    case string s:
                        return DateTime.Parse(s);
                    case char[] chars:
                        return DateTime.Parse(new string(chars));
                    case StringBuilder sb:
                        return DateTime.Parse(sb.ToString());
                    case long l:
                        return new DateTime(l);
                    default:
                        return Converter<DateTime>.ConvertFromObject(value);
                }
            };
        }
        internal static void Initialize() { }
    }
}
