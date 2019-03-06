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
        public BreakerException() : base("Service breaked") { }
        public BreakerException(string message) : base(message) { }
        public BreakerException(string message, Exception innerException) : base(message, innerException) { }
#if !NET35_CF
        protected BreakerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
    }
}