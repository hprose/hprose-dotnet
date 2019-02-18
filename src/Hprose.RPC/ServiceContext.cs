/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ServiceContext.cs                                       |
|                                                          |
|  ServiceContext class for C#.                            |
|                                                          |
|  LastModified: Feb 18, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.Net;

namespace Hprose.RPC {
    public class ServiceContext : Context {
        public Service Service { get; private set; }
        public Method Method { get; set; } = null;
        public EndPoint RemoteEndPoint { get; set; } = null;
        public object Handler { get; set; } = null;
        public IDictionary<string, object> RequestHeaders { get; private set; } = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        public IDictionary<string, object> ResponseHeaders { get; private set; } = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        public ServiceContext(Service service) {
            Service = service;
        }
        public override object Clone() {
            var context = base.Clone() as ServiceContext;
            context.RequestHeaders = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            context.ResponseHeaders = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            Copy(RequestHeaders, context.RequestHeaders);
            Copy(ResponseHeaders, context.ResponseHeaders);
            return context;
        }
    }
}