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
 * BytesConverter.cs                                      *
 *                                                        *
 * hprose BytesConverter class for C#.                    *
 *                                                        *
 * LastModified: Apr 15, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Hprose.IO.Converters {
    class BytesConverter : Converter<byte[]> {
        private static readonly UTF8Encoding UTF8 = new UTF8Encoding();
        public override byte[] Convert(object obj) {
            switch (obj) {
                case byte[] bytes:
                    return bytes;
                case String s:
                    return UTF8.GetBytes(s);
                case StringBuilder sb:
                    return UTF8.GetBytes(sb.ToString());
                case List<char> charList:
                    return UTF8.GetBytes(charList.ToArray());
                case char[] chars:
                    return UTF8.GetBytes(chars);
                case Guid guid:
                    return guid.ToByteArray();
                default:
                    return base.Convert(obj);
            }
        }
    }
}
