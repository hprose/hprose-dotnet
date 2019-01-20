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
            client.Settings.TryGetValue(fullname, out Settings defaultSettings);
            Simple = settings?.Simple ?? defaultSettings?.Simple ?? client.Simple;
            Mode = settings?.Mode ?? defaultSettings?.Mode ?? client.Mode;
            LongType = settings?.LongType ?? defaultSettings?.LongType ?? client.LongType;
            RealType = settings?.RealType ?? defaultSettings?.RealType ?? client.RealType;
            CharType = settings?.CharType ?? defaultSettings?.CharType ?? client.CharType;
            ListType = settings?.ListType ?? defaultSettings?.ListType ?? client.ListType;
            DictType = settings?.DictType ?? defaultSettings?.DictType ?? client.DictType;
            Type = settings?.Type ?? defaultSettings?.Type;
            if (!type.IsAssignableFrom(Type)) Type = type;
            Copy(client.RequestHeaders, RequestHeaders);
            Copy(defaultSettings?.RequestHeaders, RequestHeaders);
            Copy(settings?.RequestHeaders, RequestHeaders);
            Copy(defaultSettings?.Context, items);
            Copy(settings?.Context, items);
        }
    }
}