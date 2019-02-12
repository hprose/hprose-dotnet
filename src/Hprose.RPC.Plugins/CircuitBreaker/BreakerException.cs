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
using System.Runtime.Serialization;

namespace Hprose.RPC.Plugins.CircuitBreaker {
    [Serializable]
    public class BreakerException : Exception {
        public BreakerException() : base("service breaked") { }
        public BreakerException(string message) : base(message) { }
        public BreakerException(string message, Exception innerException) : base(message, innerException) { }
        protected BreakerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}