/**********************************************************\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: http://www.hprose.com/                 |
|                   http://www.hprose.org/                 |
|                                                          |
\**********************************************************/
/**********************************************************\
 *                                                        *
 * HproseInvocationHandler.cs                             *
 *                                                        *
 * hprose InvocationHandler class for C#.                 *
 *                                                        *
 * LastModified: Jan 18, 2016                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Threading;
using Hprose.IO;
using Hprose.Reflection;
#if (dotNET4 || SL5 || WP80) && !(SL4 || WP70 || WP71)
using System.Threading.Tasks;
#endif

namespace Hprose.Common {
#if (dotNET4 || SL5 || WP80) && !(SL4 || WP70 || WP71)
    interface ITaskCreator {
        Task GetTask(HproseInvoker invoker, string methodName, object[] args, bool byRef, HproseResultMode resultMode, bool simple);
    }
    class TaskCreator<T> : ITaskCreator {
        public Task GetTask(HproseInvoker invoker, string methodName, object[] args, bool byRef, HproseResultMode resultMode, bool simple) {
#if dotNET45
            return Task<T>.Run(delegate() {
                return invoker.Invoke<T>(methodName, args, byRef, resultMode, simple);
            });
#else
            return Task<T>.Factory.StartNew(delegate() {
                return invoker.Invoke<T>(methodName, args, byRef, resultMode, simple);
            }, default(CancellationToken), TaskCreationOptions.None, TaskScheduler.Default);
#endif
        }
    }
#endif
    public class HproseInvocationHandler : IInvocationHandler {
        private String ns;
        private HproseInvoker invoker;

        public HproseInvocationHandler(HproseInvoker invoker, String ns) {
            this.invoker = invoker;
            this.ns = ns;
        }
#if !dotNETMF
        private static Type[] GetTypes(ParameterInfo[] parameters) {
            int n = parameters.Length;
            Type[] types = new Type[n];
            for (int i = 0; i < n; ++i) {
                types[i] = parameters[i].ParameterType;
            }
            return types;
        }
#endif
        private static void CheckResultType(HproseResultMode resultMode, Type returnType) {
            if (resultMode != HproseResultMode.Normal &&
                returnType != null &&
#if (dotNET4 || SL5 || WP80) && !(SL4 || WP70 || WP71)
                returnType != typeof(Task<object>) &&
                returnType != typeof(Task<byte[]>) &&
                returnType != typeof(Task<MemoryStream>) &&
                returnType != typeof(Task<Stream>) &&
#endif
                returnType != typeof(object) &&
                returnType != typeof(byte[]) &&
                returnType != typeof(MemoryStream) &&
                returnType != typeof(Stream)) {
                throw new HproseException("Can't Convert MemoryStream to Type: " + returnType.ToString());
            }
        }
        public object Invoke(object proxy, string methodName, Type[] paramTypes, Type returnType, object[] attrs, object[] args) {
            HproseResultMode resultMode = HproseResultMode.Normal;
            bool simple = false;
            bool byRef = false;
#if !dotNETMF
            foreach (Type param in paramTypes) {
                if (param.IsByRef) {
                    byRef = true;
                    break;
                }
            }
#endif
            foreach (object attr in attrs) {
                if (attr is ResultModeAttribute) {
                    resultMode = (attr as ResultModeAttribute).Value;
                }
                else if (attr is SimpleModeAttribute) {
                    simple = (attr as SimpleModeAttribute).Value;
                }
                else if (attr is ByRefAttribute) {
                    byRef = (attr as ByRefAttribute).Value;
                }
                else if (attr is MethodNameAttribute) {
                    methodName = (attr as MethodNameAttribute).Value;
                }
            }
            CheckResultType(resultMode, returnType);
            if (ns != null && ns != "") {
                methodName = ns + '_' + methodName;
            }
#if (dotNET4 || SL5 || WP80) && !(SL4 || WP70 || WP71)
#if Core
            if (returnType.GetTypeInfo().IsGenericType &&
                returnType.GetGenericTypeDefinition() == typeof(Task<>)) {
                ITaskCreator taskCreator = Activator.CreateInstance(typeof(TaskCreator<>).MakeGenericType(returnType.GenericTypeArguments)) as ITaskCreator;
#else
            if (returnType.IsGenericType &&
                returnType.GetGenericTypeDefinition() == typeof(Task<>)) {
                ITaskCreator taskCreator = Activator.CreateInstance(typeof(TaskCreator<>).MakeGenericType(returnType.GetGenericArguments())) as ITaskCreator;
#endif
                return taskCreator.GetTask(invoker, methodName, args, byRef, resultMode, simple);
            }
            if (returnType == typeof(Task)) {
#if dotNET45
                return Task.Run(delegate() {
                    invoker.Invoke(methodName, args, (Type)null, byRef, resultMode, simple);
                });
#else
                return Task.Factory.StartNew(delegate() {
                    invoker.Invoke(methodName, args, (Type)null, byRef, resultMode, simple);
                }, default(CancellationToken), TaskCreationOptions.None, TaskScheduler.Default);
#endif
            }
#endif
            int n = paramTypes.Length;
            if ((n > 0) && (paramTypes[n - 1] == typeof(HproseCallback))) {
                HproseCallback callback = (HproseCallback)args[n - 1];
                object[] tmpargs = new object[n - 1];
                Array.Copy(args, 0, tmpargs, 0, n - 1);
                invoker.Invoke(methodName, tmpargs, callback, byRef, resultMode, simple);
                return null;
            }
            if ((n > 0) && (paramTypes[n - 1] == typeof(HproseCallback1))) {
                HproseCallback1 callback = (HproseCallback1)args[n - 1];
                object[] tmpargs = new object[n - 1];
                Array.Copy(args, 0, tmpargs, 0, n - 1);
                invoker.Invoke(methodName, tmpargs, callback);
                return null;
            }
            if ((n > 1) && (paramTypes[n - 2] == typeof(HproseCallback)) &&
                           (paramTypes[n - 1] == typeof(Type))) {
                HproseCallback callback = (HproseCallback)args[n - 2];
                returnType = (Type)args[n - 1];
                CheckResultType(resultMode, returnType);
                object[] tmpargs = new object[n - 2];
                Array.Copy(args, 0, tmpargs, 0, n - 2);
                invoker.Invoke(methodName, tmpargs, callback, returnType, byRef, resultMode, simple);
                return null;
            }
            if ((n > 1) && (paramTypes[n - 2] == typeof(HproseCallback1)) &&
                           (paramTypes[n - 1] == typeof(Type))) {
                HproseCallback1 callback = (HproseCallback1)args[n - 2];
                returnType = (Type)args[n - 1];
                CheckResultType(resultMode, returnType);
                object[] tmpargs = new object[n - 2];
                Array.Copy(args, 0, tmpargs, 0, n - 2);
                invoker.Invoke(methodName, tmpargs, callback, returnType, resultMode, simple);
                return null;
            }
            if ((n > 2) && (paramTypes[n - 3] == typeof(HproseCallback)) &&
                           (paramTypes[n - 2] == typeof(HproseErrorEvent)) &&
                           (paramTypes[n - 1] == typeof(Type))) {
                HproseCallback callback = (HproseCallback)args[n - 3];
                HproseErrorEvent errorEvent = (HproseErrorEvent)args[n - 2];
                returnType = (Type)args[n - 1];
                CheckResultType(resultMode, returnType);
                object[] tmpargs = new object[n - 3];
                Array.Copy(args, 0, tmpargs, 0, n - 3);
                invoker.Invoke(methodName, tmpargs, callback, errorEvent, returnType, byRef, resultMode, simple);
                return null;
            }
            if ((n > 2) && (paramTypes[n - 3] == typeof(HproseCallback1)) &&
                           (paramTypes[n - 2] == typeof(HproseErrorEvent)) &&
                           (paramTypes[n - 1] == typeof(Type))) {
                HproseCallback1 callback = (HproseCallback1)args[n - 3];
                HproseErrorEvent errorEvent = (HproseErrorEvent)args[n - 2];
                returnType = (Type)args[n - 1];
                CheckResultType(resultMode, returnType);
                object[] tmpargs = new object[n - 3];
                Array.Copy(args, 0, tmpargs, 0, n - 3);
                invoker.Invoke(methodName, tmpargs, callback, errorEvent, returnType, resultMode, simple);
                return null;
            }
#if !(dotNET10 || dotNET11 || dotNETCF10 || dotNETMF)
#if Core
            if ((n > 0) && paramTypes[n - 1].GetTypeInfo().IsGenericType &&
                           paramTypes[n - 1].GetGenericTypeDefinition() == typeof(HproseCallback<>)) {
                IInvokeHelper helper = Activator.CreateInstance(typeof(InvokeHelper<>).MakeGenericType(paramTypes[n - 1].GenericTypeArguments)) as IInvokeHelper;
#else
            if ((n > 0) && paramTypes[n - 1].IsGenericType &&
                           paramTypes[n - 1].GetGenericTypeDefinition() == typeof(HproseCallback<>)) {
                IInvokeHelper helper = Activator.CreateInstance(typeof(InvokeHelper<>).MakeGenericType(paramTypes[n - 1].GetGenericArguments())) as IInvokeHelper;
#endif
                Delegate callback = (Delegate)args[n - 1];
                object[] tmpargs = new object[n - 1];
                Array.Copy(args, 0, tmpargs, 0, n - 1);
                helper.Invoke(invoker, methodName, tmpargs, callback, null, byRef, resultMode, simple);
                return null;
            }
#if Core
            if ((n > 0) && paramTypes[n - 1].GetTypeInfo().IsGenericType &&
                           paramTypes[n - 1].GetGenericTypeDefinition() == typeof(HproseCallback1<>)) {
                IInvokeHelper1 helper = Activator.CreateInstance(typeof(InvokeHelper1<>).MakeGenericType(paramTypes[n - 1].GenericTypeArguments)) as IInvokeHelper1;
#else
            if ((n > 0) && paramTypes[n - 1].IsGenericType &&
                           paramTypes[n - 1].GetGenericTypeDefinition() == typeof(HproseCallback1<>)) {
                IInvokeHelper1 helper = Activator.CreateInstance(typeof(InvokeHelper1<>).MakeGenericType(paramTypes[n - 1].GetGenericArguments())) as IInvokeHelper1;
#endif
                Delegate callback = (Delegate)args[n - 1];
                object[] tmpargs = new object[n - 1];
                Array.Copy(args, 0, tmpargs, 0, n - 1);
                helper.Invoke(invoker, methodName, tmpargs, callback, null, resultMode, simple);
                return null;
            }
#if Core
            if ((n > 1) && paramTypes[n - 2].GetTypeInfo().IsGenericType &&
                           paramTypes[n - 2].GetGenericTypeDefinition() == typeof(HproseCallback<>) &&
                           paramTypes[n - 1] == typeof(HproseErrorEvent)) {
                IInvokeHelper helper = Activator.CreateInstance(typeof(InvokeHelper<>).MakeGenericType(paramTypes[n - 1].GenericTypeArguments)) as IInvokeHelper;
#else
            if ((n > 1) && paramTypes[n - 2].IsGenericType &&
                           paramTypes[n - 2].GetGenericTypeDefinition() == typeof(HproseCallback<>) &&
                           paramTypes[n - 1] == typeof(HproseErrorEvent)) {
                IInvokeHelper helper = Activator.CreateInstance(typeof(InvokeHelper<>).MakeGenericType(paramTypes[n - 1].GetGenericArguments())) as IInvokeHelper;
#endif
                Delegate callback = (Delegate)args[n - 2];
                HproseErrorEvent errorEvent = (HproseErrorEvent)args[n - 1];
                object[] tmpargs = new object[n - 2];
                Array.Copy(args, 0, tmpargs, 0, n - 2);
                helper.Invoke(invoker, methodName, tmpargs, callback, errorEvent, byRef, resultMode, simple);
                return null;
            }
#if Core
            if ((n > 1) && paramTypes[n - 2].GetTypeInfo().IsGenericType &&
                           paramTypes[n - 2].GetGenericTypeDefinition() == typeof(HproseCallback1<>) &&
                           paramTypes[n - 1] == typeof(HproseErrorEvent)) {
                IInvokeHelper1 helper = Activator.CreateInstance(typeof(InvokeHelper1<>).MakeGenericType(paramTypes[n - 1].GenericTypeArguments)) as IInvokeHelper1;
#else
            if ((n > 1) && paramTypes[n - 2].IsGenericType &&
                           paramTypes[n - 2].GetGenericTypeDefinition() == typeof(HproseCallback1<>) &&
                           paramTypes[n - 1] == typeof(HproseErrorEvent)) {
                IInvokeHelper1 helper = Activator.CreateInstance(typeof(InvokeHelper1<>).MakeGenericType(paramTypes[n - 1].GetGenericArguments())) as IInvokeHelper1;
#endif
                Delegate callback = (Delegate)args[n - 2];
                HproseErrorEvent errorEvent = (HproseErrorEvent)args[n - 1];
                object[] tmpargs = new object[n - 2];
                Array.Copy(args, 0, tmpargs, 0, n - 2);
                helper.Invoke(invoker, methodName, tmpargs, callback, errorEvent, resultMode, simple);
                return null;
            }
#endif
            if (returnType == typeof(void)) {
                returnType = null;
            }
            return invoker.Invoke(methodName, args, returnType, byRef, resultMode, simple);
        }
        public object Invoke(object proxy, MethodInfo method, object[] args) {
#if !dotNETMF
            Invoke(proxy, method.Name, GetTypes(method.GetParameters()), method.ReturnType, method.GetCustomAttributes(true), args);
#else
// TODO: Rewrite the IInvocationHandler and Proxy for .NET MF
            return null;
#endif
        }
    }
}
