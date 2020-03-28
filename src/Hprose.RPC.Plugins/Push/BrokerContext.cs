/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  BrokerContext.cs                                        |
|                                                          |
|  BrokerContext class for C#.                             |
|                                                          |
|  LastModified: Mar 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.RPC.Plugins.Push {
    public class BrokerContext : ServiceContext {
        public IProducer Producer { get; private set; }
        public BrokerContext(IProducer producer, ServiceContext context) : base(context.Service) {
            context.CopyTo(this);
            Producer = producer;
        }
    }
}
