/**********************************************************\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: http://www.hprose.com/                 |
|                   http://www.hprose.org/                 |
|                                                          |
\**********************************************************/
/**********************************************************\
 *                                                        *
 * SByteConverter.cs                                      *
 *                                                        *
 * hprose SByteConverter class for C#.                    *
 *                                                        *
 * LastModified: Apr 21, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Numerics;

namespace Hprose.IO.Converters {
    static class SByteConverter {
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
            Converter<DateTime, sbyte>.convert = Convert.ToSByte;
            Converter<BigInteger, sbyte>.convert = (value) => (sbyte)value;
        }
        internal static void Initialize() { }
    }
}
