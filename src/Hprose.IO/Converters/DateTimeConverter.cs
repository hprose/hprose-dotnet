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
 * DateTimeConverter.cs                                   *
 *                                                        *
 * hprose DateTimeConverter class for C#.                 *
 *                                                        *
 * LastModified: Apr 14, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Text;

namespace Hprose.IO.Converters {
    class DateTimeConverter : Converter<DateTime> {
        public override DateTime Convert(object obj) {
            switch (obj) {
                case DateTime dt:
                    return dt;
                case char[] chars:
                    return DateTime.Parse(new String(chars));
                case StringBuilder sb:
                    return DateTime.Parse(sb.ToString());
                case String s:
                    return DateTime.Parse(s);
                default:
                    return base.Convert(obj);
            }
        }
    }
}
