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
|  LastModified: Mar 20, 2019                              |
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
    public class WeightedLeastActiveLoadBalance : WeightedLoadBalance, IDisposable {
        private readonly ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
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
                    var currentWeight = random.Value.Next(totalWeight);
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
                    index = leastActiveIndexes[random.Value.Next(count)];
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
            catch {
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
                throw;
            }
        }
        private bool disposed = false;
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing) {
            if (disposed) return;
            if (disposing) {
                rwlock.Dispose();
            }
            disposed = true;
        }
    }
}
