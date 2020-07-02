/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  TimeSpanConverter.cs                                    |
|                                                          |
|  hprose TimeSpanConverter class for C#.                  |
|                                                          |
|  LastModified: Jun 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Text;

namespace Hprose.IO.Converters {
    internal static class TimeSpanConverter {
        static TimeSpanConverter() {
            Converter<long, TimeSpan>.convert = (value) => new TimeSpan(value);
            Converter<DateTime, TimeSpan>.convert = (value) => new TimeSpan(value.Ticks);
            Converter<string, TimeSpan>.convert = (value) => TimeSpan.Parse(value);
            Converter<StringBuilder, TimeSpan>.convert = (value) => TimeSpan.Parse(value.ToString());
            Converter<char[], TimeSpan>.convert = (value) => TimeSpan.Parse(new string(value));
            Converter<object, TimeSpan>.convert = (value) => value switch {
                TimeSpan ts => ts,
                DateTime dt => new TimeSpan(dt.Ticks),
                string s => TimeSpan.Parse(s),
                char[] chars => TimeSpan.Parse(new string(chars)),
                StringBuilder sb => TimeSpan.Parse(sb.ToString()),
                long l => new TimeSpan(l),
                _ => Converter<TimeSpan>.ConvertFromObject(value),
            };
        }
        internal static void Initialize() { }
    }
}
