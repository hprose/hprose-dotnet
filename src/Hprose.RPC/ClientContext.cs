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
|  LastModified: Mar 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.RPC {
    public class ClientContext : Context {
        public Client Client { get; private set; }
        public Uri Uri { get; set; }
        public Type ReturnType { get; set; }
        public TimeSpan Timeout { get; set; }
        public void Init(Client client, Type returnType = null) {
            Client = client;
            if (client.Uris.Count > 0) Uri = client.Uris[0];
            if (ReturnType == null) ReturnType = returnType;
            if (Timeout == default) Timeout = client.Timeout;
            Copy(client.RequestHeaders, RequestHeaders);
        }
        public override void CopyTo(Context context) {
            base.CopyTo(context);
            var clientContext = context as ClientContext;
            clientContext.Client = Client;
            clientContext.Uri = Uri;
            clientContext.ReturnType = ReturnType;
            clientContext.Timeout = Timeout;
        }
    }
}