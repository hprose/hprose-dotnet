/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Caller.cs                                               |
|                                                          |
|  Caller class for C#.                                    |
|                                                          |
|  LastModified: Feb 4, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Reverse {
    public class Caller {
        private static readonly (int, string, object[])[] emptyCall = new (int, string, object[])[0];
        private volatile int counter = 0;
        private ConcurrentDictionary<string, BlockingCollection<(int, string, object[])>> Calls { get; } = new ConcurrentDictionary<string, BlockingCollection<(int, string, object[])>>();
        private ConcurrentDictionary<string, ConcurrentDictionary<int, TaskCompletionSource<object>>> Results { get; } = new ConcurrentDictionary<string, ConcurrentDictionary<int, TaskCompletionSource<object>>>();
        private ConcurrentDictionary<string, TaskCompletionSource<(int, string, object[])[]>> Responders { get; } = new ConcurrentDictionary<string, TaskCompletionSource<(int, string, object[])[]>>();
        private ConcurrentDictionary<string, TaskCompletionSource<bool>> Timers { get; } = new ConcurrentDictionary<string, TaskCompletionSource<bool>>();
        public Service Service { get; private set; }
        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 2, 0);
        public Caller(Service service) {
            Service = service;
            Service.Add<Context, string>(Close, "!!")
                   .Add<Context, Task<(int, string, object[])[]>>(Begin, "!")
                   .Add<(int, object, string)[], Context>(End, "=")
                   .Use(Handler);
        }
        internal string Id(Context context) {
            if (((IDictionary<string, object>)context.RequestHeaders).TryGetValue("Id", out var id)) {
                return id.ToString();
            }
            throw new KeyNotFoundException("client unique id not found");
        }
        private bool Send(string id, TaskCompletionSource<(int, string, object[])[]> responder) {
            if (Calls.TryRemove(id, out var calls)) {
                if (calls.Count == 0) {
                    return false;
                }
                var newcalls = new BlockingCollection<(int, string, object[])>();
                Calls.AddOrUpdate(id, newcalls, (_, oldcalls) => {
                    if (oldcalls != null) {
                        newcalls.Dispose();
                        return oldcalls;
                    }
                    return newcalls;
                });
                responder.TrySetResult(calls.ToArray());
                calls.Dispose();
                return true;
            }
            return false;
        }
        private void Response(string id) {
            if (Responders.TryGetValue(id, out var responder)) {
                if (responder != null) {
                    if (Send(id, responder)) {
                        Responders.TryUpdate(id, null, responder);
                    }
                }
            }
        }
        private string Close(Context context) {
            var id = Id(context);
            if (Responders.TryRemove(id, out var responder)) {
                responder?.TrySetResult(null);
            }
            return id;
        }
        private async Task<(int, string, object[])[]> Begin(Context context) {
            var id = Close(context);
            var responder = new TaskCompletionSource<(int, string, object[])[]>();
            if (!Send(id, responder)) {
                Responders.AddOrUpdate(id, responder, (_, oldResponder) => {
                    oldResponder?.TrySetResult(null);
                    return responder;
                });
                if (Timeout > TimeSpan.Zero) {
                    using (CancellationTokenSource source = new CancellationTokenSource()) {
#if NET40
                        var delay = TaskEx.Delay(Timeout, source.Token);
                        var task = await TaskEx.WhenAny(responder.Task, delay);
#else
                        var delay = Task.Delay(Timeout, source.Token);
                        var task = await Task.WhenAny(responder.Task, delay);
#endif
                        source.Cancel();
                        if (task == delay) {
                            responder.TrySetResult(emptyCall);
                        }
                    }
                }
            }
            return await responder.Task;
        }
        private void End((int, object, string)[] results, Context context) {
            var id = Id(context);
            foreach (var (index, value, error) in results) {
                if (Results.TryGetValue(id, out var result) && result.TryRemove(index, out var task)) {
                    if (error != null) {
                        task.TrySetException(new Exception(error));
                    }
                    else {
                        task.TrySetResult(value);
                    }
                }
            }
        }
        private async Task<object> InvokeAsync(string id, string fullname, object[] args = null) {
            var index = Interlocked.Increment(ref counter);
            while (index < 0) {
                Interlocked.Add(ref counter, Int32.MinValue);
                index = Interlocked.Increment(ref counter);
            }
            var result = new TaskCompletionSource<object>();
            var calls = Calls.GetOrAdd(id, (_) => new BlockingCollection<(int, string, object[])>());
            calls.Add((index, fullname, args));
            var results = Results.GetOrAdd(id, (_) => new ConcurrentDictionary<int, TaskCompletionSource<object>>());
            results[index] = result;
            Response(id);
            return await result.Task;
        }
        public Task Invoke(string id, string fullname, in object[] args = null) {
            return InvokeAsync(id, fullname, args);
        }
        public async Task<T> Invoke<T>(string id, string fullname, object[] args = null) {
            var value = await InvokeAsync(id, fullname, args);
            if (typeof(T).IsAssignableFrom(value.GetType())) {
                return (T)value;
            }
            else {
                return Formatter.Deserialize<T>(Formatter.Serialize(value));
            }
        }
        public T UseService<T>(string id, string ns = "") {
            Type type = typeof(T);
            CallerHandler handler = new CallerHandler(this, id, ns);
            if (type.IsInterface) {
                return (T)Proxy.NewInstance(new Type[] { type }, handler);
            }
            else {
                return (T)Proxy.NewInstance(type.GetInterfaces(), handler);
            }
        }
        private async Task<object> Handler(string name, object[] args, Context context, NextInvokeHandler next) {
            context = new CallerContext(this, context as ServiceContext);
            return await next(name, args, context);
        }
    }
}