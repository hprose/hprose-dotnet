/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  RateLimiter.cs                                          |
|                                                          |
|  RateLimiter plugin for C#.                              |
|                                                          |
|  LastModified: Feb 4, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Limiter {
    public class RateLimiter {
        private long next = DateTime.Now.Ticks;
        private readonly long interval;
        public long PermitsPerSecond { get; private set; }
        public long MaxPermits { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public RateLimiter(long permitsPerSecond, long maxPermits, TimeSpan timeout = default) {
            PermitsPerSecond = permitsPerSecond;
            MaxPermits = maxPermits;
            Timeout = timeout;
            interval = new TimeSpan(0, 0, 1).Ticks / permitsPerSecond;
        }
        public async Task<long> Acquire(long tokens = 1) {
            var now = DateTime.Now.Ticks;
            long last = Interlocked.Read(ref next);
            var permits = (now - last) / interval - tokens;
            if (permits > MaxPermits) {
                permits = MaxPermits;
            }
            Interlocked.Exchange(ref next, now - permits * interval);
            var delay = new TimeSpan(last - now);
            if (delay <= TimeSpan.Zero) return last;
            if (Timeout > TimeSpan.Zero && delay > Timeout) {
                throw new TimeoutException();
            }
#if NET40
            await TaskEx.Delay(delay);
#else
            await Task.Delay(delay);
#endif
            return last;
        }
        public async Task<Stream> IOHandler(Stream request, Context context, NextIOHandler next) {
            if (!request.CanSeek) {
                MemoryStream stream = new MemoryStream();
                await request.CopyToAsync(stream);
                stream.Position = 0;
                request.Dispose();
                request = stream;
            }
            await Acquire(request.Length);
            return await next(request, context);
        }
        public async Task<object> InvokeHandler(string name, object[] args, Context context, NextInvokeHandler next) {
            await Acquire();
            return await next(name, args, context);
        }
    }
}
