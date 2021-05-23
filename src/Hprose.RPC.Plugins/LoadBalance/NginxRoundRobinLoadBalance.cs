/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  NginxRoundRobinLoadBalance.cs                           |
|                                                          |
|  Nginx RoundRobin LoadBalance plugin for C#.             |
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
    public class NginxRoundRobinLoadBalance : WeightedLoadBalance {
        // SpinLock can't store in readonly field.
        private SpinLock spanlock = new SpinLock();
        private readonly ThreadLocal<Random> random = new(() => new Random(Guid.NewGuid().GetHashCode()));
        private readonly int[] effectiveWeights;
        private readonly int[] currentWeights;
        public NginxRoundRobinLoadBalance(IDictionary<string, int> uriList) : base(uriList) {
            int n = uriList.Count;
            effectiveWeights = new int[n];
            Array.Copy(weights, effectiveWeights, n);
            currentWeights = new int[n];
        }
        public override async Task<Stream> Handler(Stream request, Context context, NextIOHandler next) {
            int n = uris.Length;
            int index = -1;
            bool gotlock = false;
            try {
                spanlock.Enter(ref gotlock);
                var totalWeight = effectiveWeights.Sum();
                if (totalWeight > 0) {
                    int currentWeight = int.MinValue;
                    for (int i = 0; i < n; ++i) {
                        int weight = (currentWeights[i] += effectiveWeights[i]);
                        if (currentWeight < weight) {
                            currentWeight = weight;
                            index = i;
                        }
                    }
                    currentWeights[index] = currentWeight - totalWeight;
                }
                else {
                    index = random.Value.Next(n);
                }
            }
            finally {
                if (gotlock) spanlock.Exit();
            }
            (context as ClientContext).Uri = uris[index];
            try {
                var response = await next(request, context).ConfigureAwait(false);
                gotlock = false;
                try {
                    spanlock.Enter(ref gotlock);
                    if (effectiveWeights[index] < weights[index]) {
                        effectiveWeights[index]++;
                    }
                }
                finally {
                    if (gotlock) spanlock.Exit();
                }
                return response;
            }
            catch {
                gotlock = false;
                try {
                    spanlock.Enter(ref gotlock);
                    if (effectiveWeights[index] > 0) {
                        effectiveWeights[index]--;
                    }
                }
                finally {
                    if (gotlock) spanlock.Exit();
                }
                throw;
            }
        }
    }
}
