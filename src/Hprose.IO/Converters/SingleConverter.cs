/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  SingleConverter.cs                                      |
|                                                          |
|  hprose SingleConverter class for C#.                    |
|                                                          |
|  LastModified: Apr 21, 2018                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Numerics;

namespace Hprose.IO.Converters {
    static class SingleConverter {
        static SingleConverter() {
            Converter<bool, float>.convert = Convert.ToSingle;
            Converter<char, float>.convert = Convert.ToSingle;
            Converter<byte, float>.convert = Convert.ToSingle;
            Converter<sbyte, float>.convert = Convert.ToSingle;
            Converter<short, float>.convert = Convert.ToSingle;
            Converter<ushort, float>.convert = Convert.ToSingle;
            Converter<int, float>.convert = Convert.ToSingle;
            Converter<uint, float>.convert = Convert.ToSingle;
            Converter<long, float>.convert = Convert.ToSingle;
            Converter<ulong, float>.convert = Convert.ToSingle;
            Converter<double, float>.convert = Convert.ToSingle;
            Converter<decimal, float>.convert = Convert.ToSingle;
            Converter<DateTime, float>.convert = Convert.ToSingle;
            Converter<BigInteger, float>.convert = (value) => (float)value;
        }
        internal static void Initialize() { }
    }
}
