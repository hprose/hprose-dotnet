/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  HandlerManager.cs                                       |
|                                                          |
|  HandlerManager class for C#.                            |
|                                                          |
|  LastModified: Jan 20, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public delegate Task<object> NextInvokeHandler(string name, object[] args, Context context);
    public delegate Task<Stream> NextIOHandler(Stream request, Context context);
    public delegate Task<object> InvokeHandler(string name, object[] args, Context context, NextInvokeHandler next);
    public delegate Task<Stream> IOHandler(Stream request, Context context, NextIOHandler next);
    public class HandlerManager {
        private List<InvokeHandler> invokeHandlers = new List<InvokeHandler>();
        private List<IOHandler> ioHandlers = new List<IOHandler>();
        private readonly NextInvokeHandler defaultInvokeHandler;
        private readonly NextIOHandler defaultIOHandler;
        public NextInvokeHandler InvokeHandler { get; private set; }
        public NextIOHandler IOHandler { get; private set; }
        public HandlerManager(NextInvokeHandler invokeHandler, NextIOHandler ioHandler) {
            InvokeHandler = defaultInvokeHandler = invokeHandler;
            IOHandler = defaultIOHandler = ioHandler;
        }
        private NextInvokeHandler GetNextInvokeHandler(InvokeHandler handler, NextInvokeHandler next) {
            return (name, args, context) => handler(name, args, context, next);
        }
        private NextIOHandler GetNextIOHandler(IOHandler handler, NextIOHandler next) {
            return (request, context) => handler(request, context, next);
        }
        private void RebuildInvokeHandler() {
            var next = defaultInvokeHandler;
            var n = invokeHandlers.Count;
            for (var i = n - 1; i >= 0; --i) {
                next = GetNextInvokeHandler(invokeHandlers[i], next);
            }
            InvokeHandler = next;
        }
        private void RebuildIOHandler() {
            var next = defaultIOHandler;
            var n = ioHandlers.Count;
            for (var i = n - 1; i >= 0; --i) {
                next = GetNextIOHandler(ioHandlers[i], next);
            }
            IOHandler = next;
        }
        public void Use(params InvokeHandler[] handlers) {
            invokeHandlers.AddRange(handlers);
            RebuildInvokeHandler();
        }
        public void Use(params IOHandler[] handlers) {
            ioHandlers.AddRange(handlers);
            RebuildIOHandler();
        }
        public void Unuse(params InvokeHandler[] handlers) {
            bool rebuild = false;
            for (int i = 0, n = handlers.Length; i < n; ++i) {
                rebuild = rebuild || invokeHandlers.Remove(handlers[i]);
            }
            if (rebuild) RebuildInvokeHandler();
        }
        public void Unuse(params IOHandler[] handlers) {
            bool rebuild = false;
            for (int i = 0, n = handlers.Length; i < n; ++i) {
                rebuild = rebuild || ioHandlers.Remove(handlers[i]);
            }
            if (rebuild) RebuildIOHandler();
        }
    }
}
