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
|  LastModified: Feb 1, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.LoadBalance {
    public class NginxRoundRobinLoadBalance : WeightedLoadBalance {
        private readonly object locker = new object();
        private readonly Random random = new Random(Guid.NewGuid().GetHashCode());
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
            lock (locker) {
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
                    index = random.Next(n);
                }
            }
                (context as ClientContext).Uri = uris[index];
            try {
                var response = await next(request, context);
                lock (locker) {
                    if (effectiveWeights[index] < weights[index]) {
                        effectiveWeights[index]++;
                    }
                }
                return response;
            }
            catch (Exception e) {
                lock (locker) {
                    if (effectiveWeights[index] > 0) {
                        effectiveWeights[index]--;
                    }
                }
                throw e;
            }
        }
    }
}
