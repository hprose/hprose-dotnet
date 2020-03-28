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
|  LastModified: Mar 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Net;

namespace Hprose.RPC {
    public class ServiceContext : Context {
        public Service Service { get; private set; }
        public Method Method { get; set; } = null;
        public EndPoint RemoteEndPoint { get; set; } = null;
        public EndPoint LocalEndPoint { get; set; } = null;
        public object Handler { get; set; } = null;
        public ServiceContext(Service service) {
            Service = service;
        }
        public override void CopyTo(Context context) {
            base.CopyTo(context);
            var serviceContext = context as ServiceContext;
            serviceContext.Service = Service;
            serviceContext.Method = Method;
            serviceContext.RemoteEndPoint = RemoteEndPoint;
            serviceContext.LocalEndPoint = LocalEndPoint;
            serviceContext.Handler = Handler;
        }
    }
}