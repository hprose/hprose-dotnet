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
 * HproseHttpListenerMethods.cs                           *
 *                                                        *
 * hprose http listener remote methods class for C#.      *
 *                                                        *
 * LastModified: Jan 23, 2016                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !(dotNET10 || dotNET11 || ClientOnly || Smartphone)
using System;
using System.Net;
#if !dotNETMF
using System.Security.Principal;
#endif
using Hprose.Common;

namespace Hprose.Server {
    public class HproseHttpListenerMethods : HproseMethods {
        protected override int GetCount(Type[] paramTypes) {
            int i = paramTypes.Length;
            if (i > 0) {
                Type paramType = paramTypes[i - 1];
                if (paramType == typeof(HttpListenerRequest) ||
                    paramType == typeof(HttpListenerResponse) ||
#if !dotNETMF
                    paramType == typeof(IPrincipal) ||
#endif
                    paramType == typeof(HproseContext) ||
                    paramType == typeof(HproseHttpListenerContext) ||
                    paramType == typeof(HttpListenerContext)) {
                    --i;
                }
            }
            return i;
        }
    }
}
#endif
