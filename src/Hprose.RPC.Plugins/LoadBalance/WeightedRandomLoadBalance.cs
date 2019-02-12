/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  WeightedRandomLoadBalance.cs                            |
|                                                          |
|  Weighted Random LoadBalance plugin for C#.              |
|                                                          |
|  LastModified: Feb 10, 2019                              |
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
    public class WeightedRandomLoadBalance : WeightedLoadBalance, IDisposable {
        private readonly ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
        private readonly int[] effectiveWeights;
        private readonly ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim();
        public WeightedRandomLoadBalance(IDictionary<string, int> uriList) : base(uriList) {
            int n = uriList.Count;
            effectiveWeights = new int[n];
            Array.Copy(weights, effectiveWeights, n);
        }
        public override async Task<Stream> Handler(Stream request, Context context, NextIOHandler next) {
            int n = uris.Length;
            var index = n - 1;
            rwlock.EnterReadLock();
            var totalWeight = effectiveWeights.Sum();
            if (totalWeight > 0) {
                var currentWeight = random.Value.Next(totalWeight);
                for (int i = 0; i < n; ++i) {
                    currentWeight -= effectiveWeights[i];
                    if (currentWeight < 0) {
                        index = i;
                        break;
                    }
                }
            }
            else {
                index = random.Value.Next(n);
            }
            rwlock.ExitReadLock();
            (context as ClientContext).Uri = uris[index];
            try {
                var response = await next(request, context).ConfigureAwait(false);
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
                random.Dispose();
                rwlock.Dispose();
            }
            disposed = true;
        }
    }
}