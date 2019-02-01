/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  LogAttribute.cs                                         |
|                                                          |
|  Log Attribute for C#.                                   |
|                                                          |
|  LastModified: Feb 2, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.RPC.Plugins.Log {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class LogAttribute : ContextAttribute {
        public LogAttribute(bool enabled) : base("Log", enabled) { }
    }
}