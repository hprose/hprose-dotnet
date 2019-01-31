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
|  LastModified: Jan 31, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins {
    public interface IMockService {
        Task<object> Invoke(string name, object[] args, Context context);
    }

    public class BreakerException : Exception {
        public BreakerException() : base("service breaked") { }
        public BreakerException(string message) : base(message) { }
    }

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
                var response = await next(request, context);
                if (failCount > 0) failCount = 0;
                return response;
            }
            catch (Exception e) {
                Interlocked.Increment(ref failCount);
                lastFailTime = DateTime.Now;
                throw e;
            }
        }
        public async Task<object> InvokeHandler(string name, object[] args, Context context, NextInvokeHandler next) {
            if (MockService == null) {
                return await next(name, args, context);
            }
            try {
                return await next(name, args, context);
            }
            catch (BreakerException e) {
                return await MockService.Invoke(name, args, context);
            }
        }
    }
}
