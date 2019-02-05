/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ProviderContext.cs                                      |
|                                                          |
|  ProviderContext class for C#.                           |
|                                                          |
|  LastModified: Feb 3, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.RPC.Plugins.Reverse {
    public class ProviderContext : Context {
        public Client Client { get; private set; }
        public Method Method { get; private set; }
        public ProviderContext(Client client, Method method) {
            Client = client;
            Method = method;
        }
    }
}