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
 * StringConverter.cs                                     *
 *                                                        *
 * hprose StringConverter class for C#.                   *
 *                                                        *
 * LastModified: Apr 14, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Text;

namespace Hprose.IO.Converters {
    class StringConverter : Converter<string> {
        public override string Convert(object obj) {
            switch (obj) {
                case String s:
                    return s;
                case char[] chars:
                    return new String(chars);
                case byte[] bytes:
                    try {
                        return Encoding.UTF8.GetString(bytes);
                    }
                    catch(Exception) {
                        return Encoding.Default.GetString(bytes);
                    }
                default:
                    return obj.ToString();
            }
        }
    }
}
