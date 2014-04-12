/**********************************************************\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: http://www.hprose.com/                 |
|                   http://www.hprose.net/                 |
|                   http://www.hprose.org/                 |
|                                                          |
\**********************************************************/
/**********************************************************\
 *                                                        *
 * HproseClient.cs                                        *
 *                                                        *
 * hprose client class for C#.                            *
 *                                                        *
 * LastModified: Apr 10, 2014                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;
#if (dotNET10 || dotNET11 || dotNETCF10)
using System.Collections;
#else
using System.Collections.Generic;
#endif
using System.IO;
#if !(SILVERLIGHT || WINDOWS_PHONE || Core)
using System.Net;
#endif
using System.Text;
using System.Threading;
using Hprose.IO;
using Hprose.Common;
#if !(PocketPC || Smartphone || WindowsCE || WINDOWS_PHONE || Core)
using Hprose.Reflection;
#endif

namespace Hprose.Client {
    public abstract class HproseClient : HproseInvoker {
        private static readonly object[] nullArgs = new object[0];
        public event HproseErrorEvent OnError = null;
        private static SynchronizationContext syncContext = SynchronizationContext.Current;
        public static SynchronizationContext SynchronizationContext {
            get {
                if (syncContext == null) {
                    syncContext = new SynchronizationContext();
                }
                return syncContext;
            }
            set {
                syncContext = value;
            }
        }
        private abstract class AsyncInvokeContextBase {
            private HproseClient client;
            private Type returnType;
            private bool byRef;
            private HproseResultMode resultMode;
            private bool simple;
            private SynchronizationContext syncContext;
            protected string functionName;
            protected object[] arguments;
            protected HproseErrorEvent errorCallback;
            public AsyncInvokeContextBase(HproseClient client, string functionName, object[] arguments, HproseErrorEvent errorCallback, Type returnType, bool byRef, HproseResultMode resultMode, bool simple) {
                this.client = client;
                this.functionName = functionName;
                this.arguments = arguments;
                this.errorCallback = (errorCallback == null) ? client.OnError : errorCallback;
                this.returnType = returnType;
                this.byRef = byRef;
                this.resultMode = resultMode;
                this.simple = simple;
                this.syncContext = HproseClient.SynchronizationContext;
            }

            public void Invoke() {
                try {
                    client.BeginSendAndReceive(
                        client.DoOutput(functionName, arguments, byRef, simple),
                            new AsyncCallback(SendAndReceiveCallback));
                }
                catch (Exception e) {
                    DoError(e);
                }
            }

            private void SendAndReceiveCallback(IAsyncResult asyncResult) {
                try {
                    object result = client.DoInput(client.EndSendAndReceive(asyncResult),
                                            arguments, returnType, resultMode);
                    if (result is HproseException) {
                        DoError(result);
                    }
                    else {
                        syncContext.Post(new SendOrPostCallback(DoCallback), result);
                    }
                }
                catch (Exception e) {
                    DoError(e);
                }
            }

            private void DoError(object e) {
                if (errorCallback != null) {
                    syncContext.Post(new SendOrPostCallback(DoErrorCallback), e);
                }
            }

            private void DoErrorCallback(object e) {
                errorCallback(functionName, (Exception)e);
            }

            protected abstract void DoCallback(object result);
        }

        private class AsyncInvokeContext : AsyncInvokeContextBase {
            private HproseCallback callback;
            internal AsyncInvokeContext(HproseClient client, string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorCallback, Type returnType, bool byRef, HproseResultMode resultMode, bool simple):
            base(client, functionName, arguments, errorCallback, returnType, byRef, resultMode, simple) {
                this.callback = callback;
            }
            protected override void DoCallback(object result) {
                callback(result, arguments);
            }
        }
        private class AsyncInvokeContext1 : AsyncInvokeContextBase {
            private HproseCallback1 callback;
            internal AsyncInvokeContext1(HproseClient client, string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorCallback, Type returnType, HproseResultMode resultMode, bool simple) :
                base(client, functionName, arguments, errorCallback, returnType, false, resultMode, simple) {
                this.callback = callback;
            }
            protected override void DoCallback(object result) {
                callback(result);
            }
        }
#if !(dotNET10 || dotNET11 || dotNETCF10)
        private class AsyncInvokeContext<T> : AsyncInvokeContextBase {
            private HproseCallback<T> callback;
            internal AsyncInvokeContext(HproseClient client, string functionName, object[] arguments, HproseCallback<T> callback, HproseErrorEvent errorCallback, bool byRef, HproseResultMode resultMode, bool simple) :
                base(client, functionName, arguments, errorCallback, typeof(T), byRef, resultMode, simple) {
                this.callback = callback;
            }
            protected override void DoCallback(object result) {
                callback((T)result, arguments);
            }
        }
        private class AsyncInvokeContext1<T> : AsyncInvokeContextBase {
            private HproseCallback1<T> callback;
            internal AsyncInvokeContext1(HproseClient client, string functionName, object[] arguments, HproseCallback1<T> callback, HproseErrorEvent errorCallback, HproseResultMode resultMode, bool simple) :
                base(client, functionName, arguments, errorCallback, typeof(T), false, resultMode, simple) {
                this.callback = callback;
            }
            protected override void DoCallback(object result) {
                callback((T)result);
            }
        }
#endif
        private HproseMode mode;
        protected string uri = null;

        public delegate HproseClient HproseClientCreator(string uri, HproseMode mode);
#if (dotNET10 || dotNET11 || dotNETCF10)
        private readonly ArrayList filters = new ArrayList();
        private static Hashtable clientFactories = new Hashtable();
#else
        private readonly List<IHproseFilter> filters = new List<IHproseFilter>();
        private static Dictionary<string, HproseClientCreator> clientFactories = new Dictionary<string, HproseClientCreator>();
#endif
        static HproseClient() {
#if !(SILVERLIGHT || WINDOWS_PHONE || Core)
            ServicePointManager.DefaultConnectionLimit = Int32.MaxValue;
            RegisterClientFactory("tcp", new HproseClientCreator(HproseTcpClient.Create));
            RegisterClientFactory("tcp4", new HproseClientCreator(HproseTcpClient.Create));
            RegisterClientFactory("tcp6", new HproseClientCreator(HproseTcpClient.Create));
#endif
            RegisterClientFactory("http", new HproseClientCreator(HproseHttpClient.Create));
            RegisterClientFactory("https", new HproseClientCreator(HproseHttpClient.Create));
        }

        public static HproseClient Create(string uri) {
            return HproseClient.Create(uri, HproseMode.MemberMode);
        }

        public static HproseClient Create(string uri, HproseMode mode) {
            Uri u = new Uri(uri);
#if (dotNET10 || dotNET11 || dotNETCF10)
            HproseClientCreator creator = (HproseClientCreator)clientFactories[u.Scheme];
#else
            HproseClientCreator creator = null;
            clientFactories.TryGetValue(u.Scheme, out creator);
#endif
            if (creator != null) {
                return creator(uri, mode);
            }
            throw new HproseException("The " + u.Scheme + " client isn't implemented.");
        }

        public static void RegisterClientFactory(string scheme, HproseClientCreator creator) {
            clientFactories[scheme.ToLower()] = creator;
        }

        protected HproseClient()
            : this(null, HproseMode.MemberMode) {
        }

        protected HproseClient(HproseMode mode)
            : this(null, mode) {
        }

        protected HproseClient(string uri)
            : this(uri, HproseMode.MemberMode) {
        }

        protected HproseClient(string uri, HproseMode mode) {
        	if (uri != null) {
	            UseService(uri);
        	}
            this.mode = mode;
        }

        public IHproseFilter Filter {
            get {
                if (filters.Count == 0) {
                    return null;
                }
#if (dotNET10 || dotNET11 || dotNETCF10)
                return (IHproseFilter)filters[0];
#else
                return filters[0];
#endif
            }
            set {
                if (filters.Count > 0) {
                    filters.Clear();
                }
                if (value != null) {
                    filters.Add(value);
                }
            }
        }

        public void AddFilter(IHproseFilter filter) {
            filters.Add(filter);
        }

        public bool RemoveFilter(IHproseFilter filter) {
#if (dotNET10 || dotNET11 || dotNETCF10)
            if (filters.Contains(filter)) {
                filters.Remove(filter);
                return true;
            }
            return false;
#else
            return filters.Remove(filter);
#endif
        }

        public virtual void UseService(string uri) {
            this.uri = uri;
        }

#if !(PocketPC || Smartphone || WindowsCE || WINDOWS_PHONE || Core)
        public object UseService(Type type) {
            return UseService(type, null);
        }

        public object UseService(string uri, Type type) {
            UseService(uri);
            return UseService(type, null);
        }

        public object UseService(Type[] types) {
            return UseService(types, null);
        }

        public object UseService(string uri, Type[] types) {
            UseService(uri);
            return UseService(types, null);
        }
        public object UseService(Type type, string ns) {
            HproseInvocationHandler handler = new HproseInvocationHandler(this, ns);
            if (type.IsInterface) {
                return Proxy.NewInstance(AppDomain.CurrentDomain, new Type[] { type }, handler);
            }
            else {
                return Proxy.NewInstance(AppDomain.CurrentDomain, type.GetInterfaces(), handler);
            }
        }

        public object UseService(string uri, Type type, string ns) {
            UseService(uri);
            return UseService(type, ns);
        }

        public object UseService(Type[] types, string ns) {
            HproseInvocationHandler handler = new HproseInvocationHandler(this, ns);
            return Proxy.NewInstance(AppDomain.CurrentDomain, types, handler);
        }

        public object UseService(string uri, Type[] types, string ns) {
            UseService(uri);
            return UseService(types, ns);
        }
#if !(dotNET10 || dotNET11 || dotNETCF10)
        public T UseService<T>() {
            return UseService<T>(null);
        }
        public T UseService<T>(string ns) {
            Type type = typeof(T);
            HproseInvocationHandler handler = new HproseInvocationHandler(this, ns);
            if (type.IsInterface) {
                return (T)Proxy.NewInstance(AppDomain.CurrentDomain, new Type[] { type }, handler);
            }
            else {
                return (T)Proxy.NewInstance(AppDomain.CurrentDomain, type.GetInterfaces(), handler);
            }
        }
        public T UseService<T>(string uri, string ns) {
            UseService(uri);
            return UseService<T>(ns);
        }
#endif
#endif
#if !(dotNET10 || dotNET11 || dotNETCF10)
        public T Invoke<T>(string functionName) {
            return (T)Invoke(functionName, nullArgs, typeof(T), false, HproseResultMode.Normal, false);
        }
        public T Invoke<T>(string functionName, HproseResultMode resultMode) {
            return (T)Invoke(functionName, nullArgs, typeof(T), false, resultMode, false);
        }
        public T Invoke<T>(string functionName, object[] arguments) {
            return (T)Invoke(functionName, arguments, typeof(T), false, HproseResultMode.Normal, false);
        }
        public T Invoke<T>(string functionName, object[] arguments, HproseResultMode resultMode) {
            return (T)Invoke(functionName, arguments, typeof(T), false, resultMode, false);
        }
        public T Invoke<T>(string functionName, object[] arguments, bool byRef) {
            return (T)Invoke(functionName, arguments, typeof(T), byRef, HproseResultMode.Normal, false);
        }
        public T Invoke<T>(string functionName, object[] arguments, bool byRef, HproseResultMode resultMode) {
            return (T)Invoke(functionName, arguments, typeof(T), byRef, resultMode, false);
        }
        public T Invoke<T>(string functionName, object[] arguments, bool byRef, bool simple) {
            return (T)Invoke(functionName, arguments, typeof(T), byRef, HproseResultMode.Normal, simple);
        }
        public T Invoke<T>(string functionName, object[] arguments, bool byRef, HproseResultMode resultMode, bool simple) {
            return (T)Invoke(functionName, arguments, typeof(T), byRef, resultMode, simple);
        }
#endif
        public object Invoke(string functionName) {
            return Invoke(functionName, nullArgs, (Type)null, false, HproseResultMode.Normal, false);
        }
        public object Invoke(string functionName, object[] arguments) {
            return Invoke(functionName, arguments, (Type)null, false, HproseResultMode.Normal, false);
        }
        public object Invoke(string functionName, object[] arguments, bool byRef) {
            return Invoke(functionName, arguments, (Type)null, byRef, HproseResultMode.Normal, false);
        }
        public object Invoke(string functionName, object[] arguments, bool byRef, bool simple) {
            return Invoke(functionName, arguments, (Type)null, byRef, HproseResultMode.Normal, simple);
        }

        public object Invoke(string functionName, Type returnType) {
            return Invoke(functionName, nullArgs, returnType, false, HproseResultMode.Normal, false);
        }
        public object Invoke(string functionName, object[] arguments, Type returnType) {
            return Invoke(functionName, arguments, returnType, false, HproseResultMode.Normal, false);
        }
        public object Invoke(string functionName, object[] arguments, Type returnType, bool byRef) {
            return Invoke(functionName, arguments, returnType, byRef, HproseResultMode.Normal, false);
        }
        public object Invoke(string functionName, object[] arguments, Type returnType, bool byRef, bool simple) {
            return Invoke(functionName, arguments, returnType, byRef, HproseResultMode.Normal, simple);
        }

        public object Invoke(string functionName, HproseResultMode resultMode) {
            return Invoke(functionName, nullArgs, (Type)null, false, resultMode, false);
        }
        public object Invoke(string functionName, object[] arguments, HproseResultMode resultMode) {
            return Invoke(functionName, arguments, (Type)null, false, resultMode, false);
        }
        public object Invoke(string functionName, object[] arguments, bool byRef, HproseResultMode resultMode) {
            return Invoke(functionName, arguments, (Type)null, byRef, resultMode, false);
        }
        public object Invoke(string functionName, object[] arguments, bool byRef, HproseResultMode resultMode, bool simple) {
            return Invoke(functionName, arguments, (Type)null, byRef, resultMode, simple);
        }

        public object Invoke(string functionName, Type returnType, HproseResultMode resultMode) {
            return Invoke(functionName, nullArgs, returnType, false, resultMode, false);
        }
        public object Invoke(string functionName, object[] arguments, Type returnType, HproseResultMode resultMode) {
            return Invoke(functionName, arguments, returnType, false, resultMode, false);
        }
        public object Invoke(string functionName, object[] arguments, Type returnType, bool byRef, HproseResultMode resultMode) {
            return Invoke(functionName, arguments, returnType, byRef, resultMode, false);
        }
#if !(SILVERLIGHT || WINDOWS_PHONE || Core)
        public object Invoke(string functionName, object[] arguments, Type returnType, bool byRef, HproseResultMode resultMode, bool simple) {
            object result = DoInput(
                                SendAndReceive(
                                    DoOutput(functionName, arguments, byRef, simple)),
                                        arguments, returnType, resultMode);
            if (result is HproseException) {
                throw (HproseException)result;
            }
            return result;
        }
#else
        public object Invoke(string functionName, object[] arguments, Type returnType, bool byRef, HproseResultMode resultMode, bool simple) {
            object result = null;
            Exception error = null;
            AutoResetEvent done = new AutoResetEvent(false);
            Invoke(functionName, arguments,
                delegate(object res, object[] args) {
                    result = res;
                    if (byRef) {
                        int length = arguments.Length;
                        if (length > args.Length) length = args.Length;
                        Array.Copy(args, 0, arguments, 0, length);
                    }
                    done.Set();
                },
                delegate(string name, Exception e) {
                    error = e;
                    done.Set();
                },
                returnType, byRef, resultMode, simple);
            done.WaitOne();
#if (SL2 || SL3)
            done.Close();
#else
            done.Dispose();
#endif
            if (error != null) throw error;
            return result;
        }

#endif
#if !(dotNET10 || dotNET11 || dotNETCF10)
        public void Invoke<T>(string functionName, HproseCallback<T> callback) {
            Invoke(functionName, nullArgs, callback, null, false, HproseResultMode.Normal, false);
        }
        public void Invoke<T>(string functionName, HproseCallback<T> callback, HproseResultMode resultMode) {
            Invoke(functionName, nullArgs, callback, null, false, resultMode, false);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback) {
            Invoke(functionName, arguments, callback, null, false, HproseResultMode.Normal, false);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, null, false, resultMode, false);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, bool byRef) {
            Invoke(functionName, arguments, callback, null, byRef, HproseResultMode.Normal, false);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, bool byRef, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, null, byRef, resultMode, false);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, bool byRef, bool simple) {
            Invoke(functionName, arguments, callback, null, byRef, HproseResultMode.Normal, simple);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, bool byRef, HproseResultMode resultMode, bool simple) {
            Invoke(functionName, arguments, callback, null, byRef, resultMode, simple);
        }

        public void Invoke<T>(string functionName, HproseCallback1<T> callback) {
            Invoke(functionName, nullArgs, callback, null, HproseResultMode.Normal, false);
        }
        public void Invoke<T>(string functionName, HproseCallback1<T> callback, HproseResultMode resultMode) {
            Invoke(functionName, nullArgs, callback, null, resultMode, false);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback1<T> callback) {
            Invoke(functionName, arguments, callback, null, HproseResultMode.Normal, false);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback1<T> callback, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, null, resultMode, false);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback1<T> callback, bool simple) {
            Invoke(functionName, arguments, callback, null, HproseResultMode.Normal, simple);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback1<T> callback, HproseResultMode resultMode, bool simple) {
            Invoke(functionName, arguments, callback, null, resultMode, simple);
        }

        public void Invoke<T>(string functionName, HproseCallback<T> callback, HproseErrorEvent errorEvent) {
            Invoke(functionName, nullArgs, callback, errorEvent, false, HproseResultMode.Normal, false);
        }
        public void Invoke<T>(string functionName, HproseCallback<T> callback, HproseErrorEvent errorEvent, HproseResultMode resultMode) {
            Invoke(functionName, nullArgs, callback, errorEvent, false, resultMode, false);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, HproseErrorEvent errorEvent) {
            Invoke(functionName, arguments, callback, errorEvent, false, HproseResultMode.Normal, false);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, HproseErrorEvent errorEvent, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, errorEvent, false, resultMode, false);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, HproseErrorEvent errorEvent, bool byRef) {
            Invoke(functionName, arguments, callback, errorEvent, byRef, HproseResultMode.Normal, false);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, HproseErrorEvent errorEvent, bool byRef, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, errorEvent, byRef, resultMode, false);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, HproseErrorEvent errorEvent, bool byRef, bool simple) {
            Invoke(functionName, arguments, callback, errorEvent, byRef, HproseResultMode.Normal, simple);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, HproseErrorEvent errorEvent, bool byRef, HproseResultMode resultMode, bool simple) {
            (new AsyncInvokeContext<T>(this, functionName, arguments, callback, errorEvent, byRef, resultMode, simple)).Invoke();
        }

        public void Invoke<T>(string functionName, HproseCallback1<T> callback, HproseErrorEvent errorEvent) {
            Invoke(functionName, nullArgs, callback, errorEvent, HproseResultMode.Normal, false);
        }
        public void Invoke<T>(string functionName, HproseCallback1<T> callback, HproseErrorEvent errorEvent, HproseResultMode resultMode) {
            Invoke(functionName, nullArgs, callback, errorEvent, resultMode, false);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback1<T> callback, HproseErrorEvent errorEvent) {
            Invoke(functionName, arguments, callback, errorEvent, HproseResultMode.Normal, false);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback1<T> callback, HproseErrorEvent errorEvent, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, errorEvent, resultMode, false);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback1<T> callback, HproseErrorEvent errorEvent, bool simple) {
            Invoke(functionName, arguments, callback, errorEvent, HproseResultMode.Normal, simple);
        }
        public void Invoke<T>(string functionName, object[] arguments, HproseCallback1<T> callback, HproseErrorEvent errorEvent, HproseResultMode resultMode, bool simple) {
            (new AsyncInvokeContext1<T>(this, functionName, arguments, callback, errorEvent, resultMode, simple)).Invoke();
        }

#endif
        public void Invoke(string functionName, HproseCallback callback) {
            Invoke(functionName, nullArgs, callback, null, null, false, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback) {
            Invoke(functionName, arguments, callback, null, null, false, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, bool byRef) {
            Invoke(functionName, arguments, callback, null, null, byRef, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, bool byRef, bool simple) {
            Invoke(functionName, arguments, callback, null, null, byRef, HproseResultMode.Normal, simple);
        }

        public void Invoke(string functionName, HproseCallback1 callback) {
            Invoke(functionName, nullArgs, callback, null, null, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback1 callback) {
            Invoke(functionName, arguments, callback, null, null, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback1 callback, bool simple) {
            Invoke(functionName, arguments, callback, null, null, HproseResultMode.Normal, simple);
        }

        public void Invoke(string functionName, HproseCallback callback, HproseErrorEvent errorEvent) {
            Invoke(functionName, nullArgs, callback, errorEvent, null, false, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent) {
            Invoke(functionName, arguments, callback, errorEvent, null, false, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, bool byRef) {
            Invoke(functionName, arguments, callback, errorEvent, null, byRef, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, bool byRef, bool simple) {
            Invoke(functionName, arguments, callback, errorEvent, null, byRef, HproseResultMode.Normal, simple);
        }

        public void Invoke(string functionName, HproseCallback1 callback, HproseErrorEvent errorEvent) {
            Invoke(functionName, nullArgs, callback, errorEvent, null, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorEvent) {
            Invoke(functionName, arguments, callback, errorEvent, null, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorEvent, bool simple) {
            Invoke(functionName, arguments, callback, errorEvent, null, HproseResultMode.Normal, simple);
        }

        public void Invoke(string functionName, HproseCallback callback, Type returnType) {
            Invoke(functionName, nullArgs, callback, null, returnType, false, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, Type returnType) {
            Invoke(functionName, arguments, callback, null, returnType, false, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, Type returnType, bool byRef) {
            Invoke(functionName, arguments, callback, null, returnType, byRef, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, Type returnType, bool byRef, bool simple) {
            Invoke(functionName, arguments, callback, null, returnType, byRef, HproseResultMode.Normal, simple);
        }

        public void Invoke(string functionName, HproseCallback1 callback, Type returnType) {
            Invoke(functionName, nullArgs, callback, null, returnType, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback1 callback, Type returnType) {
            Invoke(functionName, arguments, callback, null, returnType, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback1 callback, Type returnType, bool simple) {
            Invoke(functionName, arguments, callback, null, returnType, HproseResultMode.Normal, simple);
        }

        public void Invoke(string functionName, HproseCallback callback, HproseErrorEvent errorEvent, Type returnType) {
            Invoke(functionName, nullArgs, callback, errorEvent, returnType, false, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, Type returnType) {
            Invoke(functionName, arguments, callback, errorEvent, returnType, false, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, Type returnType, bool byRef) {
            Invoke(functionName, arguments, callback, errorEvent, returnType, byRef, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, Type returnType, bool byRef, bool simple) {
            Invoke(functionName, arguments, callback, errorEvent, returnType, byRef, HproseResultMode.Normal, simple);
        }

        public void Invoke(string functionName, HproseCallback1 callback, HproseErrorEvent errorEvent, Type returnType) {
            Invoke(functionName, nullArgs, callback, errorEvent, returnType, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorEvent, Type returnType) {
            Invoke(functionName, arguments, callback, errorEvent, returnType, HproseResultMode.Normal, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorEvent, Type returnType, bool simple) {
            Invoke(functionName, arguments, callback, errorEvent, returnType, HproseResultMode.Normal, simple);
        }

        public void Invoke(string functionName, HproseCallback callback, HproseResultMode resultMode) {
            Invoke(functionName, nullArgs, callback, null, null, false, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, null, null, false, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, bool byRef, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, null, null, byRef, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, bool byRef, HproseResultMode resultMode, bool simple) {
            Invoke(functionName, arguments, callback, null, null, byRef, resultMode, simple);
        }

        public void Invoke(string functionName, HproseCallback1 callback, HproseResultMode resultMode) {
            Invoke(functionName, nullArgs, callback, null, null, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, null, null, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseResultMode resultMode, bool simple) {
            Invoke(functionName, arguments, callback, null, null, resultMode, simple);
        }

        public void Invoke(string functionName, HproseCallback callback, Type returnType, HproseResultMode resultMode) {
            Invoke(functionName, nullArgs, callback, null, returnType, false, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, Type returnType, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, null, returnType, false, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, Type returnType, bool byRef, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, null, returnType, byRef, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, Type returnType, bool byRef, HproseResultMode resultMode, bool simple) {
            Invoke(functionName, arguments, callback, null, returnType, byRef, resultMode, simple);
        }

        public void Invoke(string functionName, HproseCallback1 callback, Type returnType, HproseResultMode resultMode) {
            Invoke(functionName, nullArgs, callback, null, returnType, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback1 callback, Type returnType, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, null, returnType, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback1 callback, Type returnType, HproseResultMode resultMode, bool simple) {
            Invoke(functionName, arguments, callback, null, returnType, resultMode, simple);
        }

        public void Invoke(string functionName, HproseCallback callback, HproseErrorEvent errorEvent, HproseResultMode resultMode) {
            Invoke(functionName, nullArgs, callback, errorEvent, null, false, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, errorEvent, null, false, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, bool byRef, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, errorEvent, null, byRef, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, bool byRef, HproseResultMode resultMode, bool simple) {
            Invoke(functionName, arguments, callback, errorEvent, null, byRef, resultMode, simple);
        }

        public void Invoke(string functionName, HproseCallback1 callback, HproseErrorEvent errorEvent, HproseResultMode resultMode) {
            Invoke(functionName, nullArgs, callback, errorEvent, null, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorEvent, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, errorEvent, null, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorEvent, HproseResultMode resultMode, bool simple) {
            Invoke(functionName, arguments, callback, errorEvent, null, resultMode, simple);
        }

        public void Invoke(string functionName, HproseCallback callback, HproseErrorEvent errorEvent, Type returnType, HproseResultMode resultMode) {
            Invoke(functionName, nullArgs, callback, errorEvent, returnType, false, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, Type returnType, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, errorEvent, returnType, false, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, Type returnType, bool byRef, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, errorEvent, returnType, byRef, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, Type returnType, bool byRef, HproseResultMode resultMode, bool simple) {
            (new AsyncInvokeContext(this, functionName, arguments, callback, errorEvent, returnType, byRef, resultMode, simple)).Invoke();
        }

        public void Invoke(string functionName, HproseCallback1 callback, HproseErrorEvent errorEvent, Type returnType, HproseResultMode resultMode) {
            Invoke(functionName, nullArgs, callback, errorEvent, returnType, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorEvent, Type returnType, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, errorEvent, returnType, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorEvent, Type returnType, HproseResultMode resultMode, bool simple) {
            (new AsyncInvokeContext1(this, functionName, arguments, callback, errorEvent, returnType, resultMode, simple)).Invoke();
        }

        // SyncInvoke
#if !(SILVERLIGHT || WINDOWS_PHONE || Core)
        protected abstract MemoryStream SendAndReceive(MemoryStream data);
#endif
        // AsyncInvoke
        protected abstract IAsyncResult BeginSendAndReceive(MemoryStream data, AsyncCallback callback);

        protected abstract MemoryStream EndSendAndReceive(IAsyncResult asyncResult);

        private MemoryStream DoOutput(string functionName, object[] arguments, bool byRef, bool simple) {
            MemoryStream outData = new MemoryStream();
            HproseWriter writer = new HproseWriter(outData, mode, simple);
            outData.WriteByte(HproseTags.TagCall);
            writer.WriteString(functionName);
            if ((arguments != null) && (arguments.Length > 0 || byRef)) {
                writer.Reset();
                writer.WriteArray(arguments);
                if (byRef) {
                    writer.WriteBoolean(true);
                }
            }
            outData.WriteByte(HproseTags.TagEnd);
            outData.Position = 0;
            for (int i = 0, n = filters.Count; i < n; ++i) {
#if (dotNET10 || dotNET11 || dotNETCF10)
                IHproseFilter filter = (IHproseFilter)filters[i];
                outData = filter.OutputFilter(outData, this);
#else
                outData = filters[i].OutputFilter(outData, this);
#endif
                outData.Position = 0;
            }
            return outData;
        }

        private object MemoryStreamToType(MemoryStream inData, Type returnType) {
            if (returnType == typeof(byte[])) {
                return inData.ToArray();
            } else if (returnType == null ||
                       returnType == typeof(object) ||
                       returnType == typeof(MemoryStream) ||
                       returnType == typeof(Stream)) {
                inData.Position = 0;
                return inData;
            } else {
                throw new HproseException("Can't Convert MemoryStream to Type: " + returnType.ToString());
            }
        }

        private object DoInput(MemoryStream inData, object[] arguments, Type returnType, HproseResultMode resultMode) {
            for (int i = filters.Count - 1; i >= 0; --i) {
                inData.Position = 0;
#if (dotNET10 || dotNET11 || dotNETCF10)
                IHproseFilter filter = (IHproseFilter)filters[i];
                inData = filter.InputFilter(inData, this);
#else
                inData = filters[i].InputFilter(inData, this);
#endif
            }
            inData.Position = inData.Length - 1;
            int tag = inData.ReadByte();
            if (tag != HproseTags.TagEnd) {
                throw new HproseException("Wrong Response: \r\n" + HproseHelper.ReadWrongInfo(inData));
            }
            inData.Position = 0;
            if (resultMode == HproseResultMode.Raw) {
                inData.SetLength(inData.Length - 1);
            }
            if (resultMode == HproseResultMode.RawWithEndTag ||
                resultMode == HproseResultMode.Raw) {
                return MemoryStreamToType(inData, returnType);
            }
            object result = null;
            HproseReader reader = new HproseReader(inData, mode);
            while ((tag = inData.ReadByte()) != HproseTags.TagEnd) {
                switch (tag) {
                    case HproseTags.TagResult:
                        if (resultMode == HproseResultMode.Normal) {
                            reader.Reset();
                            result = reader.Unserialize(returnType);
                        }
                        else {
                            result = MemoryStreamToType(reader.ReadRaw(), returnType);
                        }
                        break;
                    case HproseTags.TagArgument:
                        reader.Reset();
                        Object[] args = reader.ReadObjectArray();
                        int length = arguments.Length;
                        if (length > args.Length) length = args.Length;
                        Array.Copy(args, 0, arguments, 0, length);
                        break;
                    case HproseTags.TagError:
                        reader.Reset();
                        result = new HproseException(reader.ReadString());
                        break;
                    default:
                        throw new HproseException("Wrong Response: \r\n" + HproseHelper.ReadWrongInfo(inData));
                }
            }
            return result;
        }
    }
}