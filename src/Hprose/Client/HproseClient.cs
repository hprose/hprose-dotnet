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
 * LastModified: Feb 22, 2014                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;
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
#if !(SILVERLIGHT || WINDOWS_PHONE || Core)
        static HproseClient() {
            ServicePointManager.DefaultConnectionLimit = Int32.MaxValue;
        }
#endif
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
            protected object result;
            internal AsyncInvokeContextBase(HproseClient client, string functionName, object[] arguments, HproseErrorEvent errorCallback, Type returnType, bool byRef, HproseResultMode resultMode, bool simple) {
                this.client = client;
                this.functionName = functionName;
                this.arguments = arguments;
                this.errorCallback = errorCallback;
                this.returnType = returnType;
                this.byRef = byRef;
                this.resultMode = resultMode;
                this.simple = simple;
                this.syncContext = HproseClient.SynchronizationContext;
            }

            internal void GetOutputStream(IAsyncResult asyncResult) {
                Stream ostream = null;
                bool success = false;
                try {
                    ostream = client.EndGetOutputStream(asyncResult);
                    client.DoOutput(ostream, functionName, arguments, byRef, simple);
                    success = true;
                }
                catch (Exception e) {
                    result = e;
                    syncContext.Post(new SendOrPostCallback(DoCallback), null);
                    return;
                }
                finally {
                    if (ostream != null) {
                        client.SendData(ostream, asyncResult.AsyncState, success);
                    }
                }
                client.BeginGetInputStream(new AsyncCallback(GetInputStream), asyncResult.AsyncState);
            }

            internal void GetInputStream(IAsyncResult asyncResult) {
                bool success = false;
                result = null;
                Stream istream = null;
                try {
                    istream = client.EndGetInputStream(asyncResult);
                    result = client.DoInput(istream, arguments, returnType, resultMode);
                    success = true;
                }
                catch (Exception e) {
                    result = e;
                }
                finally {
                    if (istream != null) {
                        client.EndInvoke(istream, asyncResult.AsyncState, success);
                    }
                }
                syncContext.Post(new SendOrPostCallback(DoCallback), null);
            }
            protected abstract void DoCallback(object state);
        }

        private class AsyncInvokeContext : AsyncInvokeContextBase {
            private HproseCallback callback;
            internal AsyncInvokeContext(HproseClient client, string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorCallback, Type returnType, bool byRef, HproseResultMode resultMode, bool simple):
            base(client, functionName, arguments, errorCallback, returnType, byRef, resultMode, simple) {
                this.callback = callback;
            }

            protected override void DoCallback(object state) {
                if (result is Exception) {
                    if (errorCallback != null) {
                        errorCallback(functionName, (Exception)result);
                    }
                }
                else {
                    callback(result, arguments);
                }
            }
        }
        private class AsyncInvokeContext1 : AsyncInvokeContextBase {
            private HproseCallback1 callback;
            internal AsyncInvokeContext1(HproseClient client, string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorCallback, Type returnType, HproseResultMode resultMode, bool simple) :
                base(client, functionName, arguments, errorCallback, returnType, false, resultMode, simple) {
                this.callback = callback;
            }

            protected override void DoCallback(object state) {
                if (result is Exception) {
                    if (errorCallback != null) {
                        errorCallback(functionName, (Exception)result);
                    }
                }
                else {
                    callback(result);
                }
            }
        }
