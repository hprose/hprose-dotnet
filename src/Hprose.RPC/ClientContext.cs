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
|  LastModified: Feb 27, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;

namespace Hprose.RPC {
    public class ClientContext : Context {
        public Client Client { get; private set; }
        public Uri Uri { get; set; }
        public Type ReturnType { get; set; }
        public TimeSpan Timeout { get; set; }
        public IDictionary<string, object> RequestHeaders { get; private set; } = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        public IDictionary<string, object> ResponseHeaders { get; private set; } = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        public void Init(Client client, Type returnType) {
            Client = client;
            if (client.Uris.Count > 0) Uri = client.Uris[0];
            if (ReturnType == null) ReturnType = returnType;
            if (Timeout == default) Timeout = client.Timeout;
            Copy(client.RequestHeaders, RequestHeaders);
        }
        public override object Clone() {
            var context = base.Clone() as ClientContext;
            context.RequestHeaders = new Dictionary<string, object>(RequestHeaders, StringComparer.InvariantCultureIgnoreCase);
            context.ResponseHeaders = new Dictionary<string, object>(ResponseHeaders, StringComparer.InvariantCultureIgnoreCase);
            return context;
        }
    }
}