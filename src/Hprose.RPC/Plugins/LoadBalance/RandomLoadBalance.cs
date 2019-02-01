/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  RandomLoadBalance.cs                                    |
|                                                          |
|  Random LoadBalance plugin for C#.                       |
|                                                          |
|  LastModified: Feb 1, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.LoadBalance {
    public class RandomLoadBalance {
        private readonly Random random = new Random(Guid.NewGuid().GetHashCode());
        public Task<Stream> Handler(Stream request, Context context, NextIOHandler next) {
            var clientContext = context as ClientContext;
            var uris = clientContext.Client.Uris;
            var n = uris.Count;
            clientContext.Uri = uris[random.Next(n)];
            return next(request, context);
        }
    }
}