#if !(dotNET10 || dotNET11 || dotNETCF10)
        private class AsyncInvokeContext<T> : AsyncInvokeContextBase {
            private HproseCallback<T> callback;
            internal AsyncInvokeContext(HproseClient client, string functionName, object[] arguments, HproseCallback<T> callback, HproseErrorEvent errorCallback, bool byRef, HproseResultMode resultMode, bool simple) :
                base(client, functionName, arguments, errorCallback, typeof(T), byRef, resultMode, simple) {
                this.callback = callback;
            }

            protected override void DoCallback(object state) {
                if (result is Exception) {
                    if (errorCallback != null) {
                        errorCallback(functionName, (Exception)result);
                    }
                }
                else {
                    callback((T)result, arguments);
                }
            }
        }
        private class AsyncInvokeContext1<T> : AsyncInvokeContextBase {
            private HproseCallback1<T> callback;
            internal AsyncInvokeContext1(HproseClient client, string functionName, object[] arguments, HproseCallback1<T> callback, HproseErrorEvent errorCallback, HproseResultMode resultMode, bool simple) :
                base(client, functionName, arguments, errorCallback, typeof(T), false, resultMode, simple) {
                this.callback = callback;
            }

            protected override void DoCallback(object state) {
                if (result is Exception) {
                    if (errorCallback != null) {
                        errorCallback(functionName, (Exception)result);
                    }
                }
                else {
                    callback((T)result);
                }
            }
        }
