/**********************************************************\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: http://www.hprose.com/                 |
|                   http://www.hprose.net/                 |
|                   http://www.hprose.org/                 |
|                                                          |
\**********************************************************/
/**********************************************************\
 *                                                        *
 * HproseTcpMethods.cs                                    *
 *                                                        *
 * hprose tcp remote methods class for C#.                *
 *                                                        *
 * LastModified: Mar 18, 2014                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !(dotNET10 || dotNET11 || ClientOnly)
using System;
using System.Net.Sockets;
using Hprose.Common;

namespace Hprose.Server {
    public class HproseTcpMethods : HproseMethods {
        protected override int GetCount(Type[] paramTypes) {
            int i = paramTypes.Length;
            if (i > 0) {
                Type paramType = paramTypes[i - 1];
                if (paramType == typeof(TcpClient)) {
                    i--;
                }
            }
            return i;
        }
    }
}
#endif