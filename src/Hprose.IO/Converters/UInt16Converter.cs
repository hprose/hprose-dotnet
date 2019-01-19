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
|  LastModified: Apr 21, 2018                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Numerics;

namespace Hprose.IO.Converters {
    static class UInt16Converter {
        static UInt16Converter() {
            Converter<bool, ushort>.convert = Convert.ToUInt16;
            Converter<char, ushort>.convert = Convert.ToUInt16;
            Converter<byte, ushort>.convert = Convert.ToUInt16;
            Converter<sbyte, ushort>.convert = Convert.ToUInt16;
            Converter<short, ushort>.convert = Convert.ToUInt16;
            Converter<int, ushort>.convert = Convert.ToUInt16;
            Converter<uint, ushort>.convert = Convert.ToUInt16;
            Converter<long, ushort>.convert = Convert.ToUInt16;
            Converter<ulong, ushort>.convert = Convert.ToUInt16;
            Converter<float, ushort>.convert = Convert.ToUInt16;
            Converter<double, ushort>.convert = Convert.ToUInt16;
            Converter<decimal, ushort>.convert = Convert.ToUInt16;
            Converter<DateTime, ushort>.convert = Convert.ToUInt16;
            Converter<BigInteger, ushort>.convert = (value) => (ushort)value;
        }
        internal static void Initialize() { }
    }
}
