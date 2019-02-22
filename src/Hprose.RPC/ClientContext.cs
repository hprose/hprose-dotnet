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
|  LastModified: Feb 23, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;

namespace Hprose.RPC {
    public class ClientContext : Context {
        public Client Client { get; private set; }
        public Uri Uri { get; set; }
        public Type Type { get; set; }
        public IDictionary<string, object> RequestHeaders { get; private set; } = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        public IDictionary<string, object> ResponseHeaders { get; private set; } = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        public ClientContext(Client client, Type type, Settings settings) {
            Client = client;
            Uri = (client.Uris.Count > 0) ? client.Uris[0] : null;
            Type = settings?.Type;
            if (type != null && !type.IsAssignableFrom(Type)) Type = type;
            Copy(client.RequestHeaders, RequestHeaders);
            Copy(settings?.RequestHeaders, RequestHeaders);
            Copy(settings?.Context, Items);
        }
        public override object Clone() {
            var context = base.Clone() as ClientContext;
            context.RequestHeaders = new Dictionary<string, object>(RequestHeaders, StringComparer.InvariantCultureIgnoreCase);
            context.ResponseHeaders = new Dictionary<string, object>(ResponseHeaders, StringComparer.InvariantCultureIgnoreCase);
            return context;
        }
    }
}