/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  InvocationHandler.cs                                    |
|                                                          |
|  InvocationHandler class for C#.                         |
|                                                          |
|  LastModified: Feb 27, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/
#if !NET35_CF
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;

namespace Hprose.RPC {
    internal interface IInvoker {
        object Invoke(object[] args);
    }

    internal class SyncInvoker<T> : IInvoker {
        private readonly Client client;
        private readonly string name;
        private readonly ClientContext context;
        public SyncInvoker(Client client, string name, ClientContext context) {
            this.client = client;
            this.name = name;
            this.context = context;
        }
        public object Invoke(object[] args) {
            return client.Invoke<T>(name, args, context?.Clone() as ClientContext);
        }
    }

    internal class SyncInvoker : IInvoker {
        private readonly Client client;
        private readonly string name;
        private readonly ClientContext context;
        public SyncInvoker(Client client, string name, ClientContext context) {
            this.client = client;
            this.name = name;
            this.context = context;
        }
        public object Invoke(object[] args) {
            client.Invoke(name, args, context?.Clone() as ClientContext);
            return null;
        }
    }

    internal class AsyncInvoker<T> : IInvoker {
        private readonly Client client;
        private readonly string name;
        private readonly ClientContext context;
        public AsyncInvoker(Client client, string name, ClientContext context) {
            this.client = client;
            this.name = name;
            this.context = context;
        }
        public object Invoke(object[] args) {
            return client.InvokeAsync<T>(name, args, context?.Clone() as ClientContext);
        }
    }

    internal class AsyncInvoker : IInvoker {
        private readonly Client client;
        private readonly string name;
        private readonly ClientContext context;
        public AsyncInvoker(Client client, string name, ClientContext context) {
            this.client = client;
            this.name = name;
            this.context = context;
        }
        public object Invoke(object[] args) {
            return client.InvokeAsync(name, args, context?.Clone() as ClientContext);
        }
    }

    internal class InvocationHandler : IInvocationHandler {
        private readonly ConcurrentDictionary<MethodInfo, Lazy<IInvoker>> invokers = new();
        private readonly string ns;
        private readonly Client client;
        private readonly Func<MethodInfo, Lazy<IInvoker>> invokerFactory;
        public InvocationHandler(Client client, string ns) {
            this.client = client;
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
            ClientContext context = null;
            foreach (var attribute in attributes) {
                switch (attribute) {
                    case NameAttribute nameAttr:
                        name = nameAttr.Value;
                        break;
                    case HeaderAttribute headerAttr: {
                        if (context == null) context = new ClientContext();
                        var headers = context.RequestHeaders;
                        var (key, value) = headerAttr.Value;
                        headers[key] = value;
                    }
                    break;
                    case ContextAttribute contextAttr: {
                        if (context == null) context = new ClientContext();
                        var (key, value) = contextAttr.Value;
                        context[key] = value;
                    }
                    break;
                    case TimeoutAttribute timeoutAttr: {
                        if (context == null) context = new ClientContext();
                        context.Timeout = timeoutAttr.Value;
                    }
                    break;
                }
            }
            if (!string.IsNullOrEmpty(ns)) {
                name = ns + '_' + name;
            }
            if (typeof(Task).IsAssignableFrom(returnType)) {
                if (returnType == typeof(Task)) {
                    return new AsyncInvoker(client, name, context);
                }
                returnType = returnType.GetGenericArguments()[0];
                return (IInvoker)Activator.CreateInstance(typeof(AsyncInvoker<>).MakeGenericType(returnType), new object[] { client, name, context });
            }
            if (returnType == typeof(void)) {
                return new SyncInvoker(client, name, context);
            }
            return (IInvoker)Activator.CreateInstance(typeof(SyncInvoker<>).MakeGenericType(returnType), new object[] { client, name, context });
        }
    }
}
#endif