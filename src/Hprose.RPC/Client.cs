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
|  LastModified: Jan 24, 2021                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public interface ITransport {
        Task<Stream> Transport(Stream request, Context context);
        Task Abort();
    }

    public class Client : IDisposable {
        private static readonly object[] emptyArgs = new object[0];
        private static readonly Random random = new(Guid.NewGuid().GetHashCode());
        private static readonly ConcurrentDictionary<string, Type> transTypes = new();
        private static readonly ConcurrentDictionary<string, string> schemes = new();
        public static void Register<T>(string name) where T : ITransport, new() {
            var type = typeof(T);
            var schemes = type.GetProperty("Schemes", BindingFlags.Public | BindingFlags.Static).GetValue(type, null) as string[];
            transTypes[name] = type;
            foreach (var scheme in schemes) {
                Client.schemes[scheme] = name;
            }
        }
        static Client() {
            Register<MockTransport>("mock");
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
        public MockTransport Mock => (MockTransport)this["mock"];
        public HttpTransport Http => (HttpTransport)this["http"];
        public TcpTransport Tcp => (TcpTransport)this["tcp"];
        public UdpTransport Udp => (UdpTransport)this["udp"];
#if !NET35_CF && !NET40 && !NET45 && !NET451 && !NET452 && !NET46 && !NET461 && !NET462 && !NET47
        public SocketTransport Socket => (SocketTransport)this["socket"];
#endif
#if !NET35_CF && !NET40
        public WebSocketTransport WebSocket => (WebSocketTransport)this["websocket"];
#endif
        private readonly ConcurrentDictionary<string, ITransport> transports = new();
        public ITransport this[string name] => transports[name];
        public IDictionary<string, object> RequestHeaders { get; } = new ConcurrentDictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        public IClientCodec Codec { get; set; } = ClientCodec.Instance;
        private List<Uri> urilist = new();
        public List<Uri> Uris {
            get => urilist;
            set {
                if (value.Count > 0) {
                    urilist = value.OrderBy(x => random.Next()).ToList();
                }
            }
        }
        public TimeSpan Timeout { get; set; } = new(0, 0, 30);
        private readonly InvokeManager invokeManager;
        private readonly IOManager ioManager;
        public Client() {
            invokeManager = new InvokeManager(Call);
            ioManager = new IOManager(Transport);
            foreach (var entry in transTypes) {
                transports[entry.Key] = (ITransport)Activator.CreateInstance(entry.Value);
            };
        }
        public Client(string uri) : this() {
            if (uri == null) {
                throw new ArgumentNullException(nameof(uri));
            }
            if (uri.Length > 0) {
                urilist.Add(new Uri(uri));
            }
        }
        public Client(Uri uri) : this() {
            if (uri == null) {
                throw new ArgumentNullException(nameof(uri));
            }
            urilist.Add(uri);
        }
        public Client(string[] uris) : this() {
            if (uris == null) {
                throw new ArgumentNullException(nameof(uris));
            }
            urilist.AddRange(uris.Select((uri) => new Uri(uri)));
        }
        public Client(Uri[] uris) : this() {
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
        public void Invoke(string name, object[] args = null, ClientContext context = null) {
            InvokeAsync<object>(name, args, context).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public T Invoke<T>(string name, object[] args = null, ClientContext context = null) {
            return InvokeAsync<T>(name, args, context).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public Task InvokeAsync(string name, object[] args = null, ClientContext context = null) {
            return InvokeAsync<object>(name, args, context);
        }
        public async Task<T> InvokeAsync<T>(string name, object[] args = null, ClientContext context = null) {
            if (args == null) args = emptyArgs;
            for (int i = 0; i < args.Length; ++i) {
                if (args[i] is Task) {
                    args[i] = await TaskResult.Get(args[i] as Task).ConfigureAwait(false);
                }
            }
            if (context == null) context = new ClientContext();
            context.Init(this, typeof(T));
            var task = invokeManager.Handler(name, args, context);
            await task.ContinueWith((_) => { }, TaskScheduler.Current).ConfigureAwait(false);
            return (T)(await task.ConfigureAwait(false));
        }
        public async Task<object> Call(string name, object[] args, Context context) {
            var request = Codec.Encode(name, args, context as ClientContext);
            Stream response = await Request(request, context).ConfigureAwait(false);
            MemoryStream stream = await response.ToMemoryStream().ConfigureAwait(false);
            return Codec.Decode(stream, context as ClientContext);
        }
        public Task<Stream> Request(Stream request, Context context) {
            return ioManager.Handler(request, context);
        }
        public Task<Stream> Transport(Stream request, Context context) {
            var uri = (context as ClientContext).Uri;
            var scheme = uri.Scheme;
            if (schemes.TryGetValue(scheme, out string name)) {
                return transports[name].Transport(request, context);
            }
            throw new NotSupportedException("The protocol " + scheme + " is not supported.");
        }
        public async Task Abort() {
            var tasks = new Task[transports.Count];
            var i = 0;
            foreach (var pair in transports) {
                tasks[i++] = pair.Value.Abort();
            }
#if NET40
            await TaskEx.WhenAll(tasks).ConfigureAwait(false);
#else
            await Task.WhenAll(tasks).ConfigureAwait(false);
#endif
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
