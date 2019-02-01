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
|  LastModified: Jan 28, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public interface IHandler<T> {
        Task Bind(T server);
    }
    public partial class Service {
        private readonly static List<(string, Type)> handlerTypes = new List<(string, Type)>();
        private readonly static Dictionary<Type, List<string>> serverTypes = new Dictionary<Type, List<string>>();
        public static void Register<HT, ST>(string name) where HT: IHandler<ST> {
            handlerTypes.Add((name, typeof(HT)));
            if (serverTypes.ContainsKey(typeof(ST))) {
                serverTypes[typeof(ST)].Add(name);
            }
            else {
                serverTypes[typeof(ST)] = new List<string>(new string[] { name });
            }
        }
        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 30);
        public IServiceCodec Codec { get; set; } = ServiceCodec.Instance;
        public int MaxRequestLength { get; set; } = 0x7FFFFFFF;
        private readonly HandlerManager handlerManager;
        private readonly MethodManager methodManager = new MethodManager();
        private readonly Dictionary<string, object> handlers = new Dictionary<string, object>();
        public object this[string name] => handlers[name];
        public Service() {
            handlerManager = new HandlerManager(Execute, Process);
            foreach (var (name, type) in handlerTypes) {
                var handler = Activator.CreateInstance(type, new object[] { this });
                handlers[name] = handler;
            }
            AddMethod("GetNames", methodManager, "~");
        }
        public void Bind<T>(T server, string name = null) {
            var serverType = typeof(T);
            if (serverTypes.ContainsKey(serverType)) {
                var names = serverTypes[serverType];
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
        }
        public async Task<Stream> Handle(Stream request, Context context) {
            var result = handlerManager.IOHandler(request, context);
            if (Timeout.Ticks > 0) {
#if NET40
                var timer = TaskEx.Delay(Timeout);
                var task = await TaskEx.WhenAny(result, timer);
#else
                var timer = Task.Delay(Timeout);
                var task = await Task.WhenAny(result, timer);
#endif
                if (task == timer) {
                    throw new TimeoutException();
                }
            }
            return await result;
        }
        public async Task<Stream> Process(Stream request, Context context) {
            object result;
            try {
                var (fullname, args) = await Codec.Decode(request, context as ServiceContext);
                result = await handlerManager.InvokeHandler(fullname, args, context);
            }
            catch(Exception e) {
                result = e;
            }
            return Codec.Encode(result, context as ServiceContext);
        }
        public async Task<object> Execute(string fullname, object[] args, Context context) {
            var method = (context as ServiceContext).Method;
            object result;
            if (method.Missing) {
                result = method.MethodInfo.Invoke(method.Target, new object[] { fullname, args });
            }
            else {
                result = method.MethodInfo.Invoke(method.Target, args);
            }
            if (result is Task) {
                return await TaskResult.Get((Task)result);
            }
            return result;
        }
        public Service Use(params InvokeHandler[] handlers) {
            handlerManager.Use(handlers);
            return this;
        }
        public Service Use(params IOHandler[] handlers) {
            handlerManager.Use(handlers);
            return this;
        }
        public Service Unuse(params InvokeHandler[] handlers) {
            handlerManager.Unuse(handlers);
            return this;
        }
        public Service Unuse(params IOHandler[] handlers) {
            handlerManager.Unuse(handlers);
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
    }
}