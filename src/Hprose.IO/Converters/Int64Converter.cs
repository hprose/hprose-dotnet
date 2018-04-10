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
 * Int64Converter.cs                                      *
 *                                                        *
 * hprose Int64Converter class for C#.                    *
 *                                                        *
 * LastModified: Apr 10, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Text;

namespace Hprose.IO.Converters {
    class Int64Converter : Converter<long> {
        public override long Convert(object obj) {
            switch (obj) {
                case DateTime dt:
                    return dt.Ticks;
                case char[] chars:
                    return base.Convert(new String(chars));
                case StringBuilder sb:
                    return base.Convert(sb.ToString());
                default:
                    return base.Convert(obj);
            }
        }
    }
}
