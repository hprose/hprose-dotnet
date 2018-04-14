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
 * TimeSpanConverter.cs                                   *
 *                                                        *
 * hprose TimeSpanConverter class for C#.                 *
 *                                                        *
 * LastModified: Apr 14, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Text;

namespace Hprose.IO.Converters {
    class TimeSpanConverter : Converter<TimeSpan> {
        public override TimeSpan Convert(object obj) {
            switch (obj) {
                case DateTime dt:
                    return new TimeSpan(dt.Ticks);
                case char[] chars:
                    return TimeSpan.Parse(new String(chars));
                case StringBuilder sb:
                    return TimeSpan.Parse(sb.ToString());
                case String s:
                    return TimeSpan.Parse(s);
                default:
                    return base.Convert(obj);
            }
        }
    }
}
