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
 * BigIntegerConverter.cs                                 *
 *                                                        *
 * hprose BigIntegerConverter class for C#.               *
 *                                                        *
 * LastModified: Apr 13, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Numerics;
using System.Text;

namespace Hprose.IO.Converters {
    class BigIntegerConverter : Converter<BigInteger> {
        public override BigInteger Convert(object obj) {
            switch (obj) {
                case DateTime dt:
                    return dt.Ticks;
                case char[] chars:
                    return BigInteger.Parse(new String(chars));
                case StringBuilder sb:
                    return BigInteger.Parse(sb.ToString());
                case String s:
                    return BigInteger.Parse(s);
                default:
                    return base.Convert(obj);
            }
        }
    }
}
