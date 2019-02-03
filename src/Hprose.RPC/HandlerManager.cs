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
|  LastModified: Feb 4, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections.Generic;
using System.Threading;

namespace Hprose.RPC {
    public abstract class HandlerManager<HandlerType, NextHandlerType> {
        private List<HandlerType> handlers = new List<HandlerType>();
        private ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim();
        private readonly NextHandlerType defaultHandler;
        public NextHandlerType Handler { get; private set; }
        public HandlerManager(NextHandlerType handler) {
            Handler = defaultHandler = handler;
        }
        protected abstract NextHandlerType GetNextHandler(HandlerType handler, NextHandlerType next);
        private void RebuildHandler() {
            var next = defaultHandler;
            rwlock.EnterReadLock();
            var n = handlers.Count;
            for (var i = n - 1; i >= 0; --i) {
                next = GetNextHandler(handlers[i], next);
            }
            rwlock.ExitReadLock();
            Handler = next;
        }
        public void Use(params HandlerType[] handlers) {
            rwlock.EnterWriteLock();
            this.handlers.AddRange(handlers);
            rwlock.ExitWriteLock();
            RebuildHandler();
        }
        public void Unuse(params HandlerType[] handlers) {
            bool rebuild = false;
            rwlock.EnterWriteLock();
            for (int i = 0, n = handlers.Length; i < n; ++i) {
                rebuild = rebuild || this.handlers.Remove(handlers[i]);
            }
            rwlock.ExitWriteLock();
            if (rebuild) RebuildHandler();
        }
    }
}
