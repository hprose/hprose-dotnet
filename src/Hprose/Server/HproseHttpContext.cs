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
 * HproseHttpContext.cs                                   *
 *                                                        *
 * hprose http context class for C#.                      *
 *                                                        *
 * LastModified: May 30, 2015                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !(ClientOnly || ClientProfile || Smartphone || dotNETMF)
using System.Web;
using System.Web.SessionState;
using Hprose.Common;

namespace Hprose.Server {
    public class HproseHttpContext : HproseContext {
        private readonly HttpContext context;

        public HproseHttpContext(HttpContext context) {
            this.context = context;
        }
        public HttpContext Context {
            get {
                return context;
            }
        }
        public HttpRequest Request {
            get {
                return context.Request;
            }
        }
        public HttpResponse Response {
            get {
                return context.Response;
            }
        }
        public HttpServerUtility Server {
            get {
                return context.Server;
            }
        }
        public HttpApplicationState Application {
            get {
                return context.Application;
            }
        }
        public HttpSessionState Session {
            get {
                return context.Session;
            }
        }
    }
}
#endif
