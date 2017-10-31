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
 * HproseHttpListenerContext.cs                           *
 *                                                        *
 * hprose http listener context class for C#.             *
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
    public class HproseHttpListenerContext : HproseContext {
        private readonly HttpListenerContext context;
#if dotNETMF
        [CLSCompliantAttribute(false)]
#endif
        public HproseHttpListenerContext(HttpListenerContext context) {
            this.context = context;
        }
#if dotNETMF
        [CLSCompliantAttribute(false)]
#endif
        public HttpListenerContext Context {
            get {
                return context;
            }
        }
#if dotNETMF
        [CLSCompliantAttribute(false)]
#endif
        public HttpListenerRequest Request {
            get {
                return context.Request;
            }
        }
#if dotNETMF
        [CLSCompliantAttribute(false)]
#endif
        public HttpListenerResponse Response {
            get {
                return context.Response;
            }
        }
#if !dotNETMF
        public IPrincipal User {
            get {
                return context.User;
            }
        }
#endif
    }
}
#endif
