/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  LeastActiveLoadBalance.cs                               |
|                                                          |
|  LeastActive LoadBalance plugin for C#.                  |
|                                                          |
|  LastModified: Feb 1, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.LoadBalance {
    public class LeastActiveLoadBalance {
        private readonly Random random = new Random(Guid.NewGuid().GetHashCode());
        private int[] actives = new int[0];
        private readonly ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim();
        public async Task<Stream> Handler(Stream request, Context context, NextIOHandler next) {
            var clientContext = context as ClientContext;
            var uris = clientContext.Client.Uris;
            var n = uris.Count;
            var leastActiveIndexes = new List<int>(n);

            rwlock.EnterUpgradeableReadLock();
            if (actives.Length < n) {
                rwlock.EnterWriteLock();
                actives = new int[n];
                rwlock.ExitWriteLock();
            }
            rwlock.ExitUpgradeableReadLock();

            rwlock.EnterReadLock();
            var leastActive = (actives.Length > n) ? actives.Take(n).Min() : actives.Min();
            for (int i = 0; i < n; ++i) {
                if (actives[i] == leastActive) {
                    leastActiveIndexes.Add(i);
                }
            }
            rwlock.ExitReadLock();

            int index = leastActiveIndexes[0];
            var count = leastActiveIndexes.Count;
            if (count > 1) {
                index = leastActiveIndexes[random.Next(count)];
            }

            (context as ClientContext).Uri = uris[index];

            rwlock.EnterWriteLock();
            actives[index]++;
            rwlock.ExitWriteLock();
            try {
                var response = await next(request, context);
                rwlock.EnterWriteLock();
                actives[index]--;
                rwlock.ExitWriteLock();
                return response;
            }
            catch (Exception e) {
                rwlock.EnterWriteLock();
                actives[index]--;
                rwlock.ExitWriteLock();
                throw e;
            }
        }
    }
}
