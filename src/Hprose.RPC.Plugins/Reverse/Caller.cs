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
|  LastModified: Feb 6, 2019                               |
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
#if NET40 || NET45 || NET451 || NET452
        private static readonly (int, string, object[])[] emptyCall = new (int, string, object[])[0];
#else
        private static readonly (int, string, object[])[] emptyCall = Array.Empty<(int, string, object[])>();
#endif
        private volatile int counter = 0;
        private ConcurrentDictionary<string, ConcurrentQueue<(int, string, object[])>> Calls { get; } = new ConcurrentDictionary<string, ConcurrentQueue<(int, string, object[])>>();
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
                var newcalls = new ConcurrentQueue<(int, string, object[])>();
                Calls.AddOrUpdate(id, newcalls, (_, oldcalls) => oldcalls ?? newcalls);
                responder.TrySetResult(calls.ToArray());
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
                        var task = await TaskEx.WhenAny(responder.Task, delay).ConfigureAwait(false);
#else
                        var delay = Task.Delay(Timeout, source.Token);
                        var task = await Task.WhenAny(responder.Task, delay).ConfigureAwait(false);
#endif
                        source.Cancel();
                        if (task == delay) {
                            responder.TrySetResult(emptyCall);
                        }
                    }
                }
            }
            return await responder.Task.ConfigureAwait(false);
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
        public Task InvokeAsync(string id, string fullname, in object[] args = null) {
            return InvokeAsync<object>(id, fullname, args);
        }
        public async Task<T> InvokeAsync<T>(string id, string fullname, object[] args = null) {
            var index = Interlocked.Increment(ref counter);
            while (index < 0) {
                Interlocked.Add(ref counter, Int32.MinValue);
                index = Interlocked.Increment(ref counter);
            }
            var result = new TaskCompletionSource<object>();
            var calls = Calls.GetOrAdd(id, (_) => new ConcurrentQueue<(int, string, object[])>());
            calls.Enqueue((index, fullname, args));
            var results = Results.GetOrAdd(id, (_) => new ConcurrentDictionary<int, TaskCompletionSource<object>>());
            results[index] = result;
            Response(id);
            var value = await result.Task.ConfigureAwait(false);
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
        private Task<object> Handler(string name, object[] args, Context context, NextInvokeHandler next) {
            context = new CallerContext(this, context as ServiceContext);
            return next(name, args, context);
        }
    }
}