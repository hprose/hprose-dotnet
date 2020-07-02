/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Int64Converter.cs                                       |
|                                                          |
|  hprose Int64Converter class for C#.                     |
|                                                          |
|  LastModified: Jun 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Numerics;

namespace Hprose.IO.Converters {
    internal static class Int64Converter {
        static Int64Converter() {
            Converter<bool, long>.convert = Convert.ToInt64;
            Converter<char, long>.convert = Convert.ToInt64;
            Converter<byte, long>.convert = Convert.ToInt64;
            Converter<sbyte, long>.convert = Convert.ToInt64;
            Converter<short, long>.convert = Convert.ToInt64;
            Converter<ushort, long>.convert = Convert.ToInt64;
            Converter<int, long>.convert = Convert.ToInt64;
            Converter<uint, long>.convert = Convert.ToInt64;
            Converter<ulong, long>.convert = Convert.ToInt64;
            Converter<float, long>.convert = Convert.ToInt64;
            Converter<double, long>.convert = Convert.ToInt64;
            Converter<decimal, long>.convert = Convert.ToInt64;
            Converter<DateTime, long>.convert = (value) => value.Ticks;
            Converter<TimeSpan, long>.convert = (value) => value.Ticks;
            Converter<BigInteger, long>.convert = (value) => (long)value;
            Converter<object, long>.convert = (value) => value switch {
                long l => l,
                DateTime dt => dt.Ticks,
                TimeSpan ts => ts.Ticks,
                _ => Converter<long>.ConvertFrom(value),
            };
        }
        internal static void Initialize() { }
    }
}
