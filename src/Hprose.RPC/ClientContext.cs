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
|  LastModified: Dec 30, 2019                              |
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
        public void Init(Client client, Type returnType) {
            Client = client;
            if (client.Uris.Count > 0) Uri = client.Uris[0];
            if (ReturnType == null) ReturnType = returnType;
            if (Timeout == default) Timeout = client.Timeout;
            Copy(client.RequestHeaders, RequestHeaders);
        }
    }
}