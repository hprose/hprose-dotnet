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
|  LastModified: Jan 27, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public interface ITransport {
        Task<Stream> Transport(Stream request, Context context);
        void Abort();
    }
    public class Result<T> {
        public T Value { get; private set; }
        public Context Context { get; private set; }
        public Result(T value, Context context) {
            Value = value;
            Context = context;
        }
    }
    public partial class Client {
        private static readonly List<ValueTuple<string, Type>> transTypes = new List<ValueTuple<string, Type>>();
        private static readonly Dictionary<string, string> schemes = new Dictionary<string, string>();
        public static void Register<T>(string name, string[] schemes) where T: ITransport, new() {
            transTypes.Add((name, typeof(T)));
            for (int i = 0, n = schemes.Length; i < n; ++i) {
                var scheme = schemes[i];
                Client.schemes[scheme] = name;
            }
        }
        private readonly Dictionary<string, ITransport> transports = new Dictionary<string, ITransport>();
        public ITransport this[string name] => transports[name];
        public ExpandoObject RequestHeaders { get; set; } = new ExpandoObject();
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
        public Client() {
            handlerManager = new HandlerManager(Call, Transport);
            for (int i = 0, n = transTypes.Count; i < n; ++i) {
                var (name, type) = transTypes[i];
                transports[name] = (ITransport)Activator.CreateInstance(type);
            };
        }
        public Client(string uri) : this() => urilist.Add(uri);
        public Client(string[] uris) : this() => urilist.AddRange(uris);
        public T UseService<T>(string ns = "") {
            Type type = typeof(T);
            InvocationHandler handler = new InvocationHandler(this, ns);
            if (type.IsInterface) {
                return (T)Proxy.NewInstance(new Type[] { type }, handler);
            }
            else {
                return (T)Proxy.NewInstance(type.GetInterfaces(), handler);
            }
        }
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
            Type type = typeof(T);
            bool isResultType = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Result<>);
            if (isResultType) {
                type = type.GetGenericArguments()[0];
            }
            var context = new ClientContext(this, fullname, type, settings);
            var result = await handlerManager.InvokeHandler(fullname, args, context);
            if (isResultType) {
                return (T)Activator.CreateInstance(typeof(T), new object[] { result, context });
            }
            return (T)result;
        }
        public async Task<object> Call(string fullname, object[] args, Context context) {
            var request = Codec.Encode(fullname, args, context as ClientContext);
            var response = await handlerManager.IOHandler(request, context);
            return await Codec.Decode(response, context as ClientContext);
        }
        public async Task<Stream> Transport(Stream request, Context context) {
            var uri = new Uri((context as ClientContext).Uri);
            var scheme = uri.Scheme;
            if (schemes.ContainsKey(scheme)) {
                return await transports[schemes[scheme]].Transport(request, context);
            }
            throw new NotSupportedException("The protocol " + scheme + " is not supported.");
        }
        public void Abort() {
            foreach (var pair in transports) {
                pair.Value.Abort();
            }
        }
    }
}
