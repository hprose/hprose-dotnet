/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  FailfastConfig.cs                                       |
|                                                          |
|  FailfastConfig class for C#.                            |
|                                                          |
|  LastModified: Feb 1, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.RPC.Plugins.Cluster {
    public class FailfastConfig : ClusterConfig {
        public FailfastConfig(Action<Context> onFailure) {
            Retry = 0;
            OnFailure = onFailure;
        }
    }
}
