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
 * LastModified: May 30, 2015                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !(dotNET10 || dotNET11 || ClientOnly || Smartphone)
using System.Net;
using System.Security.Principal;
using Hprose.Common;

namespace Hprose.Server {
    public class HproseHttpListenerContext : HproseContext {
        private readonly HttpListenerContext context;
        public HproseHttpListenerContext(HttpListenerContext context) {
            this.context = context;
        }
        public HttpListenerContext Context {
            get {
                return context;
            }
        }
        public HttpListenerRequest Request {
            get {
                return context.Request;
            }
        }
        public HttpListenerResponse Response {
            get {
                return context.Response;
            }
        }
        public IPrincipal User {
            get {
                return context.User;
            }
        }
    }
}
#endif
