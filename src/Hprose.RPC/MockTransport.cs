/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
| MockTransport.cs                                         |
|                                                          |
| MockTransport for C#.                                    |
|                                                          |
| LastModified: Feb 27, 2019                               |
| Author: Ma Bingyao <andot@hprose.com>                    |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class MockTransport : ITransport {
        public static string[] Schemes { get; } = new string[] { "mock" };
        public async Task<Stream> Transport(Stream request, Context context) {
            var clientContext = context as ClientContext;
            var result = MockAgent.Handler(clientContext.Uri.Host, request);
            var timeout = clientContext.Timeout;
            if (timeout > TimeSpan.Zero) {
                using (CancellationTokenSource source = new CancellationTokenSource()) {
#if NET40
                   var timer = TaskEx.Delay(timeout, source.Token);
                   var task = await TaskEx.WhenAny(timer, result).ConfigureAwait(false);
#else
                    var timer = Task.Delay(timeout, source.Token);
                    var task = await Task.WhenAny(timer, result).ConfigureAwait(false);
#endif
                    source.Cancel();
                    if (task == timer) {
                        throw new TimeoutException();
                    }
                }
            }
            return await result;
        }
        public Task Abort() {
#if NET40
            return TaskEx.FromResult<object>(null);
#elif NET45 || NET451 || NET452
            return Task.FromResult<object>(null);
#else
            return Task.CompletedTask;
#endif
        }
    }
}
