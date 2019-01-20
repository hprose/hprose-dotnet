/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Client.cs                                               |
|                                                          |
|  Client class for C#.                                    |
|                                                          |
|  LastModified: Jan 20, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
using Hprose.RPC.Codec;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public abstract class Client {
        public ConcurrentDictionary<string, Settings> Settings { get; } = new ConcurrentDictionary<string, Settings>();
        public ExpandoObject RequestHeaders { get; set; } = new ExpandoObject();
        public int Timeout { get; set; } = 30000;
        public bool Simple { get; set; } = false;
        public Mode Mode { get; set; } = Mode.MemberMode;
        public LongType LongType { get; set; } = LongType.Int64;
        public RealType RealType { get; set; } = RealType.Double;
        public CharType CharType { get; set; } = CharType.String;
        public ListType ListType { get; set; } = ListType.List;
        public DictType DictType { get; set; } = DictType.NullableKeyDictionary;
        public IClientCodec Codec { get; set; } = ClientCodec.Instance;
        private List<string> urilist = new List<string>();
        public List<string> Uris {
            get => urilist;
            set {
                if (value.Count > 0) {
                    var random = new Random(Guid.NewGuid().GetHashCode());
                    urilist = value.OrderBy(x => random.Next()).ToList();
                }
            }
        }
        private readonly HandlerManager handlerManager;
        Client() => handlerManager = new HandlerManager(Call, Transport);
        Client(string uri) : this() => urilist.Add(uri);
        Client(string[] uris) : this() => urilist.AddRange(uris);

        public Client Use(params InvokeHandler[] handlers) {
            handlerManager.Use(handlers);
            return this;
        }
        public Client Use(params IOHandler[] handlers) {
            handlerManager.Use(handlers);
            return this;
        }
        public Client Unuse(params InvokeHandler[] handlers) {
            handlerManager.Unuse(handlers);
            return this;
        }
        public Client Unuse(params IOHandler[] handlers) {
            handlerManager.Unuse(handlers);
            return this;
        }
        public async Task<T> Invoke<T>(string fullname, object[] args = null, Settings settings = null) {
            var context = new ClientContext(this, fullname, typeof(T), settings);
            return (T)(await handlerManager.InvokeHandler(fullname, args, context));
        }
        public async Task<object> Call(string fullname, object[] args, Context context) {
            var request = Codec.Encode(fullname, args, context as ClientContext);
            var response = await handlerManager.IOHandler(request, context);
            return Codec.Decode(response, context as ClientContext);
        }
        public abstract Task<Stream> Transport(Stream request, Context context);
    }
}
