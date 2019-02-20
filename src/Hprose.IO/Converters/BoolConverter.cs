/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  BoolConverter.cs                                        |
|                                                          |
|  hprose BoolConverter class for C#.                      |
|                                                          |
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Numerics;

namespace Hprose.IO.Converters {
    internal static class BoolConverter {
        static BoolConverter() {
            Converter<char, bool>.convert = Convert.ToBoolean;
            Converter<sbyte, bool>.convert = Convert.ToBoolean;
            Converter<byte, bool>.convert = Convert.ToBoolean;
            Converter<short, bool>.convert = Convert.ToBoolean;
            Converter<ushort, bool>.convert = Convert.ToBoolean;
            Converter<int, bool>.convert = Convert.ToBoolean;
            Converter<uint, bool>.convert = Convert.ToBoolean;
            Converter<long, bool>.convert = Convert.ToBoolean;
            Converter<ulong, bool>.convert = Convert.ToBoolean;
            Converter<float, bool>.convert = Convert.ToBoolean;
            Converter<double, bool>.convert = Convert.ToBoolean;
            Converter<decimal, bool>.convert = Convert.ToBoolean;
#if !NET35_CF
            Converter<DateTime, bool>.convert = Convert.ToBoolean;
#endif
            Converter<BigInteger, bool>.convert = (value) => !value.IsZero;
        }
        internal static void Initialize() { }
    }
}
