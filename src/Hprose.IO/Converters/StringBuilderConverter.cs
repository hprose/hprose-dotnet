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
 * StringBuilderConverter.cs                              *
 *                                                        *
 * hprose StringBuilderConverter class for C#.            *
 *                                                        *
 * LastModified: Apr 14, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Hprose.IO.Converters {
    class StringBuilderConverter : Converter<StringBuilder> {
        public override StringBuilder Convert(object obj) {
            switch (obj) {
                case StringBuilder sb:
                    return sb;
                case String s:
                    return new StringBuilder(s);
                case char[] chars:
                    return new StringBuilder(chars.Length).Append(chars);
                case List<char> charList:
                    return new StringBuilder(charList.Count).Append(charList.ToArray());
                case byte[] bytes:
                    try {
                        return new StringBuilder(Encoding.UTF8.GetString(bytes));
                    }
                    catch(Exception) {
                        return new StringBuilder(Encoding.Default.GetString(bytes));
                    }
                default:
                    return new StringBuilder(obj.ToString());
            }
        }
    }
}
