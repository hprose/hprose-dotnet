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
|  LastModified: Feb 8, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Dynamic;
using System.Net;

namespace Hprose.RPC {
    public class ServiceContext : Context {
        public Service Service { get; private set; }
        public Method Method { get; set; } = null;
        public EndPoint RemoteEndPoint { get; set; } = null;
        public dynamic RequestHeaders { get; private set; } = new ExpandoObject();
        public dynamic ResponseHeaders { get; private set; } = new ExpandoObject();
        public ServiceContext(Service service) {
            Service = service;
        }
        public override object Clone() {
            ServiceContext context = base.Clone() as ServiceContext;
            context.RequestHeaders = new ExpandoObject();
            context.ResponseHeaders = new ExpandoObject();
            Copy(RequestHeaders, context.RequestHeaders);
            Copy(ResponseHeaders, context.ResponseHeaders);
            return context;
        }
    }
}