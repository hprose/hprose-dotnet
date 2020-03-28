/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  CallerContext.cs                                        |
|                                                          |
|  CallerContext class for C#.                             |
|                                                          |
|  LastModified: Mar 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Reverse {
    public class CallerContext : ServiceContext {
        public Caller Caller { get; private set; }
        public CallerContext(Caller caller, ServiceContext context) : base(context.Service) {
            context.CopyTo(this);
            Caller = caller;
        }
        public void Invoke(string name, in object[] args = null) {
            Caller.InvokeAsync<object>(Caller.GetId(this), name, args).ConfigureAwait(false).GetAwaiter().GetResult();
            return;
        }
        public T Invoke<T>(string name, in object[] args = null) {
            return Caller.InvokeAsync<T>(Caller.GetId(this), name, args).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public Task InvokeAsync(string name, object[] args = null) {
            return Caller.InvokeAsync(Caller.GetId(this), name, args);
        }
        public Task<T> InvokeAsync<T>(string name, object[] args = null) {
            return Caller.InvokeAsync<T>(Caller.GetId(this), name, args);
        }
    }
#if !NET35_CF
    public class CallerContext<T> : CallerContext {
        public T Proxy { get; private set; }
        public CallerContext(Caller caller, ServiceContext context) : base(caller, context) {
            Proxy = caller.UseService<T>(Caller.GetId(this));
        }
    }
#endif
}