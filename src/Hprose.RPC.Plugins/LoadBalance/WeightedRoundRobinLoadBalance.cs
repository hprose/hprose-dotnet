/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  WeightedRoundRobinLoadBalance.cs                        |
|                                                          |
|  Weighted RoundRobin LoadBalance plugin for C#.          |
|                                                          |
|  LastModified: Feb 8, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.LoadBalance {
    public class WeightedRoundRobinLoadBalance : WeightedLoadBalance {
        private SpinLock spanlock = new SpinLock();
        private readonly int maxWeight;
        private readonly int gcdWeight;
        private volatile int index;
        private volatile int currentWeight;
        private static int GCD(int x, int y) {
            if (x < y) {
                (x, y) = (y, x);
            }
            while (y != 0) {
                (x, y) = (y, x % y);
            }
            return x;
        }
        public WeightedRoundRobinLoadBalance(IDictionary<string, int> uriList) : base(uriList) {
            maxWeight = weights.Max();
            gcdWeight = weights.Aggregate(GCD);
            index = -1;
            currentWeight = 0;
        }
        public override Task<Stream> Handler(Stream request, Context context, NextIOHandler next) {
            int n = uris.Length;
            bool gotlock = false;
            try {
                spanlock.Enter(ref gotlock);
                while (true) {
                    index = (index + 1) % n;
                    if (index == 0) {
                        currentWeight -= gcdWeight;
                        if (currentWeight <= 0) {
                            currentWeight = maxWeight;
                        }
                        if (weights[index] >= currentWeight) {
                            (context as ClientContext).Uri = uris[index];
                            break;
                        }
                    }
                }
            }
            finally {
                if (gotlock) spanlock.Exit();
            }
            return next(request, context);
        }
    }
}
