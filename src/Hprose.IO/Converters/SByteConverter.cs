/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  SByteConverter.cs                                       |
|                                                          |
|  hprose SByteConverter class for C#.                     |
|                                                          |
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Numerics;

namespace Hprose.IO.Converters {
    internal static class SByteConverter {
        static SByteConverter() {
            Converter<bool, sbyte>.convert = Convert.ToSByte;
            Converter<char, sbyte>.convert = Convert.ToSByte;
            Converter<byte, sbyte>.convert = Convert.ToSByte;
            Converter<short, sbyte>.convert = Convert.ToSByte;
            Converter<ushort, sbyte>.convert = Convert.ToSByte;
            Converter<int, sbyte>.convert = Convert.ToSByte;
            Converter<uint, sbyte>.convert = Convert.ToSByte;
            Converter<long, sbyte>.convert = Convert.ToSByte;
            Converter<ulong, sbyte>.convert = Convert.ToSByte;
            Converter<float, sbyte>.convert = Convert.ToSByte;
            Converter<double, sbyte>.convert = Convert.ToSByte;
            Converter<decimal, sbyte>.convert = Convert.ToSByte;
#if !NET35_CF
            Converter<DateTime, sbyte>.convert = Convert.ToSByte;
#endif
            Converter<BigInteger, sbyte>.convert = (value) => (sbyte)value;
        }
        internal static void Initialize() { }
    }
}
