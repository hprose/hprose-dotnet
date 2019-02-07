/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  UInt64Converter.cs                                      |
|                                                          |
|  hprose UInt64Converter class for C#.                    |
|                                                          |
|  LastModified: Apr 21, 2018                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Numerics;

namespace Hprose.IO.Converters {
    internal static class UInt64Converter {
        static UInt64Converter() {
            Converter<bool, ulong>.convert = Convert.ToUInt64;
            Converter<char, ulong>.convert = Convert.ToUInt64;
            Converter<byte, ulong>.convert = Convert.ToUInt64;
            Converter<sbyte, ulong>.convert = Convert.ToUInt64;
            Converter<short, ulong>.convert = Convert.ToUInt64;
            Converter<ushort, ulong>.convert = Convert.ToUInt64;
            Converter<int, ulong>.convert = Convert.ToUInt64;
            Converter<uint, ulong>.convert = Convert.ToUInt64;
            Converter<long, ulong>.convert = Convert.ToUInt64;
            Converter<float, ulong>.convert = Convert.ToUInt64;
            Converter<double, ulong>.convert = Convert.ToUInt64;
            Converter<decimal, ulong>.convert = Convert.ToUInt64;
            Converter<DateTime, ulong>.convert = Convert.ToUInt64;
            Converter<BigInteger, ulong>.convert = (value) => (ulong)value;
        }
        internal static void Initialize() { }
    }
}
