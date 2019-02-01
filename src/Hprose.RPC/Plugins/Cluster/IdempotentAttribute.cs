/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  IdempotentAttribute.cs                                  |
|                                                          |
|  Idempotent Attribute for C#.                            |
|                                                          |
|  LastModified: May 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.RPC.Plugins.Cluster {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class IdempotentAttribute : ContextAttribute {
        public IdempotentAttribute(bool value = true) : base("Idempotent", value) { }
    }
}
