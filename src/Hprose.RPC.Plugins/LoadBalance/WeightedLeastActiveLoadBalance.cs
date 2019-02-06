/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  WeightedLeastActiveLoadBalance.cs                       |
|                                                          |
|  Weighted LeastActive LoadBalance plugin for C#.         |
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
    public class WeightedLeastActiveLoadBalance : WeightedLoadBalance {
        private readonly Random random = new Random(Guid.NewGuid().GetHashCode());
        private readonly int[] actives = null;
        private readonly int[] effectiveWeights;
        private readonly ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim();
        public WeightedLeastActiveLoadBalance(IDictionary<string, int> uriList) : base(uriList) {
            int n = uriList.Count;
            effectiveWeights = new int[n];
            Array.Copy(weights, effectiveWeights, n);
            actives = new int[n];
        }
        public override async Task<Stream> Handler(Stream request, Context context, NextIOHandler next) {
            int n = uris.Length;
            var leastActiveIndexes = new List<int>(n);

            rwlock.EnterReadLock();
            var leastActive = actives.Min();
            int totalWeight = 0;
            for (int i = 0; i < n; ++i) {
                if (actives[i] == leastActive) {
                    leastActiveIndexes.Add(i);
                    totalWeight += effectiveWeights[i];
                }
            }
            rwlock.ExitReadLock();

            int index = leastActiveIndexes[0];
            var count = leastActiveIndexes.Count;
            if (count > 1) {
                if (totalWeight > 0) {
                    var currentWeight = random.Next(totalWeight);
                    rwlock.EnterReadLock();
                    for (int i = 0; i < count; ++i) {
                        currentWeight -= effectiveWeights[leastActiveIndexes[i]];
                        if (currentWeight < 0) {
                            index = leastActiveIndexes[i];
                            break;
                        }
                    }
                    rwlock.ExitReadLock();
                }
                else {
                    index = leastActiveIndexes[random.Next(count)];
                }
            }

            (context as ClientContext).Uri = uris[index];

            rwlock.EnterWriteLock();
            actives[index]++;
            rwlock.ExitWriteLock();
            try {
                var response = await next(request, context).ConfigureAwait(false);
                rwlock.EnterWriteLock();
                actives[index]--;
                rwlock.ExitWriteLock();
                rwlock.EnterUpgradeableReadLock();
                if (effectiveWeights[index] < weights[index]) {
                    rwlock.EnterWriteLock();
                    effectiveWeights[index]++;
                    rwlock.ExitWriteLock();
                }
                rwlock.ExitUpgradeableReadLock();
                return response;
            }
            catch (Exception e) {
                rwlock.EnterWriteLock();
                actives[index]--;
                rwlock.ExitWriteLock();
                rwlock.EnterUpgradeableReadLock();
                if (effectiveWeights[index] > 0) {
                    rwlock.EnterWriteLock();
                    effectiveWeights[index]--;
                    rwlock.ExitWriteLock();
                }
                rwlock.ExitUpgradeableReadLock();
                throw e;
            }
        }
    }
}
