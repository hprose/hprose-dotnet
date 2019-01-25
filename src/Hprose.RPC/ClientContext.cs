/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ClientContext.cs                                        |
|                                                          |
|  ClientContext class for C#.                             |
|                                                          |
|  LastModified: Jan 20, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
using System;
using System.Collections.Generic;

namespace Hprose.RPC {
    public class ClientContext : Context {
        public Client Client { get; private set; }
        public string Uri { get; set; }
        public bool Simple { get; set; }
        public Mode Mode { get; set; }
        public LongType LongType { get; set; }
        public RealType RealType { get; set; }
        public CharType CharType { get; set; }
        public ListType ListType { get; set; }
        public DictType DictType { get; set; }
        public Type Type { get; set; }
        private void Copy(IDictionary<string, object> src, IDictionary<string, object> dist) {
            if (src != null) {
                foreach (var p in src) dist[p.Key] = p.Value;
            }
        }
        public ClientContext(Client client, string fullname, Type type, Settings settings = null) {
            Client = client;
            Uri = (client.Uris.Count > 0) ? client.Uris[0] : "";
            Simple = settings?.Simple ?? client.Simple;
            Mode = settings?.Mode ?? client.Mode;
            LongType = settings?.LongType ?? client.LongType;
            RealType = settings?.RealType ?? client.RealType;
            CharType = settings?.CharType ?? client.CharType;
            ListType = settings?.ListType ?? client.ListType;
            DictType = settings?.DictType ?? client.DictType;
            Type = settings?.Type;
            if (!type.IsAssignableFrom(Type)) Type = type;
            Copy(client.RequestHeaders, RequestHeaders);
            Copy(settings?.RequestHeaders, RequestHeaders);
            Copy(settings?.Context, items);
        }
    }
}