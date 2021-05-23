﻿/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Caller.cs                                               |
|                                                          |
|  Caller class for C#.                                    |
|                                                          |
|  LastModified: Jul 2, 2020                               |
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
        private static readonly object[] emptyArgs = new object[0];
        private static readonly (int, string, object[])[] emptyCall = new (int, string, object[])[0];
        private volatile int counter = 0;
        private ConcurrentDictionary<string, ConcurrentQueue<(int, string, object[])>> Calls { get; } = new();
        private ConcurrentDictionary<string, ConcurrentDictionary<int, TaskCompletionSource<object>>> Results { get; } = new();
        private ConcurrentDictionary<string, TaskCompletionSource<(int, string, object[])[]>> Responders { get; } = new();
        private ConcurrentDictionary<string, bool> Onlines { get; } = new();
        public Service Service { get; private set; }
        public TimeSpan HeartBeat { get; set; } = new(0, 2, 0);
        public TimeSpan Timeout { get; set; } = new(0, 0, 30);
        public Caller(Service service) {
            Service = service;
            Service.Add<ServiceContext>(Close, "!!")
                   .Add<ServiceContext, Task<(int, string, object[])[]>>(Begin, "!")
                   .Add<(int, object, string)[], ServiceContext>(End, "=")
                   .Use(Handler);
        }
        internal static string GetId(ServiceContext context) {
            if (context.RequestHeaders.TryGetValue("id", out var id)) {
                return id.ToString();
            }
            throw new KeyNotFoundException("Client unique id not found");
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
        private string Stop(ServiceContext context) {
            var id = GetId(context);
            if (Responders.TryRemove(id, out var responder)) {
                responder?.TrySetResult(null);
            }
            return id;
        }
        private void Close(ServiceContext context) {
            var id = Stop(context);
            Onlines.TryRemove(id, out _);
        }
        private async Task<(int, string, object[])[]> Begin(ServiceContext context) {
            var id = Stop(context);
            Onlines.TryAdd(id, true);
            var responder = new TaskCompletionSource<(int, string, object[])[]>();
            if (!Send(id, responder)) {
                Responders.AddOrUpdate(id, responder, (_, oldResponder) => {
                    oldResponder?.TrySetResult(null);
                    return responder;
                });
                if (HeartBeat > TimeSpan.Zero) {
                    using var source = new CancellationTokenSource();
#if NET40
                    var delay = TaskEx.Delay(HeartBeat, source.Token);
                    var task = await TaskEx.WhenAny(responder.Task, delay).ConfigureAwait(false);
#else
                    var delay = Task.Delay(HeartBeat, source.Token);
                    var task = await Task.WhenAny(responder.Task, delay).ConfigureAwait(false);
#endif
                    source.Cancel();
                    if (task == delay) {
                        responder.TrySetResult(emptyCall);
                    }
                }
            }
            return await responder.Task.ConfigureAwait(false);
        }
        private void End((int, object, string)[] results, ServiceContext context) {
            var id = GetId(context);
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
        public void Invoke(string id, string name, object[] args = null) {
            InvokeAsync<object>(id, name, args).Wait();
        }
        public T Invoke<T>(string id, string name, object[] args = null) {
            return InvokeAsync<T>(id, name, args).Result;
        }
        public Task InvokeAsync(string id, string name, object[] args = null) {
            return InvokeAsync<object>(id, name, args);
        }
        public async Task<T> InvokeAsync<T>(string id, string name, object[] args = null) {
            if (args == null) args = emptyArgs;
            for (int i = 0; i < args.Length; ++i) {
                if (args[i] is Task) {
                    args[i] = await TaskResult.Get(args[i] as Task).ConfigureAwait(false);
                }
            }
            var index = Interlocked.Increment(ref counter) & 0x7FFFFFFF;
            var result = new TaskCompletionSource<object>();
            var calls = Calls.GetOrAdd(id, (_) => new ConcurrentQueue<(int, string, object[])>());
            calls.Enqueue((index, name, args));
            var results = Results.GetOrAdd(id, (_) => new ConcurrentDictionary<int, TaskCompletionSource<object>>());
            results[index] = result;
            Response(id);
            if (Timeout > TimeSpan.Zero) {
                using var source = new CancellationTokenSource();
#if NET40
                var delay = TaskEx.Delay(Timeout, source.Token);
                var task = await TaskEx.WhenAny(result.Task, delay).ConfigureAwait(false);
#else
                var delay = Task.Delay(Timeout, source.Token);
                var task = await Task.WhenAny(result.Task, delay).ConfigureAwait(false);
#endif
                source.Cancel();
                if (task == delay) {
                    lock (calls) {
                        for (var i = 0; i < calls.Count; i++) {
                            if (calls.TryDequeue(out (int index, string name, object[] args) call)) {
                                if (index != call.index) {
                                    calls.Enqueue(call);
                                }
                            }
                        }
                    }
                    results.TryRemove(index, out var _);
                    result.TrySetException(new TimeoutException());
                }
            }
            await result.Task.ContinueWith((_) => { }, TaskScheduler.Current).ConfigureAwait(false);
            var value = await result.Task.ConfigureAwait(false);
            if (value == null) {
                return default;
            }
            if (typeof(T).IsAssignableFrom(value.GetType())) {
                return (T)value;
            }
            else {
                return Formatter.Deserialize<T>(Formatter.Serialize(value));
            }
        }
#if !NET35_CF
        public T UseService<T>(string id, string ns = "") {
            var type = typeof(T);
            var handler = new CallerHandler(this, id, ns);
            if (type.IsInterface) {
                return (T)Proxy.NewInstance(new Type[] { type }, handler);
            }
            else {
                return (T)Proxy.NewInstance(type.GetInterfaces(), handler);
            }
        }
#endif
        public bool Exists(string id) {
            return Onlines.ContainsKey(id);
        }
        public IList<string> IdList() {
            return new List<string>(Onlines.Keys);
        }
        private Task<object> Handler(string name, object[] args, Context context, NextInvokeHandler next) {
            var serviceContext = context as ServiceContext;
#if !NET35_CF
            var parameters = serviceContext.Method.Parameters;
            if (parameters.Length > 0) {
                var parameterType = parameters[parameters.Length - 1].ParameterType;
                if (typeof(CallerContext).IsAssignableFrom(parameterType) && parameterType.IsGenericType) {
                    context = (CallerContext)(Activator.CreateInstance(typeof(CallerContext<>).MakeGenericType(parameterType.GetGenericArguments()[0]), new object[] { this, serviceContext }));
                    return next(name, args, context);
                }
            }
#endif
            context = new CallerContext(this, serviceContext);
            return next(name, args, context);
        }
    }
}