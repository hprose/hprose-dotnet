/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
| BigIntegerConverter.cs                                   |
|                                                          |
| hprose BigIntegerConverter class for C#.                 |
|                                                          |
| LastModified: Apr 13, 2018                               |
| Author: Ma Bingyao <andot@hprose.com>                    |
|                                                          |
\*________________________________________________________*/

using System;
using System.Numerics;
using System.Text;

namespace Hprose.IO.Converters {
    static class BigIntegerConverter {
        static BigIntegerConverter() {
            Converter<bool, BigInteger>.convert = (value) => value ? BigInteger.One : BigInteger.Zero;
            Converter<char, BigInteger>.convert = (value) => value;
            Converter<byte, BigInteger>.convert = (value) => value;
            Converter<sbyte, BigInteger>.convert = (value) => value;
            Converter<short, BigInteger>.convert = (value) => value;
            Converter<ushort, BigInteger>.convert = (value) => value;
            Converter<int, BigInteger>.convert = (value) => value;
            Converter<uint, BigInteger>.convert = (value) => value;
            Converter<long, BigInteger>.convert = (value) => value;
            Converter<ulong, BigInteger>.convert = (value) => value;
            Converter<float, BigInteger>.convert = (value) => (long)value;
            Converter<double, BigInteger>.convert = (value) => (long)value;
            Converter<decimal, BigInteger>.convert = (value) => (long)value;
            Converter<DateTime, BigInteger>.convert = (value) => value.Ticks;
            Converter<TimeSpan, BigInteger>.convert = (value) => value.Ticks;
            Converter<string, BigInteger>.convert = (value) => BigInteger.Parse(value);
            Converter<char[], BigInteger>.convert = (value) => BigInteger.Parse(new string(value));
            Converter<StringBuilder, BigInteger>.convert = (value) => BigInteger.Parse(value.ToString());
            Converter<object, BigInteger>.convert = (value) => {
                switch (value) {
                    case BigInteger bi:
                        return bi;
                    case string s:
                        return BigInteger.Parse(s);
                    case DateTime dt:
                        return dt.Ticks;
                    case TimeSpan ts:
                        return ts.Ticks;
                    case char[] chars:
                        return BigInteger.Parse(new string(chars));
                    case StringBuilder sb:
                        return BigInteger.Parse(sb.ToString());
                    default:
                        return Converter<BigInteger>.ConvertFromObject(value);
                }
            };
        }
        internal static void Initialize() { }
    }
}
