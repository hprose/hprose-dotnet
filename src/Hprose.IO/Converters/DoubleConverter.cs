/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  DoubleConverter.cs                                      |
|                                                          |
|  hprose DoubleConverter class for C#.                    |
|                                                          |
|  LastModified: Apr 21, 2018                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Numerics;

namespace Hprose.IO.Converters {
    internal static class DoubleConverter {
        static DoubleConverter() {
            Converter<bool, double>.convert = Convert.ToDouble;
            Converter<char, double>.convert = Convert.ToDouble;
            Converter<byte, double>.convert = Convert.ToDouble;
            Converter<sbyte, double>.convert = Convert.ToDouble;
            Converter<short, double>.convert = Convert.ToDouble;
            Converter<ushort, double>.convert = Convert.ToDouble;
            Converter<int, double>.convert = Convert.ToDouble;
            Converter<uint, double>.convert = Convert.ToDouble;
            Converter<long, double>.convert = Convert.ToDouble;
            Converter<ulong, double>.convert = Convert.ToDouble;
            Converter<float, double>.convert = Convert.ToDouble;
            Converter<decimal, double>.convert = Convert.ToDouble;
            Converter<DateTime, double>.convert = Convert.ToDouble;
            Converter<BigInteger, double>.convert = (value) => (double)value;
        }
        internal static void Initialize() { }
    }
}
