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
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/
#if !NET35_CF
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Limiter {
    public class RateLimiter {
        private long next = DateTime.Now.Ticks;
        private readonly double interval;
        public long PermitsPerSecond { get; private set; }
        public long MaxPermits { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public RateLimiter(long permitsPerSecond, long maxPermits = long.MaxValue, TimeSpan timeout = default) {
            PermitsPerSecond = permitsPerSecond;
            MaxPermits = maxPermits;
            Timeout = timeout;
            interval = (double)(new TimeSpan(0, 0, 1).Ticks) / permitsPerSecond;
        }
        public async Task<long> Acquire(long tokens = 1) {
            var now = DateTime.Now.Ticks;
            long last = Interlocked.Read(ref next);
            double permits = (now - last) / interval - tokens;
            if (permits > MaxPermits) {
                permits = MaxPermits;
            }
            Interlocked.Exchange(ref next, now - (long)(permits * interval));
            var delay = new TimeSpan(last - now);
            if (delay <= TimeSpan.Zero) return last;
            if (Timeout > TimeSpan.Zero && delay > Timeout) {
                throw new TimeoutException();
            }
#if NET40
            await TaskEx.Delay(delay).ConfigureAwait(false);
#else
            await Task.Delay(delay).ConfigureAwait(false);
#endif
            return last;
        }
        public async Task<Stream> IOHandler(Stream request, Context context, NextIOHandler next) {
            if (!request.CanSeek) {
                request = await request.ToMemoryStream().ConfigureAwait(false);
            }
            await Acquire(request.Length).ConfigureAwait(false);
            return await next(request, context).ConfigureAwait(false);
        }
        public async Task<object> InvokeHandler(string name, object[] args, Context context, NextInvokeHandler next) {
            await Acquire().ConfigureAwait(false);
            return await next(name, args, context).ConfigureAwait(false);
        }
    }
}
#endif