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
 * BaseConverter.cs                                       *
 *                                                        *
 * hprose BaseConverter class for C#.                     *
 *                                                        *
 * LastModified: Apr 10, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Text;

namespace Hprose.IO.Converters {
    class BaseConverter<T> : Converter<T> {
        public override T Convert(object obj) {
            switch (obj) {
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
