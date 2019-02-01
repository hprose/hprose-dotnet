/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  RoundRobinLoadBalance.cs                                |
|                                                          |
|  RoundRobin LoadBalance plugin for C#.                   |
|                                                          |
|  LastModified: Feb 1, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.LoadBalance {
    public class RoundRobinLoadBalance {
        private volatile int index = -1;
        public Task<Stream> Handler(Stream request, Context context, NextIOHandler next) {
            var clientContext = context as ClientContext;
            var uris = clientContext.Client.Uris;
            var n = uris.Count;
            if (n > 1) {
                if (Interlocked.Increment(ref index) >= n) {
                    index = 0;
                }
                clientContext.Uri = uris[index];
            }
            return next(request, context);
        }
    }
}
