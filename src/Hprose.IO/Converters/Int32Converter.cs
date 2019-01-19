/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Int32Converter.cs                                       |
|                                                          |
|  hprose Int32Converter class for C#.                     |
|                                                          |
|  LastModified: Apr 21, 2018                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Numerics;

namespace Hprose.IO.Converters {
    static class Int32Converter {
        static Int32Converter() {
            Converter<bool, int>.convert = Convert.ToInt32;
            Converter<char, int>.convert = Convert.ToInt32;
            Converter<byte, int>.convert = Convert.ToInt32;
            Converter<sbyte, int>.convert = Convert.ToInt32;
            Converter<short, int>.convert = Convert.ToInt32;
            Converter<ushort, int>.convert = Convert.ToInt32;
            Converter<uint, int>.convert = Convert.ToInt32;
            Converter<long, int>.convert = Convert.ToInt32;
            Converter<ulong, int>.convert = Convert.ToInt32;
            Converter<float, int>.convert = Convert.ToInt32;
            Converter<double, int>.convert = Convert.ToInt32;
            Converter<decimal, int>.convert = Convert.ToInt32;
            Converter<DateTime, int>.convert = Convert.ToInt32;
            Converter<BigInteger, int>.convert = (value) => (int)value;
        }
        internal static void Initialize() { }
    }
}
