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
|  LastModified: Feb 27, 2019                              |
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
        Task Abort();
    }

    public class Client : IDisposable {
        private static readonly object[] emptyArgs = new object[0];
        private static readonly Random random = new Random(Guid.NewGuid().GetHashCode());
        private static readonly List<(string, Type)> transTypes = new List<(string, Type)>();
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
        private readonly Dictionary<string, ITransport> transports = new Dictionary<string, ITransport>();
        public ITransport this[string name] => transports[name];
        public IDictionary<string, object> RequestHeaders { get; } = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        public IClientCodec Codec { get; set; } = ClientCodec.Instance;
        private List<Uri> urilist = new List<Uri>();
        public List<Uri> Uris {
            get => urilist;
            set {
                if (value.Count > 0) {
                    urilist = value.OrderBy(x => random.Next()).ToList();
                }
            }
        }
        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 30);
        private readonly InvokeManager invokeManager;
        private readonly IOManager ioManager;
        public Client() {
            invokeManager = new InvokeManager(Call);
            ioManager = new IOManager(Transport);
            foreach (var (name, type) in transTypes) {
                transports[name] = (ITransport)Activator.CreateInstance(type);
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
        public void Invoke(string fullname, object[] args = null, ClientContext context = null) {
            InvokeAsync<object>(fullname, args, context).Wait();
        }
        public T Invoke<T>(string fullname, object[] args = null, ClientContext context = null) {
            return InvokeAsync<T>(fullname, args, context).Result;
        }
        public Task InvokeAsync(string fullname, object[] args = null, ClientContext context = null) {
            return InvokeAsync<object>(fullname, args, context);
        }
        public async Task<T> InvokeAsync<T>(string fullname, object[] args = null, ClientContext context = null) {
            if (args == null) args = emptyArgs;
            if (context == null) context = new ClientContext();
            context.Init(this, typeof(T));
            var task = invokeManager.Handler(fullname, args, context);
#if !NET35_CF
            await task.ContinueWith((_) => { }, TaskScheduler.Current).ConfigureAwait(false);
#else
            await task.ContinueWith((_) => { }).ConfigureAwait(false);
#endif
            return (T)(await task.ConfigureAwait(false));
        }
        public async Task<object> Call(string fullname, object[] args, Context context) {
            var request = Codec.Encode(fullname, args, context as ClientContext);
            var response = await ioManager.Handler(request, context).ConfigureAwait(false);
            return await Codec.Decode(response, context as ClientContext).ConfigureAwait(false);
        }
        public async Task<Stream> Transport(Stream request, Context context) {
            var uri = (context as ClientContext).Uri;
            var scheme = uri.Scheme;
            if (schemes.TryGetValue(scheme, out string name)) {
                return await transports[name].Transport(request, context).ConfigureAwait(false);
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
            await TaskEx.WhenAll(tasks);
#else
            await Task.WhenAll(tasks);
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
