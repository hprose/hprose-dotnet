/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ByteConverter.cs                                        |
|                                                          |
|  hprose ByteConverter class for C#.                      |
|                                                          |
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Numerics;

namespace Hprose.IO.Converters {
    internal static class ByteConverter {
        static ByteConverter() {
            Converter<bool, byte>.convert = Convert.ToByte;
            Converter<char, byte>.convert = Convert.ToByte;
            Converter<sbyte, byte>.convert = Convert.ToByte;
            Converter<short, byte>.convert = Convert.ToByte;
            Converter<ushort, byte>.convert = Convert.ToByte;
            Converter<int, byte>.convert = Convert.ToByte;
            Converter<uint, byte>.convert = Convert.ToByte;
            Converter<long, byte>.convert = Convert.ToByte;
            Converter<ulong, byte>.convert = Convert.ToByte;
            Converter<float, byte>.convert = Convert.ToByte;
            Converter<double, byte>.convert = Convert.ToByte;
            Converter<decimal, byte>.convert = Convert.ToByte;
#if !NET35_CF
            Converter<DateTime, byte>.convert = Convert.ToByte;
#endif
            Converter<BigInteger, byte>.convert = (value) => (byte)value;
        }
        internal static void Initialize() { }
    }
}
