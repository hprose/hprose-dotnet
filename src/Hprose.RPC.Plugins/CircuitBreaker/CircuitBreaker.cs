/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  CircuitBreaker.cs                                       |
|                                                          |
|  CircuitBreaker plugin for C#.                           |
|                                                          |
|  LastModified: Feb 1, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.CircuitBreaker {
    public class CircuitBreaker {
        private DateTime lastFailTime = new DateTime(0);
        private volatile int failCount = 0;
        public int Threshold { get; private set; }
        public TimeSpan RecoverTime { get; private set; }
        public IMockService MockService { get; private set; }
        public CircuitBreaker(int threshold = 5, TimeSpan recoverTime = default, IMockService mockService = null) {
            if (recoverTime == default) {
                recoverTime = new TimeSpan(0, 0, 30);
            }
            Threshold = threshold;
            RecoverTime = recoverTime;
            MockService = mockService;
        }
        public async Task<Stream> IOHandler(Stream request, Context context, NextIOHandler next) {
            if (failCount > Threshold) {
                var interval = DateTime.Now - lastFailTime;
                if (interval < RecoverTime) {
                    throw new BreakerException();
                }
                failCount = Threshold >> 1;
            }
            try {
                var response = await next(request, context).ConfigureAwait(false);
                if (failCount > 0) failCount = 0;
                return response;
            }
            catch (Exception) {
                Interlocked.Increment(ref failCount);
                lastFailTime = DateTime.Now;
                throw;
            }
        }
        public async Task<object> InvokeHandler(string name, object[] args, Context context, NextInvokeHandler next) {
            if (MockService == null) {
                return await next(name, args, context).ConfigureAwait(false);
            }
            try {
                return await next(name, args, context).ConfigureAwait(false);
            }
            catch (BreakerException) {
                return await MockService.Invoke(name, args, context).ConfigureAwait(false);
            }
        }
    }
}
