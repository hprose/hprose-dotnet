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
|  LastModified: Feb 1, 2019                               |
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
                dynamic clientContext = context;
                clientContext.Retried++;
                TimeSpan interval = clientContext.Retried * minInterval;
                if (interval > maxInterval) {
                    interval = maxInterval;
                }
                return interval;
            };
        }
    }
}
