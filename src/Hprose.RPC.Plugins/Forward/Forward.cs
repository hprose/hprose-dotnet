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
|  LastModified: Mar 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Forward {
    public class Forward : IDisposable {
        private readonly Client client;
        public TimeSpan Timeout { get; set; }
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
            var clientContext = new ClientContext {
                Timeout = Timeout
            };
            clientContext.Init(client);
            return client.Request(request, clientContext);
        }
        public async Task<object> InvokeHandler(string name, object[] args, Context context, NextInvokeHandler next) {
            var clientContext = new ClientContext {
                Timeout = Timeout
            };
            clientContext.RequestHeaders = context.RequestHeaders;
            var result = await client.InvokeAsync<object>(name, args, clientContext).ConfigureAwait(false);
            context.ResponseHeaders = clientContext.ResponseHeaders;
            return result;
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
