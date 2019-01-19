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
|  LastModified: Apr 21, 2018                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Text;

namespace Hprose.IO.Converters {
    static class TimeSpanConverter {
        static TimeSpanConverter() {
            Converter<long, TimeSpan>.convert = (value) => new TimeSpan(value);
            Converter<DateTime, TimeSpan>.convert = (value) => new TimeSpan(value.Ticks);
            Converter<string, TimeSpan>.convert = (value) => TimeSpan.Parse(value);
            Converter<StringBuilder, TimeSpan>.convert = (value) => TimeSpan.Parse(value.ToString());
            Converter<char[], TimeSpan>.convert = (value) => TimeSpan.Parse(new string(value));
            Converter<object, TimeSpan>.convert = (value) => {
                switch (value) {
                    case TimeSpan ts:
                        return ts;
                    case DateTime dt:
                        return new TimeSpan(dt.Ticks);
                    case string s:
                        return TimeSpan.Parse(s);
                    case char[] chars:
                        return TimeSpan.Parse(new string(chars));
                    case StringBuilder sb:
                        return TimeSpan.Parse(sb.ToString());
                    case long l:
                        return new TimeSpan(l);
                    default:
                        return Converter<TimeSpan>.ConvertFromObject(value);
                }
            };
        }
        internal static void Initialize() { }
    }
}
