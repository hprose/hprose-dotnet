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
|  LastModified: Feb 9, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public interface IHandler<T> {
        Task Bind(T server);
    }
    public class Service : IDisposable {
        private readonly static List<(string, Type)> handlerTypes = new List<(string, Type)>();
        private readonly static ConcurrentDictionary<Type, HashSet<string>> serverTypes = new ConcurrentDictionary<Type, HashSet<string>>();
        public static void Register<T, S>(string name) where T : IHandler<S> {
            handlerTypes.Add((name, typeof(T)));
            serverTypes.GetOrAdd(typeof(S), new HashSet<string>()).Add(name);
        }
        static Service() {
            Register<HttpHandler, HttpListener>("http");
            Register<TcpHandler, TcpListener>("tcp");
            Register<UdpHandler, UdpClient>("udp");
#if !NET40 && !NET45 && !NET451 && !NET452 && !NET46 && !NET461 && !NET462 && !NET47
            Register<TcpSocketHandler, Socket>("tcpsocket");
#endif
        }
        public HttpHandler Http => (HttpHandler)this["http"];
        public TcpHandler Tcp => (TcpHandler)this["tcp"];
        public UdpHandler Udp => (UdpHandler)this["udp"];
#if !NET40 && !NET45 && !NET451 && !NET452 && !NET46 && !NET461 && !NET462 && !NET47
        public TcpSocketHandler TcpSocket => (TcpSocketHandler)this["tcpsocket"];
#endif
        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 30);
        public IServiceCodec Codec { get; set; } = ServiceCodec.Instance;
        public int MaxRequestLength { get; set; } = 0x7FFFFFFF;
        private readonly InvokeManager invokeManager;
        private readonly IOManager ioManager;
        private readonly MethodManager methodManager = new MethodManager();
        private readonly Dictionary<string, object> handlers = new Dictionary<string, object>();
        public object this[string name] => handlers[name];
        public Service() {
            invokeManager = new InvokeManager(Execute);
            ioManager = new IOManager(Process);
            foreach (var (name, type) in handlerTypes) {
                var handler = Activator.CreateInstance(type, new object[] { this });
                handlers[name] = handler;
            }
            Add(methodManager.GetNames, "~");
        }
        public Service Bind<T>(T server, string name = null) {
            if (serverTypes.TryGetValue(typeof(T), out var names)) {
                foreach (var n in names) {
                    if (name == null || name == n) {
                        var handler = handlers[n];
                        var bindMethod = handler.GetType().GetMethod("Bind");
                        bindMethod.Invoke(handler, new object[] { server });
                    }
                }
            }
            else {
                throw new NotSupportedException("This type server is not supported.");
            }
            return this;
        }
        public async Task<Stream> Handle(Stream request, Context context) {
            var result = ioManager.Handler(request, context);
            if (Timeout > TimeSpan.Zero) {
                using (CancellationTokenSource source = new CancellationTokenSource()) {

#if NET40
                    var timer = TaskEx.Delay(Timeout, source.Token);
                    var task = await TaskEx.WhenAny(result, timer).ConfigureAwait(false);
#else
                    var timer = Task.Delay(Timeout, source.Token);
                    var task = await Task.WhenAny(result, timer).ConfigureAwait(false);
#endif
                    source.Cancel();
                    if (task == timer) {
                        throw new TimeoutException();
                    }
                }
            }
            return await result.ConfigureAwait(false);
        }
        public async Task<Stream> Process(Stream request, Context context) {
            object result;
            try {
                var (fullname, args) = await Codec.Decode(request, context as ServiceContext).ConfigureAwait(false);
                result = await invokeManager.Handler(fullname, args, context).ConfigureAwait(false);
            }
            catch (Exception e) {
                result = e.InnerException ?? e;
            }
            return Codec.Encode(result, context as ServiceContext);
        }
        public static async Task<object> Execute(string fullname, object[] args, Context context) {
            var method = (context as ServiceContext).Method;
            var result = method.MethodInfo.Invoke(
                method.Target,
                method.Missing ? 
                method.Parameters.Length == 3 ?
                new object[] { fullname, args, context } :
                new object[] { fullname, args } :
                args
            );
            if (result is Task) {
                return await TaskResult.Get((Task)result).ConfigureAwait(false);
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
        public Method Get(string fullname, int paramCount) => methodManager.Get(fullname, paramCount);
        public Service Remove(string fullname, int paramCount = -1) {
            methodManager.Remove(fullname, paramCount);
            return this;
        }
        public Service Add(Method method) {
            methodManager.Add(method);
            return this;
        }
        public Service Add(MethodInfo methodInfo, string fullname, object target = null) {
            methodManager.Add(methodInfo, fullname, target);
            return this;
        }
        public Service Add(Action action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Service Add<T>(Action<T> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Service Add<T1, T2>(Action<T1, T2> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Service Add<T1, T2, T3>(Action<T1, T2, T3> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Service Add<TResult>(Func<TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Service Add<T1, TResult>(Func<T1, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Service Add<T1, T2, TResult>(Func<T1, T2, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Service Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Service AddMethod(string name, object target, string fullname = "") {
            methodManager.AddMethod(name, target, fullname);
            return this;
        }
        public Service AddMethod(string name, Type type, string fullname = "") {
            methodManager.AddMethod(name, type, fullname);
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