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
 * HproseClientContext.cs                                 *
 *                                                        *
 * hprose client context class for C#.                    *
 *                                                        *
 * LastModified: May 30, 2015                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using Hprose.Common;

namespace Hprose.Client {
    public class HproseClientContext : HproseContext {
        private readonly HproseClient client;

        public HproseClientContext(HproseClient client) {
            this.client = client;
        }

        public HproseClient Client {
            get {
                return client;
            }
        }
    }
}
