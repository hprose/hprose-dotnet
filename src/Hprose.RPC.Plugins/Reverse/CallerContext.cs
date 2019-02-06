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
|  LastModified: Feb 3, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Reverse {
    public class CallerContext : ServiceContext {
        public Caller Caller { get; private set; }
        public CallerContext(Caller caller, ServiceContext context) : base(context.Service) {
            Caller = caller;
            Copy(context.RequestHeaders, RequestHeaders);
            Copy(context.ResponseHeaders, ResponseHeaders);
            Method = context.Method;
            Copy(context.Items, Items);
        }
        public Task InvokeAsync(string fullname, in object[] args = null) {
            return Caller.InvokeAsync(Caller.Id(this), fullname, args);
        }
        public Task<T> InvokeAsync<T>(string fullname, in object[] args = null) {
            return Caller.InvokeAsync<T>(Caller.Id(this), fullname, args);
        }
    }
}