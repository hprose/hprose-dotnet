/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ExecuteTimeout.cs                                       |
|                                                          |
|  ExecuteTimeout plugin for C#.                           |
|                                                          |
|  LastModified: Mar 7, 2020                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Timeout {
    public class ExecuteTimeout {
        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 30);
        public async Task<object> Handler(string fullname, object[] args, Context context, NextInvokeHandler next) {
            var resultTask = next(fullname, args, context);
            var serviceContext = context as ServiceContext;
            var timeout = Timeout;
            if (serviceContext.Method.Options.ContainsKey("timeout")) {
                timeout = (TimeSpan)(serviceContext.Method.Options["timeout"]);
            }
            if (timeout > TimeSpan.Zero) {
                using CancellationTokenSource source = new CancellationTokenSource();
#if NET40
                var timer = TaskEx.Delay(timeout, source.Token);
                var task = await TaskEx.WhenAny(resultTask, timer).ConfigureAwait(false);
#else
                var timer = Task.Delay(timeout, source.Token);
                var task = await Task.WhenAny(resultTask, timer).ConfigureAwait(false);
#endif
                source.Cancel();
                if (task == timer) {
                    throw new TimeoutException();
                }
            }
            return await resultTask.ConfigureAwait(false);
        }
    }
}
