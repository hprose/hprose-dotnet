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
 * CharsConverter.cs                                      *
 *                                                        *
 * hprose CharsConverter class for C#.                    *
 *                                                        *
 * LastModified: Apr 14, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Hprose.IO.Converters {
    class CharsConverter : Converter<char[]> {
        public override char[] Convert(object obj) {
            switch (obj) {
                case char[] chars:
                    return chars;
                case String s:
                    return s.ToCharArray();
                case StringBuilder sb:
                    char[] result = new char[sb.Length];
                    sb.CopyTo(0, result, 0, sb.Length);
                    return result;
                case List<char> charList:
                    return charList.ToArray();
                case byte[] bytes:
                    try {
                        return Encoding.UTF8.GetChars(bytes);
                    }
                    catch(Exception) {
                        return Encoding.Default.GetChars(bytes);
                    }
                default:
                    return obj.ToString().ToCharArray();
            }
        }
    }
}
