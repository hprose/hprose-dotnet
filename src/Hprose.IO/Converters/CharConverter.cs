/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  CharConverter.cs                                        |
|                                                          |
|  hprose CharConverter class for C#.                      |
|                                                          |
|  LastModified: Apr 21, 2018                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Numerics;

namespace Hprose.IO.Converters {
    internal static class CharConverter {
        static CharConverter() {
            Converter<bool, char>.convert = Convert.ToChar;
            Converter<sbyte, char>.convert = Convert.ToChar;
            Converter<byte, char>.convert = Convert.ToChar;
            Converter<short, char>.convert = Convert.ToChar;
            Converter<ushort, char>.convert = Convert.ToChar;
            Converter<int, char>.convert = Convert.ToChar;
            Converter<uint, char>.convert = Convert.ToChar;
            Converter<long, char>.convert = Convert.ToChar;
            Converter<float, char>.convert = Convert.ToChar;
            Converter<double, char>.convert = Convert.ToChar;
            Converter<decimal, char>.convert = Convert.ToChar;
            Converter<DateTime, char>.convert = Convert.ToChar;
            Converter<BigInteger, char>.convert = (value) => (char)value;
        }
        internal static void Initialize() { }
    }
}
