/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  FailoverConfig.cs                                       |
|                                                          |
|  FailoverConfig class for C#.                            |
|                                                          |
|  LastModified: Feb 18, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Threading;

namespace Hprose.RPC.Plugins.Cluster {
    public class FailoverConfig : ClusterConfig {
        public static ClusterConfig Instance { get; } = new FailoverConfig();
        public FailoverConfig(int retry = 10, TimeSpan minInterval = default, TimeSpan maxInterval = default) {
            Retry = retry;
            if (minInterval == default) minInterval = new TimeSpan(0, 0, 0, 0, 500);
            if (maxInterval == default) maxInterval = new TimeSpan(0, 0, 5);
            var index = 0;
            OnFailure = (context) => {
                var clientContext = context as ClientContext;
                var uris = clientContext.Client.Uris;
                var n = uris.Count;
                if (n > 1) {
                    if (Interlocked.Increment(ref index) >= n) {
                        index = 0;
                    }
                    clientContext.Uri = uris[index];
                }
            };
            OnRetry = (context) => {
                var clientContext = context as ClientContext;
                int retried = (int)context["retried"];
                context["retried"] = ++retried;
                TimeSpan interval = new TimeSpan(minInterval.Ticks * (retried - clientContext.Client.Uris.Count));
                if (interval > maxInterval) {
                    interval = maxInterval;
                }
                return interval;
            };
        }
    }
}
