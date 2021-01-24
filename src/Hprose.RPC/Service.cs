/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Service.cs                                              |
|                                                          |
|  Service class for C#.                                   |
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
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public interface IHandler<T> {
        Task Bind(T server);
    }
    public class Service : IDisposable {
        private readonly static ConcurrentDictionary<string, Type> handlerTypes = new ConcurrentDictionary<string, Type>();
        private readonly static ConcurrentDictionary<Type, string> serverTypes = new ConcurrentDictionary<Type, string>();
        public static void Register<T, S>(string name) where T : IHandler<S> {
            handlerTypes[name] = typeof(T);
            serverTypes[typeof(S)] = name;
        }
        static Service() {
            Register<MockHandler, MockServer>("mock");
#if !NET35_CF
#if !NET40
            Register<WebSocketHandler, HttpListener>("http");
#else
            Register<HttpHandler, HttpListener>("http");
#endif
#endif
            Register<TcpHandler, TcpListener>("tcp");
            Register<UdpHandler, UdpClient>("udp");
#if !NET35_CF && !NET40 && !NET45 && !NET451 && !NET452 && !NET46 && !NET461 && !NET462 && !NET47
            Register<SocketHandler, Socket>("socket");
#endif
        }
        public MockHandler Mock => (MockHandler)this["mock"];
#if !NET35_CF
#if !NET40
        public WebSocketHandler Http => (WebSocketHandler)this["http"];
#else
        public HttpHandler Http => (HttpHandler)this["http"];
#endif
#endif
        public TcpHandler Tcp => (TcpHandler)this["tcp"];
        public UdpHandler Udp => (UdpHandler)this["udp"];
#if !NET35_CF && !NET40 && !NET45 && !NET451 && !NET452 && !NET46 && !NET461 && !NET462 && !NET47
        public SocketHandler Socket => (SocketHandler)this["socket"];
#endif
        public IServiceCodec Codec { get; set; } = ServiceCodec.Instance;
        public int MaxRequestLength { get; set; } = 0x7FFFFFFF;
        private readonly InvokeManager invokeManager;
        private readonly IOManager ioManager;
        private readonly MethodManager methodManager = new MethodManager();
        private readonly ConcurrentDictionary<string, object> handlers = new ConcurrentDictionary<string, object>();
        public object this[string name] => handlers[name];
        public List<string> Names => new List<string>(methodManager.GetNames());
        public IDictionary<string, object> Options { get; private set; } = new ConcurrentDictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        public Service() {
            invokeManager = new InvokeManager(Execute);
            ioManager = new IOManager(Process);
            foreach (var pair in handlerTypes) {
#if !NET35_CF
                var handler = Activator.CreateInstance(pair.Value, new object[] { this });
#else
                var handler = pair.Value.GetConstructor(new Type[] { typeof(Service) }).Invoke(new object[] { this });
#endif
                handlers[pair.Key] = handler;
            }
            Add(methodManager.GetNames, "~");
        }
        public Service Bind<T>(T server) {
            if (serverTypes.TryGetValue(typeof(T), out var name)) {
                var handler = handlers[name];
                var bindMethod = handler.GetType().GetMethod("Bind");
                bindMethod.Invoke(handler, new object[] { server });
            }
            else {
                throw new NotSupportedException("This type server is not supported.");
            }
            return this;
        }
        public Task<Stream> Handle(Stream request, Context context) => ioManager.Handler(request, context);
        public async Task<Stream> Process(Stream request, Context context) {
            object result;
            try {
                var stream = await request.ToMemoryStream().ConfigureAwait(false);
                var (name, args) = Codec.Decode(stream, context as ServiceContext);
                result = await invokeManager.Handler(name, args, context).ConfigureAwait(false);
            }
            catch (Exception e) {
                result = e.InnerException ?? e;
            }
            return Codec.Encode(result, context as ServiceContext);
        }

        public static async Task<object> Execute(string name, object[] args, Context context) {
            var method = (context as ServiceContext).Method;
            if (method.Missing) {
                args = method.PassContext ? new object[] { name, args, context } : new object[] { name, args };
            }
            else if (method.PassContext) {
                var arglist = args.ToList();
                arglist.Add(context);
                args = arglist.ToArray();
            }
            var result = method.MethodInfo.Invoke(method.Target, args);
            if (result is Task task) {
                return await TaskResult.Get(task).ConfigureAwait(false);
            }
            return result;
        }
        public Service Use(params InvokeHandler[] handlers) {
            invokeManager.Use(handlers);
            return this;
        }
        public Service Use(params IOHandler[] handlers) {
            ioManager.Use(handlers);
            return this;
        }
        public Service Unuse(params InvokeHandler[] handlers) {
            invokeManager.Unuse(handlers);
            return this;
        }
        public Service Unuse(params IOHandler[] handlers) {
            ioManager.Unuse(handlers);
            return this;
        }
        public Method Get(string name, int paramCount) => methodManager.Get(name, paramCount);
        public Service Remove(string name, int paramCount = -1) {
            methodManager.Remove(name, paramCount);
            return this;
        }
        public Service Add(Method method) {
            methodManager.Add(method);
            return this;
        }
        public Service Add(MethodInfo methodInfo, string name, object target = null) {
            methodManager.Add(methodInfo, name, target);
            return this;
        }
        public Service Add(Action action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Service Add<T>(Action<T> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Service Add<T1, T2>(Action<T1, T2> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Service Add<T1, T2, T3>(Action<T1, T2, T3> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Service Add<TResult>(Func<TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Service Add<T1, TResult>(Func<T1, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Service Add<T1, T2, TResult>(Func<T1, T2, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Service Add<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Service AddMethod(string name, object target, string alias = "") {
            methodManager.AddMethod(name, target, alias);
            return this;
        }
        public Service AddMethod(string name, Type type, string alias = "") {
            methodManager.AddMethod(name, type, alias);
            return this;
        }
        public Service AddMethods(string[] names, object target, string ns = "") {
            methodManager.AddMethods(names, target, ns);
            return this;
        }
        public Service AddMethods(string[] names, Type type, string ns = "") {
            methodManager.AddMethods(names, type, ns);
            return this;
        }
        public Service AddInstanceMethods(object target, string ns = "") {
            methodManager.AddInstanceMethods(target, ns);
            return this;
        }
        public Service AddStaticMethods(Type type, string ns = "") {
            methodManager.AddStaticMethods(type, ns);
            return this;
        }
        public Service AddMissingMethod(Func<string, object[], Task<object>> method) {
            methodManager.AddMissingMethod(method);
            return this;
        }
        public Service AddMissingMethod(Func<string, object[], object> method) {
            methodManager.AddMissingMethod(method);
            return this;
        }
        public Service AddMissingMethod(Func<string, object[], Context, Task<object>> method) {
            methodManager.AddMissingMethod(method);
            return this;
        }
        public Service AddMissingMethod(Func<string, object[], Context, object> method) {
            methodManager.AddMissingMethod(method);
            return this;
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