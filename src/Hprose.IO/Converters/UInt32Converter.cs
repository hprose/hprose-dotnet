/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  UInt32Converter.cs                                      |
|                                                          |
|  hprose UInt32Converter class for C#.                    |
|                                                          |
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Numerics;

namespace Hprose.IO.Converters {
    internal static class UInt32Converter {
        static UInt32Converter() {
            Converter<bool, uint>.convert = Convert.ToUInt32;
            Converter<char, uint>.convert = Convert.ToUInt32;
            Converter<byte, uint>.convert = Convert.ToUInt32;
            Converter<sbyte, uint>.convert = Convert.ToUInt32;
            Converter<short, uint>.convert = Convert.ToUInt32;
            Converter<ushort, uint>.convert = Convert.ToUInt32;
            Converter<int, uint>.convert = Convert.ToUInt32;
            Converter<long, uint>.convert = Convert.ToUInt32;
            Converter<ulong, uint>.convert = Convert.ToUInt32;
            Converter<float, uint>.convert = Convert.ToUInt32;
            Converter<double, uint>.convert = Convert.ToUInt32;
            Converter<decimal, uint>.convert = Convert.ToUInt32;
#if !NET35_CF
            Converter<DateTime, uint>.convert = Convert.ToUInt32;
#endif
            Converter<BigInteger, uint>.convert = (value) => (uint)value;
        }
        internal static void Initialize() { }
    }
}
