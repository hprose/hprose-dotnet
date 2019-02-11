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
        public dynamic RequestHeaders { get; } = new ExpandoObject();
        public dynamic ResponseHeaders { get; } = new ExpandoObject();
        public ServiceContext(Service service) {
            Service = service;
        }
    }
}