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
|  LastModified: Feb 3, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.RPC {
    public class ServiceContext : Context {
        public Service Service { get; private set; }
        public Method Method { get; set; } = null;
        public ServiceContext(Service service) {
            Service = service;
        }
    }
}