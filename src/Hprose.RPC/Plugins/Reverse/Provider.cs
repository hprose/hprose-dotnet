/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Provider.cs                                             |
|                                                          |
|  Provider class for C#.                                  |
|                                                          |
|  LastModified: Feb 4, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Reverse {
    public class Provider {
        private readonly MethodManager methodManager = new MethodManager();
        private readonly InvokeManager invokeManager;
        public bool Debug { get; set; } = false;
        public Action<Exception> OnError { get; set; } = null;
        public Client Client { get; private set; }
        public string Id {
            get {
                if (((IDictionary<string, object>)Client.RequestHeaders).ContainsKey("Id")) {
                    return Client.RequestHeaders.Id.ToString();
                }
                throw new KeyNotFoundException("client unique id not found");
            }
            set {
                Client.RequestHeaders.Id = value;
            }
        }
        public Provider(Client client, string id = null) {
            invokeManager = new InvokeManager(Execute);
            Client = client;
            if (id != null && id.Length > 0) {
                Id = id;
            }
            Add(methodManager.GetNames, "~");
        }
        public async Task<object> Execute(string fullname, object[] args, Context context) {
            var method = (context as ProviderContext).Method;
            var result = method.MethodInfo.Invoke(
                method.Target,
                method.Missing ?
                method.Parameters.Length == 3 ?
                new object[] { fullname, args, context } :
                new object[] { fullname, args } :
                args
            );
            if (result is Task) {
                return await TaskResult.Get((Task)result);
            }
            return result;
        }
        private async Task<Tuple<int, object, string>> Process((int index, string name, object[] args) call) {
            var (index, name, args) = call;
            var method = Get(name, args.Length);
            try {
                if (method == null) {
                    throw new Exception("Can't find this method " + name + "().");
                }
                var context = new ProviderContext(Client, method);
                if (!method.Missing) {
                    var count = args.Length;
                    var parameters = method.Parameters;
                    var n = parameters.Length;
                    var arguments = new object[n];
                    var autoParams = 0;
                    for (int i = 0; i < n; ++i) {
                        var paramType = parameters[i].ParameterType;
                        if (typeof(Context).IsAssignableFrom(paramType)) {
                            autoParams = 1;
                            arguments[i] = context;
                        }
                        else if (i - autoParams < count) {
                            var argument = args[i - autoParams];
                            if (paramType.IsAssignableFrom(argument.GetType())) {
                                arguments[i] = argument;
                            }
                            else {
                                arguments[i] = Formatter.Deserialize(Formatter.Serialize(argument), paramType);
                            }
                        }
                        else {
                            arguments[i] = parameters[i].RawDefaultValue;
                        }
                    }
                    args = arguments;
                }
                var result = await invokeManager.Handler(name, args, context);
                return new Tuple<int, object, string>(index, result, null);
            }
            catch (Exception e) {
                return new Tuple<int, object, string>(index, null, Debug ? e.ToString() : e.Message);
            }
        }
        private async void Dispatch((int index, string name, object[] args)[] calls) {
            var n = calls.Length;
            var results = new Task<Tuple<int, object, string>>[n];
            for (int i = 0; i < n; ++i) {
                results[i] = Process(calls[i]);
            }
            try {
#if NET40
                await Client.Invoke("=", await TaskEx.WhenAll(results));
#else
                await Client.Invoke("=", await Task.WhenAll(results));
#endif
            }
            catch (Exception e) {
                OnError?.Invoke(e);
            }
        }
        public async Task Listen() {
            while(true) {
                try {
                    var calls = await Client.Invoke<(int index, string name, object[] args)[]>("!");
                    if (calls == null) return;
                    Dispatch(calls);
                }
                catch (Exception e) {
                    OnError?.Invoke(e);
                }
            }
        }
        public Provider Use(params InvokeHandler[] handlers) {
            invokeManager.Use(handlers);
            return this;
        }
        public Provider Unuse(params InvokeHandler[] handlers) {
            invokeManager.Unuse(handlers);
            return this;
        }
        public Method Get(string fullname, int paramCount) {
            return methodManager.Get(fullname, paramCount);
        }
        public Provider Remove(string fullname, int paramCount = -1) {
            methodManager.Remove(fullname, paramCount);
            return this;
        }
        public Provider Add(Method method) {
            methodManager.Add(method);
            return this;
        }
        public Provider Add(MethodInfo methodInfo, string fullname, object target = null) {
            methodManager.Add(methodInfo, fullname, target);
            return this;
        }
        public Provider Add(Action action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Provider Add<T>(Action<T> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Provider Add<T1, T2>(Action<T1, T2> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3>(Action<T1, T2, T3> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, string fullname = null) {
            methodManager.Add(action, fullname);
            return this;
        }
        public Provider Add<TResult>(Func<TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Provider Add<T1, TResult>(Func<T1, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Provider Add<T1, T2, TResult>(Func<T1, T2, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, string fullname = null) {
            methodManager.Add(func, fullname);
            return this;
        }
        public Provider AddMethod(string name, object target, string fullname = "") {
            methodManager.AddMethod(name, target, fullname);
            return this;
        }
        public Provider AddMethod(string name, Type type, string fullname = "") {
            methodManager.AddMethod(name, type, fullname);
            return this;
        }
        public Provider AddMethods(string[] names, object target, string ns = "") {
            methodManager.AddMethods(names, target, ns);
            return this;
        }
        public Provider AddMethods(string[] names, Type type, string ns = "") {
            methodManager.AddMethods(names, type, ns);
            return this;
        }
        public Provider AddInstanceMethods(object target, string ns = "") {
            methodManager.AddInstanceMethods(target, ns);
            return this;
        }
        public Provider AddStaticMethods(Type type, string ns = "") {
            methodManager.AddStaticMethods(type, ns);
            return this;
        }
        public Provider AddMissingMethod(Func<string, object[], Task<object>> method) {
            methodManager.AddMissingMethod(method);
            return this;
        }
        public Provider AddMissingMethod(Func<string, object[], object> method) {
            methodManager.AddMissingMethod(method);
            return this;
        }
        public Provider AddMissingMethod(Func<string, object[], Context, Task<object>> method) {
            methodManager.AddMissingMethod(method);
            return this;
        }
        public Provider AddMissingMethod(Func<string, object[], Context, object> method) {
            methodManager.AddMissingMethod(method);
            return this;
        }

    }
}