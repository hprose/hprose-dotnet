/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ClusterConfig.cs                                        |
|                                                          |
|  ClusterConfig class for C#.                             |
|                                                          |
|  LastModified: Feb 1, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.RPC.Plugins.Cluster {
    public class ClusterConfig {
        public int Retry { get; set; } = -1;
        public bool Idempotent { get; set; } = false;
        public Action<Context> OnSuccess { get; set; } = null;
        public Action<Context> OnFailure { get; set; } = null;
        public Func<Context, TimeSpan> OnRetry { get; set; } = null;
    }
}
