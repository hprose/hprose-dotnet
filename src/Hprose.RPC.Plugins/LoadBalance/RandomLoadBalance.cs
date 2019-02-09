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
|  LastModified: Feb 10, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.LoadBalance {
    public class RandomLoadBalance : IDisposable {
        private readonly ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
        public Task<Stream> Handler(Stream request, Context context, NextIOHandler next) {
            var clientContext = context as ClientContext;
            var uris = clientContext.Client.Uris;
            var n = uris.Count;
            clientContext.Uri = uris[random.Value.Next(n)];
            return next(request, context);
        }
        private bool disposed = false;
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing) {
            if (disposed) return;
            if (disposing) {
                random.Dispose();
            }
            disposed = true;
        }
    }
}