#endif
        private HproseMode mode;
        private IHproseFilter filter;

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
            filter = null;
        }

        public IHproseFilter Filter {
            get {
                return filter;
            }
            set {
                filter = value;
            }
        }
        public abstract void UseService(string uri);

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
            object context = GetInvokeContext();
            Stream ostream = GetOutputStream(context);
            bool success = false;
            try {
                DoOutput(ostream, functionName, arguments, byRef, simple);
                success = true;
            }
            finally {
                SendData(ostream, context, success);
            }
            object result = null;
            Stream istream = GetInputStream(context);
            success = false;
            try {
                result = DoInput(istream, arguments, returnType, resultMode);
                success = true;
            }
            finally {
                EndInvoke(istream, context, success);
            }
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
        private object DoInput(Stream istream, object[] arguments, Type returnType, HproseResultMode resultMode) {
            int tag;
            if (filter != null) istream = filter.InputFilter(istream);
            object result = null;
            HproseReader hproseReader = new HproseReader(istream, mode);
            MemoryStream memstream = null;
            if (resultMode == HproseResultMode.RawWithEndTag || resultMode == HproseResultMode.Raw) {
                memstream = new MemoryStream();
            }
            while ((tag = istream.ReadByte()) != HproseTags.TagEnd) {
                switch (tag) {
                    case HproseTags.TagResult:
                        if (resultMode == HproseResultMode.Normal) {
                            hproseReader.Reset();
                            result = hproseReader.Unserialize(returnType);
                        }
                        else if (resultMode == HproseResultMode.Serialized) {
                            memstream = hproseReader.ReadRaw();
                        }
                        else {
                            memstream.WriteByte(HproseTags.TagResult);
                            hproseReader.ReadRaw(memstream);
                        }
                        break;
                    case HproseTags.TagArgument:
                        if (resultMode == HproseResultMode.RawWithEndTag || resultMode == HproseResultMode.Raw) {
                            memstream.WriteByte(HproseTags.TagArgument);
                            hproseReader.ReadRaw(memstream);
                        }
                        else {
                            hproseReader.Reset();
                            Object[] args = hproseReader.ReadObjectArray();
                            int length = arguments.Length;
                            if (length > args.Length) length = args.Length;
                            Array.Copy(args, 0, arguments, 0, length);
                        }
                        break;
                    case HproseTags.TagError:
                        if (resultMode == HproseResultMode.RawWithEndTag || resultMode == HproseResultMode.Raw) {
                            memstream.WriteByte(HproseTags.TagError);
                            hproseReader.ReadRaw(memstream);
                        }
                        else {
                            hproseReader.Reset();
                            result = new HproseException(hproseReader.ReadString());
                        }
                        break;
                    default:
                        throw new HproseException("Wrong Resoponse: \r\n" + HproseHelper.ReadWrongInfo(istream, tag));
                }
            }
            if (resultMode != HproseResultMode.Normal) {
                if (resultMode == HproseResultMode.RawWithEndTag) {
                    memstream.WriteByte(HproseTags.TagEnd);
                }
                if (returnType == typeof(byte[])) {
                    result = memstream.ToArray();
                } else if (returnType == null ||
                           returnType == typeof(object) ||
                           returnType == typeof(MemoryStream) ||
                           returnType == typeof(Stream)) {
                    memstream.Position = 0;
                    result = memstream;
                } else {
                    throw new HproseException("Can't Convert MemoryStream to Type: " + returnType.ToString());
                }
            }
            return result;
        }

        private void DoOutput(Stream ostream, string functionName, object[] arguments, bool byRef, bool simple) {
            if (filter != null) ostream = filter.OutputFilter(ostream);
            HproseWriter hproseWriter = new HproseWriter(ostream, mode, simple);
            ostream.WriteByte(HproseTags.TagCall);
            hproseWriter.WriteString(functionName);
            if ((arguments != null) && (arguments.Length > 0 || byRef)) {
                hproseWriter.Reset();
                hproseWriter.WriteArray(arguments);
                if (byRef) {
                    hproseWriter.WriteBoolean(true);
                }
            }
            ostream.WriteByte(HproseTags.TagEnd);
        }

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
            if (errorEvent == null) {
                errorEvent = OnError;
            }
            AsyncInvokeContext<T> context = new AsyncInvokeContext<T>(this, functionName, arguments, callback, errorEvent, byRef, resultMode, simple);
            try {
                BeginGetOutputStream(new AsyncCallback(context.GetOutputStream), GetInvokeContext());
            }
            catch (Exception e) {
                if (errorEvent != null) {
                    errorEvent(functionName, e);
                }
            }
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
            if (errorEvent == null) {
                errorEvent = OnError;
            }
            AsyncInvokeContext1<T> context = new AsyncInvokeContext1<T>(this, functionName, arguments, callback, errorEvent, resultMode, simple);
            try {
                BeginGetOutputStream(new AsyncCallback(context.GetOutputStream), GetInvokeContext());
            }
            catch (Exception e) {
                if (errorEvent != null) {
                    errorEvent(functionName, e);
                }
            }
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
            if (errorEvent == null) {
                errorEvent = OnError;
            }
            AsyncInvokeContext context = new AsyncInvokeContext(this, functionName, arguments, callback, errorEvent, returnType, byRef, resultMode, simple);
            try {
                BeginGetOutputStream(new AsyncCallback(context.GetOutputStream), GetInvokeContext());
            }
            catch (Exception e) {
                if (errorEvent != null) {
                    errorEvent(functionName, e);
                }
            }
        }

        public void Invoke(string functionName, HproseCallback1 callback, HproseErrorEvent errorEvent, Type returnType, HproseResultMode resultMode) {
            Invoke(functionName, nullArgs, callback, errorEvent, returnType, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorEvent, Type returnType, HproseResultMode resultMode) {
            Invoke(functionName, arguments, callback, errorEvent, returnType, resultMode, false);
        }
        public void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorEvent, Type returnType, HproseResultMode resultMode, bool simple) {
            if (errorEvent == null) {
                errorEvent = OnError;
            }
            AsyncInvokeContext1 context = new AsyncInvokeContext1(this, functionName, arguments, callback, errorEvent, returnType, resultMode, simple);
            try {
                BeginGetOutputStream(new AsyncCallback(context.GetOutputStream), GetInvokeContext());
            }
            catch (Exception e) {
                if (errorEvent != null) {
                    errorEvent(functionName, e);
                }
            }
        }

        protected abstract object GetInvokeContext();

        protected abstract void SendData(Stream ostream, object context, bool success);

        protected abstract void EndInvoke(Stream istream, object context, bool success);
#if !(SILVERLIGHT || WINDOWS_PHONE || Core)
        protected abstract Stream GetOutputStream(object context);

        protected abstract Stream GetInputStream(object context);
#endif
        protected abstract IAsyncResult BeginGetOutputStream(AsyncCallback callback, object context);

        protected abstract Stream EndGetOutputStream(IAsyncResult asyncResult);

        protected abstract IAsyncResult BeginGetInputStream(AsyncCallback callback, object context);

        protected abstract Stream EndGetInputStream(IAsyncResult asyncResult);
    }
}