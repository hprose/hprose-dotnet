/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  InvokeManager.cs                                        |
|                                                          |
|  InvokeManager class for C#.                             |
|                                                          |
|  LastModified: Feb 16, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Threading.Tasks;

namespace Hprose.RPC {
    public delegate Task<object> NextInvokeHandler(string name, object[] args, Context context);
    public delegate Task<object> InvokeHandler(string name, object[] args, Context context, NextInvokeHandler next);
    public class InvokeManager : PluginManager<InvokeHandler, NextInvokeHandler> {
        public InvokeManager(NextInvokeHandler handler) : base(handler) { }
        protected override NextInvokeHandler GetNextHandler(InvokeHandler handler, NextInvokeHandler next) {
            return (name, args, context) => handler(name, args, context, next);
        }
    }
}
