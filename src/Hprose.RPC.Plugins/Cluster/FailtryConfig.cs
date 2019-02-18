/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  FailtryConfig.cs                                        |
|                                                          |
|  FailtryConfig class for C#.                             |
|                                                          |
|  LastModified: Feb 18, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.RPC.Plugins.Cluster {
    public class FailtryConfig : ClusterConfig {
        public static ClusterConfig Instance { get; } = new FailtryConfig();
        public FailtryConfig(int retry = 10, TimeSpan minInterval = default, TimeSpan maxInterval = default) {
            Retry = retry;
            if (minInterval == default) minInterval = new TimeSpan(0, 0, 0, 0, 500);
            if (maxInterval == default) maxInterval = new TimeSpan(0, 0, 5);
            OnRetry = (context) => {
                var clientContext = context as ClientContext;
                int retried = (int)context["retried"];
                context["retried"] = ++retried;
                TimeSpan interval = new TimeSpan(minInterval.Ticks * retried);
                if (interval > maxInterval) {
                    interval = maxInterval;
                }
                return interval;
            };
        }
    }
}
