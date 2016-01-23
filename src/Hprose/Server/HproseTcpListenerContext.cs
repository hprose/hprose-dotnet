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
 * LastModified: Jan 23, 2016                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !(ClientOnly || dotNETMF)
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
