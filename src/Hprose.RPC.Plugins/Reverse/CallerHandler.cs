﻿/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  CallerHandler.cs                                        |
|                                                          |
|  CallerHandler class for C#.                             |
|                                                          |
|  LastModified: Jul 2, 2020                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/
#if !NET35_CF
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Reverse {
    internal interface IInvoker {
        object Invoke(object[] args);
    }

    internal class SyncInvoker<T> : IInvoker {
        private readonly Caller caller;
        private readonly string name;
        private readonly string id;
        public SyncInvoker(Caller caller, string id, string name) {
            this.caller = caller;
            this.name = name;
            this.id = id;
        }
        public object Invoke(object[] args) {
            return caller.Invoke<T>(id, name, args);
        }
    }

    internal class SyncInvoker : IInvoker {
        private readonly Caller caller;
        private readonly string name;
        private readonly string id;
        public SyncInvoker(Caller caller, string id, string name) {
            this.caller = caller;
            this.name = name;
            this.id = id;
        }
        public object Invoke(object[] args) {
            caller.Invoke(id, name, args);
            return null;
        }
    }

    internal class AsyncInvoker<T> : IInvoker {
        private readonly Caller caller;
        private readonly string name;
        private readonly string id;
        public AsyncInvoker(Caller caller, string id, string name) {
            this.caller = caller;
            this.name = name;
            this.id = id;
        }
        public object Invoke(object[] args) {
            return caller.InvokeAsync<T>(id, name, args);
        }
    }

    internal class AsyncInvoker : IInvoker {
        private readonly Caller caller;
        private readonly string name;
        private readonly string id;
        public AsyncInvoker(Caller caller, string id, string name) {
            this.caller = caller;
            this.name = name;
            this.id = id;
        }
        public object Invoke(object[] args) {
            return caller.InvokeAsync(id, name, args);
        }
    }

    internal class CallerHandler : IInvocationHandler {
        private readonly ConcurrentDictionary<MethodInfo, Lazy<IInvoker>> invokers = new();
        private readonly string id;
        private readonly string ns;
        private readonly Caller caller;
        private readonly Func<MethodInfo, Lazy<IInvoker>> invokerFactory;
        public CallerHandler(Caller caller, string id, string ns) {
            this.caller = caller;
            this.id = id;
            this.ns = ns;
            invokerFactory = method => new Lazy<IInvoker>(() => GetIInvoker(method));
        }
        public object Invoke(object proxy, MethodInfo method, object[] args) {
            return invokers.GetOrAdd(method, invokerFactory).Value.Invoke(args);
        }
        private IInvoker GetIInvoker(MethodInfo method) {
            var name = method.Name;
            var returnType = method.ReturnType;
            var attributes = Attribute.GetCustomAttributes(method, true);
            foreach (var attribute in attributes) {
                if (attribute is NameAttribute attr) {
                    name = attr.Value;
                }
            }
            if (!string.IsNullOrEmpty(ns)) {
                name = ns + '_' + name;
            }
            if (typeof(Task).IsAssignableFrom(returnType)) {
                if (returnType == typeof(Task)) {
                    return new AsyncInvoker(caller, id, name);
                }
                return (IInvoker)Activator.CreateInstance(typeof(AsyncInvoker<>).MakeGenericType(returnType.GetGenericArguments()[0]), new object[] { caller, id, name });
            }
            if (returnType == typeof(void)) {
                return new SyncInvoker(caller, id, name);
            }
            return (IInvoker)Activator.CreateInstance(typeof(SyncInvoker<>).MakeGenericType(returnType), new object[] { caller, id, name });
        }
    }
}
#endif