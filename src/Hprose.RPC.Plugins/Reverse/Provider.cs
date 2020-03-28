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
|  LastModified: Mar 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Reverse {
    public class Provider : IDisposable {
        private volatile int closed = 1;
        private readonly MethodManager methodManager = new MethodManager();
        private readonly InvokeManager invokeManager;
        public bool Debug { get; set; } = false;
        public event Action<Exception> OnError;
        public Client Client { get; private set; }
        public string Id {
            get {
                return GetId();
            }
            set {
                Client.RequestHeaders["id"] = value;
            }
        }
        private string GetId() {
            if (Client.RequestHeaders.ContainsKey("id")) {
                return Client.RequestHeaders["id"].ToString();
            }
            throw new KeyNotFoundException("Client unique id not found");
        }
        public Provider(Client client, string id = null) {
            invokeManager = new InvokeManager(Execute);
            Client = client;
            if (id != null && id.Length > 0) {
                Id = id;
            }
            Add(methodManager.GetNames, "~");
        }
        public static async Task<object> Execute(string name, object[] args, Context context) {
            var method = (context as ProviderContext).Method;
            var result = method.MethodInfo.Invoke(
                method.Target,
                method.Missing ?
                method.Parameters.Length == 3 ?
                new object[] { name, args, context } :
                new object[] { name, args } :
                args
            );
            if (result is Task) {
                return await TaskResult.Get((Task)result).ConfigureAwait(false);
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
                            arguments[i] = parameters[i].DefaultValue;
                        }
                    }
                    args = arguments;
                }
                var result = await invokeManager.Handler(name, args, context).ConfigureAwait(false);
                return new Tuple<int, object, string>(index, result, null);
            }
            catch (Exception e) {
                e = e.InnerException ?? e;
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
                await Client.InvokeAsync("=", new object[] { await TaskEx.WhenAll(results).ConfigureAwait(false) }).ConfigureAwait(false);
#else
                await Client.InvokeAsync("=", new object[] { await Task.WhenAll(results).ConfigureAwait(false) }).ConfigureAwait(false);
#endif
            }
            catch (Exception e) {
                OnError?.Invoke(e);
            }
        }
        public async Task Listen() {
            Interlocked.Exchange(ref closed, 0);
            while(closed == 0) {
                try {
                    var calls = await Client.InvokeAsync<(int index, string name, object[] args)[]>("!").ConfigureAwait(false);
                    if (calls == null) return;
                    Dispatch(calls);
                }
                catch (Exception e) {
                    OnError?.Invoke(e);
                }
            }
        }
        public async Task Close() {
            Interlocked.Exchange(ref closed, 1);
            await Client.InvokeAsync("!!").ConfigureAwait(false);
        }
        public Provider Use(params InvokeHandler[] handlers) {
            invokeManager.Use(handlers);
            return this;
        }
        public Provider Unuse(params InvokeHandler[] handlers) {
            invokeManager.Unuse(handlers);
            return this;
        }
        public Method Get(string name, int paramCount) {
            return methodManager.Get(name, paramCount);
        }
        public Provider Remove(string name, int paramCount = -1) {
            methodManager.Remove(name, paramCount);
            return this;
        }
        public Provider Add(Method method) {
            methodManager.Add(method);
            return this;
        }
        public Provider Add(MethodInfo methodInfo, string name, object target = null) {
            methodManager.Add(methodInfo, name, target);
            return this;
        }
        public Provider Add(Action action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Provider Add<T>(Action<T> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Provider Add<T1, T2>(Action<T1, T2> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Provider Add<T1, T2, T3>(Action<T1, T2, T3> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, string name = null) {
            methodManager.Add(action, name);
            return this;
        }
        public Provider Add<TResult>(Func<TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Provider Add<T1, TResult>(Func<T1, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Provider Add<T1, T2, TResult>(Func<T1, T2, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Provider Add<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Provider Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, string name = null) {
            methodManager.Add(func, name);
            return this;
        }
        public Provider AddMethod(string name, object target, string alias = "") {
            methodManager.AddMethod(name, target, alias);
            return this;
        }
        public Provider AddMethod(string name, Type type, string alias = "") {
            methodManager.AddMethod(name, type, alias);
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
        private bool disposed = false;
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing) {
            if (disposed) return;
            if (disposing) {
                invokeManager.Dispose();
            }
            disposed = true;
        }
    }
}