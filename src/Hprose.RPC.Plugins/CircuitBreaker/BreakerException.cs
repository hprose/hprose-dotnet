/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  BreakerException.cs                                     |
|                                                          |
|  BreakerException for C#.                                |
|                                                          |
|  LastModified: Feb 1, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.RPC.Plugins.CircuitBreaker {
    public class BreakerException : Exception {
        public BreakerException() : base("service breaked") { }
        public BreakerException(string message) : base(message) { }
    }
}