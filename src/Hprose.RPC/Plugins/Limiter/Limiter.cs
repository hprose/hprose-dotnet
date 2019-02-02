/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Limiter.cs                                              |
|                                                          |
|  Limiter plugin for C#.                                  |
|                                                          |
|  LastModified: Feb 2, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Limiter {
    public class Limiter {
        private volatile int counter = 0;
        private ConcurrentQueue<TaskCompletionSource<bool>> tasks = new ConcurrentQueue<TaskCompletionSource<bool>>();
        public int MaxConcurrentRequests { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public Limiter(int maxConcurrentRequests, TimeSpan timeout) {
            MaxConcurrentRequests = maxConcurrentRequests;
            Timeout = timeout;
        }
        public async Task Acquire() {
            if (Interlocked.Increment(ref counter) < MaxConcurrentRequests) return;
            var deferred = new TaskCompletionSource<bool>();
            tasks.Enqueue(deferred);
            if (Timeout > TimeSpan.Zero) {
                using (CancellationTokenSource source = new CancellationTokenSource()) {
#if NET40
                    var timer = TaskEx.Delay(Timeout, source.Token);
                    var task = await TaskEx.WhenAny(timer, deferred.Task);
#else
                    var timer = Task.Delay(Timeout, source.Token);
                    var task = await Task.WhenAny(timer, deferred.Task);
#endif
                    source.Cancel();
                    if (task == timer) {
                        deferred.TrySetException(new TimeoutException());
                    }
                }
            }
            await deferred.Task;
        }
        public void Release() {
            if (Interlocked.Decrement(ref counter) >= MaxConcurrentRequests) return;
            if(tasks.TryDequeue(out var task)) {
                task.TrySetResult(true);
            }
        }
        public async Task<object> Handler(string name, object[] args, Context context, NextInvokeHandler next) {
            await Acquire();
            var result = next(name, args, context);
            await result.ContinueWith((task) => Release());
            return await result;
        }
    }
}
