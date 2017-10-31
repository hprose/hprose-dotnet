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
 * HproseTcpListenerMethods.cs                            *
 *                                                        *
 * hprose tcp listener emote methods class for C#.        *
 *                                                        *
 * LastModified: Jan 23, 2016                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !(ClientOnly || dotNETMF)
using System;
using System.Net.Sockets;
using Hprose.Common;

namespace Hprose.Server {
    public class HproseTcpListenerMethods : HproseMethods {
        protected override int GetCount(Type[] paramTypes) {
            int i = paramTypes.Length;
            if (i > 0) {
                Type paramType = paramTypes[i - 1];
                if (paramType == typeof(HproseContext) ||
                    paramType == typeof(HproseTcpListenerContext) ||
                    paramType == typeof(TcpClient)) {
                    --i;
                }
            }
            return i;
        }
    }
}
#endif
