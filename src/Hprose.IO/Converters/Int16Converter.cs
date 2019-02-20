/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Int16Converter.cs                                       |
|                                                          |
|  hprose Int16Converter class for C#.                     |
|                                                          |
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Numerics;

namespace Hprose.IO.Converters {
    internal static class Int16Converter {
        static Int16Converter() {
            Converter<bool, short>.convert = Convert.ToInt16;
            Converter<char, short>.convert = Convert.ToInt16;
            Converter<byte, short>.convert = Convert.ToInt16;
            Converter<sbyte, short>.convert = Convert.ToInt16;
            Converter<ushort, short>.convert = Convert.ToInt16;
            Converter<int, short>.convert = Convert.ToInt16;
            Converter<uint, short>.convert = Convert.ToInt16;
            Converter<long, short>.convert = Convert.ToInt16;
            Converter<ulong, short>.convert = Convert.ToInt16;
            Converter<float, short>.convert = Convert.ToInt16;
            Converter<double, short>.convert = Convert.ToInt16;
            Converter<decimal, short>.convert = Convert.ToInt16;
#if !NET35_CF
            Converter<DateTime, short>.convert = Convert.ToInt16;
#endif
            Converter<BigInteger, short>.convert = (value) => (short)value;
        }
        internal static void Initialize() { }
    }
}
