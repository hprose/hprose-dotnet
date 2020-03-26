/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Forward.cs                                              |
|                                                          |
|  Forward plugin for C#.                                  |
|                                                          |
|  LastModified: Mar 26, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Oneway {
    public class Forward : IDisposable {
        private readonly Client client;
        public Forward() {
            client = new Client();
        }
        public Forward(string uri) {
            client = new Client(uri);
        }
        public Forward(Uri uri) {
            client = new Client(uri);
        }
        public Forward(string[] uris) {
            client = new Client(uris);
        }
        public Forward(Uri[] uris) {
            client = new Client(uris);
        }
        public Task<Stream> IOHandler(Stream request, Context context, NextIOHandler next) {
            var clientContext = new ClientContext();
            clientContext.Init(client, null);
            return client.Request(request, clientContext);
        }
        public Task<object> InvokeHandler(string fullname, object[] args, Context context, NextInvokeHandler next) {
            return client.InvokeAsync<object>(fullname, args);
        }
        public Forward Use(params InvokeHandler[] handlers) {
            client.Use(handlers);
            return this;
        }
        public Forward Use(params IOHandler[] handlers) {
            client.Use(handlers);
            return this;
        }
        public Forward Unuse(params InvokeHandler[] handlers) {
            client.Unuse(handlers);
            return this;
        }
        public Forward Unuse(params IOHandler[] handlers) {
            client.Unuse(handlers);
            return this;
        }
        public void Dispose() {
            client.Dispose();
        }
    }
}
