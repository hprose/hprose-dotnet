/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  IProducer.cs                                            |
|                                                          |
|  IProducer interface for C#.                             |
|                                                          |
|  LastModified: Feb 2, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections.Generic;

namespace Hprose.RPC.Plugins.Push {
    public interface IProducer {
        string From { get; }
        bool Unicast(object data, string topic, string id);
        IDictionary<string, bool> Multicast(object data, string topic, IEnumerable<string> ids);
        IDictionary<string, bool> Broadcast(object data, string topic);
        bool Push(object data, string topic, string id);
        IDictionary<string, bool> Push(object data, string topic, IEnumerable<string> ids);
        IDictionary<string, bool> Push(object data, string topic);
        void Deny(string id = null, string topic = null);
        bool Exists(string topic, string id = null);
        IList<string> IdList(string topic);
    }
}
