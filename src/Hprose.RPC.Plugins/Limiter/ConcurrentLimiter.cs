/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ConcurrentLimiter.cs                                    |
|                                                          |
|  ConcurrentLimiter plugin for C#.                        |
|                                                          |
|  LastModified: Mar 26, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Limiter {
    public class ConcurrentLimiter {
        private volatile int counter = 0;
        private readonly ConcurrentQueue<TaskCompletionSource<bool>> tasks = new ConcurrentQueue<TaskCompletionSource<bool>>();
        public int MaxConcurrentRequests { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public ConcurrentLimiter(int maxConcurrentRequests, TimeSpan timeout = default) {
            MaxConcurrentRequests = maxConcurrentRequests;
            Timeout = timeout;
        }
        public async Task Acquire() {
            if (Interlocked.Increment(ref counter) <= MaxConcurrentRequests) return;
            var deferred = new TaskCompletionSource<bool>();
            tasks.Enqueue(deferred);
            if (Timeout > TimeSpan.Zero) {
                using (CancellationTokenSource source = new CancellationTokenSource()) {
#if NET40
                    var timer = TaskEx.Delay(Timeout, source.Token);
                    var task = await TaskEx.WhenAny(timer, deferred.Task).ConfigureAwait(false);
#else
                    var timer = Task.Delay(Timeout, source.Token);
                    var task = await Task.WhenAny(timer, deferred.Task).ConfigureAwait(false);
#endif
                    source.Cancel();
                    if (task == timer) {
                        deferred.TrySetException(new TimeoutException());
                    }
                }
            }
            await deferred.Task.ConfigureAwait(false);
        }
        public void Release() {
            while (true) {
                Interlocked.Decrement(ref counter);
                if (tasks.TryDequeue(out var task)) {
                    if (task.TrySetResult(true)) return;
                } else {
                    return;
                }
            }
        }
        public async Task<Stream> Handler(Stream request, Context context, NextIOHandler next) {
            await Acquire().ConfigureAwait(false);
            try {
                return await next(request, context).ConfigureAwait(false);
            }
            finally {
                Release();
            }
        }
    }
}
