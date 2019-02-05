/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  RetryAttribute.cs                                       |
|                                                          |
|  Retry Attribute for C#.                                 |
|                                                          |
|  LastModified: May 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.RPC.Plugins.Cluster {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RetryAttribute : ContextAttribute {
        public RetryAttribute(int value) : base("Retry", value) { }
    }
}
