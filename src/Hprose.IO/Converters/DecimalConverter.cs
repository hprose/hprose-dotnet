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
 * DecimalConverter.cs                                    *
 *                                                        *
 * hprose DecimalConverter class for C#.                  *
 *                                                        *
 * LastModified: Apr 21, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Numerics;

namespace Hprose.IO.Converters {
    static class DecimalConverter {
        static DecimalConverter() {
            Converter<bool, decimal>.convert = Convert.ToDecimal;
            Converter<char, decimal>.convert = Convert.ToDecimal;
            Converter<byte, decimal>.convert = Convert.ToDecimal;
            Converter<sbyte, decimal>.convert = Convert.ToDecimal;
            Converter<short, decimal>.convert = Convert.ToDecimal;
            Converter<ushort, decimal>.convert = Convert.ToDecimal;
            Converter<int, decimal>.convert = Convert.ToDecimal;
            Converter<uint, decimal>.convert = Convert.ToDecimal;
            Converter<long, decimal>.convert = Convert.ToDecimal;
            Converter<ulong, decimal>.convert = Convert.ToDecimal;
            Converter<float, decimal>.convert = Convert.ToDecimal;
            Converter<double, decimal>.convert = Convert.ToDecimal;
            Converter<DateTime, decimal>.convert = Convert.ToDecimal;
            Converter<BigInteger, decimal>.convert = (value) => (decimal)value;
        }
        internal static void Initialize() { }
    }
}
