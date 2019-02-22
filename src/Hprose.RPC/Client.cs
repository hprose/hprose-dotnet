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
|  LastModified: Feb 23, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
    public class Client : IDisposable {
        private static readonly Random random = new Random(Guid.NewGuid().GetHashCode());
        private static readonly List<ValueTuple<string, Type>> transTypes = new List<ValueTuple<string, Type>>();
        private static readonly Dictionary<string, string> schemes = new Dictionary<string, string>();
        public static void Register<T>(string name) where T : ITransport, new() {
            var type = typeof(T);
            var schemes = type.GetProperty("Schemes", BindingFlags.Public | BindingFlags.Static).GetValue(type, null) as string[];
            transTypes.Add((name, type));
            foreach (var scheme in schemes) {
                Client.schemes[scheme] = name;
            }
        }
        static Client() {
            Register<HttpTransport>("http");
            Register<TcpTransport>("tcp");
            Register<UdpTransport>("udp");
#if !NET35_CF && !NET40 && !NET45 && !NET451 && !NET452 && !NET46 && !NET461 && !NET462 && !NET47
            Register<SocketTransport>("socket");
#endif
#if !NET35_CF && !NET40
            Register<WebSocketTransport>("websocket");
#endif
        }
        public HttpTransport Http => (HttpTransport)this["http"];
        public TcpTransport Tcp => (TcpTransport)this["tcp"];
        public UdpTransport Udp => (UdpTransport)this["udp"];
#if !NET35_CF && !NET40 && !NET45 && !NET451 && !NET452 && !NET46 && !NET461 && !NET462 && !NET47
        public SocketTransport Socket => (SocketTransport)this["socket"];
#endif
#if !NET35_CF && !NET40
        public WebSocketTransport WebSocket => (WebSocketTransport)this["websocket"];
#endif
        private readonly Dictionary<string, ITransport> transports = new Dictionary<string, ITransport>();
        public ITransport this[string name] => transports[name];
        public IDictionary<string, object> RequestHeaders { get; } = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        public IClientCodec Codec { get; set; } = ClientCodec.Instance;
        private List<string> urilist = new List<string>();
        public List<string> Uris {
            get => urilist;
            set {
                if (value.Count > 0) {
                    urilist = value.OrderBy(x => random.Next()).ToList();
                }
            }
        }
        private readonly InvokeManager invokeManager;
        private readonly IOManager ioManager;
        public Client() {
            invokeManager = new InvokeManager(Call);
            ioManager = new IOManager(Transport);
            for (int i = 0, n = transTypes.Count; i < n; ++i) {
                var (name, type) = transTypes[i];
                transports[name] = (ITransport)Activator.CreateInstance(type);
            };
        }
        public Client(string uri) : this() {
#if !NET35_CF
            if (string.IsNullOrWhiteSpace(uri)) {
#else
            if (String2.IsNullOrWhiteSpace(uri)) {
#endif
                throw new ArgumentException("invalid uri", nameof(uri));
            }
            urilist.Add(uri);
        }
        public Client(string[] uris) : this() {
            if (uris == null) {
                throw new ArgumentNullException(nameof(uris));
            }
            urilist.AddRange(uris);
        }
#if !NET35_CF
        public T UseService<T>(string ns = "") {
            var type = typeof(T);
            var handler = new InvocationHandler(this, ns);
            if (type.IsInterface) {
                return (T)Proxy.NewInstance(new Type[] { type }, handler);
            }
            else {
                return (T)Proxy.NewInstance(type.GetInterfaces(), handler);
            }
        }
#endif
        public Client Use(params InvokeHandler[] handlers) {
            invokeManager.Use(handlers);
            return this;
        }
        public Client Use(params IOHandler[] handlers) {
            ioManager.Use(handlers);
            return this;
        }
        public Client Unuse(params InvokeHandler[] handlers) {
            invokeManager.Unuse(handlers);
            return this;
        }
        public Client Unuse(params IOHandler[] handlers) {
            ioManager.Unuse(handlers);
            return this;
        }
        public void Invoke(string fullname, in object[] args = null, Settings settings = null) {
            var context = new ClientContext(this, null, settings);
            invokeManager.Handler(fullname, args, context).Wait();
            return;
        }
        public T Invoke<T>(string fullname, in object[] args = null, Settings settings = null) {
            return InvokeAsync<T>(fullname, args, settings).Result;
        }
        public async Task InvokeAsync(string fullname, object[] args = null, Settings settings = null) {
            var context = new ClientContext(this, null, settings);
            var task = invokeManager.Handler(fullname, args, context);
#if !NET35_CF
            await task.ContinueWith((_) => { }, TaskScheduler.Current).ConfigureAwait(false);
#else
            await task.ContinueWith((_) => { }).ConfigureAwait(false);
#endif
            return;
        }
        public async Task<T> InvokeAsync<T>(string fullname, object[] args = null, Settings settings = null) {
            var type = typeof(T);
            bool isResultType = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Result<>);
            if (isResultType) {
                type = type.GetGenericArguments()[0];
            }
            var context = new ClientContext(this, type, settings);
            var task = invokeManager.Handler(fullname, args, context);
#if !NET35_CF
            await task.ContinueWith((_) => { }, TaskScheduler.Current).ConfigureAwait(false);
#else
            await task.ContinueWith((_) => { }).ConfigureAwait(false);
#endif
            var result = await task.ConfigureAwait(false);
            if (isResultType) {
#if !NET35_CF
                return (T)Activator.CreateInstance(typeof(T), new object[] { result, context });
#else
                var ctor = typeof(T).GetConstructor(new Type[] { type, typeof(Context) });
                return (T)ctor.Invoke(new object[] { result, context });
#endif
            }
            return (T)result;
        }
        public async Task<object> Call(string fullname, object[] args, Context context) {
            var request = Codec.Encode(fullname, args, context as ClientContext);
            var response = await ioManager.Handler(request, context).ConfigureAwait(false);
            return await Codec.Decode(response, context as ClientContext).ConfigureAwait(false);
        }
        public async Task<Stream> Transport(Stream request, Context context) {
            var uri = new Uri((context as ClientContext).Uri);
            var scheme = uri.Scheme;
            if (schemes.TryGetValue(scheme, out string name)) {
                return await transports[name].Transport(request, context).ConfigureAwait(false);
            }
            throw new NotSupportedException("The protocol " + scheme + " is not supported.");
        }
        public void Abort() {
            foreach (var pair in transports) {
                pair.Value.Abort();
            }
        }
        private bool disposed = false;
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing) {
            if (disposed) return;
            if (disposing) {
                invokeManager.Dispose();
                ioManager.Dispose();
            }
            disposed = true;
        }
    }
}
