/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  PluginManager.cs                                        |
|                                                          |
|  PluginManager class for C#.                             |
|                                                          |
|  LastModified: Jan 24, 2021                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.Threading;

namespace Hprose.RPC {
    public abstract class PluginManager<THandler, TNextHandler> : IDisposable {
        private readonly List<THandler> handlers = new();
        private readonly ReaderWriterLockSlim rwlock = new();
        private readonly TNextHandler defaultHandler;
        public TNextHandler Handler { get; private set; }
        public PluginManager(TNextHandler handler) {
            Handler = defaultHandler = handler;
        }
        protected abstract TNextHandler GetNextHandler(THandler handler, TNextHandler next);
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
        public void Use(params THandler[] handlers) {
            rwlock.EnterWriteLock();
            this.handlers.AddRange(handlers);
            rwlock.ExitWriteLock();
            RebuildHandler();
        }
        public void Unuse(params THandler[] handlers) {
            bool rebuild = false;
            rwlock.EnterWriteLock();
            for (int i = 0, n = handlers.Length; i < n; ++i) {
                if (this.handlers.Remove(handlers[i])) {
                    rebuild = true;
                }
            }
            rwlock.ExitWriteLock();
            if (rebuild) RebuildHandler();
        }
        private bool disposed = false;
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing) {
            if (disposed) return;
            if (disposing) {
                rwlock.Dispose();
            }
            disposed = true;
        }
    }
}
