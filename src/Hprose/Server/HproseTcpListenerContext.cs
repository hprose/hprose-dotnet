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
 * HproseTcpListenerContext.cs                            *
 *                                                        *
 * hprose tcp listener context class for C#.              *
 *                                                        *
 * LastModified: May 30, 2015                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !(dotNET10 || dotNET11 || ClientOnly)
using System.Net.Sockets;
using Hprose.Common;

namespace Hprose.Server {
    public class HproseTcpListenerContext : HproseContext {
        private readonly TcpClient client;
        public HproseTcpListenerContext(TcpClient client) {
            this.client = client;
        }
        public TcpClient Client {
            get {
                return client;
            }
        }
    }
}
#endif
