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
|  LastModified: Feb 4, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Threading.Tasks;

namespace Hprose.RPC {

    interface IInvoker {
        object Invoke(object[] args);
    }

    class SyncInvoker<T> : IInvoker {
        private readonly Client client;
        private readonly string name;
        private readonly Settings settings;
        public SyncInvoker(Client client, string name, Settings settings) {
            this.client = client;
            this.name = name;
            this.settings = settings;
        }
        public object Invoke(object[] args) {
            return client.Invoke<T>(name, args, settings);
        }
    }

    class SyncInvoker : IInvoker {
        private readonly Client client;
        private readonly string name;
        private readonly Settings settings;
        public SyncInvoker(Client client, string name, Settings settings) {
            this.client = client;
            this.name = name;
            this.settings = settings;
        }
        public object Invoke(object[] args) {
            client.Invoke(name, args, settings);
            return null;
        }
    }

    class AsyncInvoker<T> : IInvoker {
        private readonly Client client;
        private readonly string name;
        private readonly Settings settings;
        public AsyncInvoker(Client client, string name, Settings settings) {
            this.client = client;
            this.name = name;
            this.settings = settings;
        }
        public object Invoke(object[] args) {
            return client.InvokeAsync<T>(name, args, settings);
        }
    }

    class AsyncInvoker : IInvoker {
        private readonly Client client;
        private readonly string name;
        private readonly Settings settings;
        public AsyncInvoker(Client client, string name, Settings settings) {
            this.client = client;
            this.name = name;
            this.settings = settings;
        }
        public object Invoke(object[] args) {
            return client.InvokeAsync(name, args, settings);
        }
    }

    class InvocationHandler : IInvocationHandler {
        private readonly ConcurrentDictionary<MethodInfo, Lazy<IInvoker>> invokers = new ConcurrentDictionary<MethodInfo, Lazy<IInvoker>>();
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
            var settings = new Settings();
            foreach (var attribute in attributes) {
                if (attribute is NameAttribute) {
                    name = ((NameAttribute)attribute).Value;
                }
                else if (attribute is HeaderAttribute) {
                    if (settings.RequestHeaders == null) {
                        settings.RequestHeaders = new ExpandoObject();
                    }
                    IDictionary<string, object> headers = settings.RequestHeaders;
                    var (key, value) = ((HeaderAttribute)attribute).Value;
                    headers.Add(key, value);
                }
                else if (attribute is ContextAttribute) {
                    if (settings.Context == null) {
                        settings.Context = new Dictionary<string, object>();
                    }
                    IDictionary<string, object> context = settings.Context;
                    var (key, value) = ((ContextAttribute)attribute).Value;
                    context.Add(key, value);
                }
            }
            if (string.IsNullOrEmpty(ns)) {
                name = ns + '_' + name;
            }
            if (typeof(Task).IsAssignableFrom(returnType)) {
                if (returnType == typeof(Task)) {
                    return new AsyncInvoker(client, name, settings);
                }
                settings.Type = returnType.GetGenericArguments()[0];
                return (IInvoker)Activator.CreateInstance(typeof(AsyncInvoker<>).MakeGenericType(settings.Type), new object[] { client, name, settings });
            }
            if (returnType == typeof(void)) {
                return new SyncInvoker(client, name, settings);
            }
            settings.Type = returnType;
            return (IInvoker)Activator.CreateInstance(typeof(SyncInvoker<>).MakeGenericType(settings.Type), new object[] { client, name, settings });
        }
    }
}
