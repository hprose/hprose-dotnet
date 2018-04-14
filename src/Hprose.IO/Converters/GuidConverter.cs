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
 * GuidConverter.cs                                       *
 *                                                        *
 * hprose GuidConverter class for C#.                     *
 *                                                        *
 * LastModified: Apr 14, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Text;

namespace Hprose.IO.Converters {
    class GuidConverter : Converter<Guid> {
        public override Guid Convert(object obj) {
            switch (obj) {
                case Guid guid:
                    return guid;
                case byte[] bytes:
                    return new Guid(bytes);
                case string s:
                    return new Guid(s);
                case char[] chars:
                    return new Guid(new String(chars));
                case StringBuilder sb:
                    return new Guid(sb.ToString());
                default:
                    return base.Convert(obj);
            }
        }
    }
}
