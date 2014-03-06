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
 * HproseInvocationHandler.cs                             *
 *                                                        *
 * hprose InvocationHandler class for C#.                 *
 *                                                        *
 * LastModified: Mar 6, 2014                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !(PocketPC || Smartphone || WindowsCE || WINDOWS_PHONE || Core)
using System;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Threading;
using Hprose.IO;
using Hprose.Reflection;
#if dotNET45
using System.Threading.Tasks;
#endif

namespace Hprose.Common {
#if dotNET45
    interface ITaskCreator {
        Task GetTask(HproseInvoker invoker, string methodName, object[] args, bool byRef, HproseResultMode resultMode, bool simple);
    }
    class TaskCreator<T> : ITaskCreator {
        public Task GetTask(HproseInvoker invoker, string methodName, object[] args, bool byRef, HproseResultMode resultMode, bool simple) {
            return Task<T>.Run(delegate() {
                return invoker.Invoke<T>(methodName, args, byRef, resultMode, simple);
            });
        }
    }
#endif
    class HproseInvocationHandler : IInvocationHandler {
        private String ns;
        private HproseInvoker invoker;

        public HproseInvocationHandler(HproseInvoker invoker, String ns) {
            this.invoker = invoker;
            this.ns = ns;
        }

        private static Type[] GetTypes(ParameterInfo[] parameters) {
            int n = parameters.Length;
            Type[] types = new Type[n];
            for (int i = 0; i < n; i++) {
                types[i] = parameters[i].ParameterType;
            }
            return types;
        }
        private static void CheckResultType(HproseResultMode resultMode, Type returnType) {
            if (resultMode != HproseResultMode.Normal &&
                returnType != null &&
#if dotNET45
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
        public object Invoke(object proxy, MethodInfo method, object[] args) {
            ParameterInfo[] parameters = method.GetParameters();
            HproseResultMode resultMode = HproseResultMode.Normal;
            bool simple = false;
            string methodName = method.Name;
#if dotNET45
            Attribute rmAttr = method.GetCustomAttribute(typeof(ResultModeAttribute), true);
            if (rmAttr != null) {
                resultMode = (rmAttr as ResultModeAttribute).Value;
            }
            Attribute smAttr = method.GetCustomAttribute(typeof(SimpleModeAttribute), true);
            if (smAttr != null) {
                simple = (smAttr as SimpleModeAttribute).Value;
            }
            Attribute mnAttr = method.GetCustomAttribute(typeof(MethodNameAttribute), true);
            if (mnAttr != null) {
                methodName = (mnAttr as MethodNameAttribute).Value;
            }
#else
            object[] resultModes = method.GetCustomAttributes(typeof(ResultModeAttribute), true);
            if (resultModes.Length == 1) {
                resultMode = (resultModes[0] as ResultModeAttribute).Value;
            }
            object[] simpleModes = method.GetCustomAttributes(typeof(SimpleModeAttribute), true);
            if (simpleModes.Length == 1) {
                simple = (simpleModes[0] as SimpleModeAttribute).Value;
            }
            object[] methodNames = method.GetCustomAttributes(typeof(MethodNameAttribute), true);
            if (methodNames.Length == 1) {
                methodName = (methodNames[0] as MethodNameAttribute).Value;
            }
#endif
            if (ns != null && ns != "") {
                methodName = ns + '_' + methodName;
            }
            Type returnType = method.ReturnType;
            if (returnType == typeof(void)) {
                returnType = null;
            }
            CheckResultType(resultMode, returnType);
            bool byRef = false;
            Type[] paramTypes = GetTypes(parameters);
            foreach (Type param in paramTypes) {
                if (param.IsByRef) {
                    byRef = true;
                    break;
                }
            }
#if dotNET45
            if (returnType.IsGenericType &&
                returnType.GetGenericTypeDefinition() == typeof(Task<>)) {
                ITaskCreator taskCreator = Activator.CreateInstance(typeof(TaskCreator<>).MakeGenericType(returnType.GetGenericArguments())) as ITaskCreator;
                return taskCreator.GetTask(invoker, methodName, args, byRef, resultMode, simple);
            }
            if (returnType == typeof(Task)) {
                return Task.Run(delegate() {
                    invoker.Invoke(methodName, args, (Type)null, byRef, resultMode, simple);
                });
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
#if !(dotNET10 || dotNET11 || dotNETCF10)
            if ((n > 0) && paramTypes[n - 1].IsGenericType &&
                           paramTypes[n - 1].GetGenericTypeDefinition() == typeof(HproseCallback<>)) {
                IInvokeHelper helper = Activator.CreateInstance(typeof(InvokeHelper<>).MakeGenericType(paramTypes[n - 1].GetGenericArguments())) as IInvokeHelper;
                Delegate callback = (Delegate)args[n - 1];
                object[] tmpargs = new object[n - 1];
                Array.Copy(args, 0, tmpargs, 0, n - 1);
                helper.Invoke(invoker, methodName, tmpargs, callback, null, byRef, resultMode, simple);
                return null;
            }
            if ((n > 0) && paramTypes[n - 1].IsGenericType &&
                           paramTypes[n - 1].GetGenericTypeDefinition() == typeof(HproseCallback1<>)) {
                IInvokeHelper1 helper = Activator.CreateInstance(typeof(InvokeHelper1<>).MakeGenericType(paramTypes[n - 1].GetGenericArguments())) as IInvokeHelper1;
                Delegate callback = (Delegate)args[n - 1];
                object[] tmpargs = new object[n - 1];
                Array.Copy(args, 0, tmpargs, 0, n - 1);
                helper.Invoke(invoker, methodName, tmpargs, callback, null, resultMode, simple);
                return null;
            }
            if ((n > 1) && paramTypes[n - 2].IsGenericType &&
                           paramTypes[n - 2].GetGenericTypeDefinition() == typeof(HproseCallback<>) &&
                           paramTypes[n - 1] == typeof(HproseErrorEvent)) {
                IInvokeHelper helper = Activator.CreateInstance(typeof(InvokeHelper<>).MakeGenericType(paramTypes[n - 1].GetGenericArguments())) as IInvokeHelper;
                Delegate callback = (Delegate)args[n - 2];
                HproseErrorEvent errorEvent = (HproseErrorEvent)args[n - 1];
                object[] tmpargs = new object[n - 2];
                Array.Copy(args, 0, tmpargs, 0, n - 2);
                helper.Invoke(invoker, methodName, tmpargs, callback, errorEvent, byRef, resultMode, simple);
                return null;
            }
            if ((n > 1) && paramTypes[n - 2].IsGenericType &&
                           paramTypes[n - 2].GetGenericTypeDefinition() == typeof(HproseCallback1<>) &&
                           paramTypes[n - 1] == typeof(HproseErrorEvent)) {
                IInvokeHelper1 helper = Activator.CreateInstance(typeof(InvokeHelper1<>).MakeGenericType(paramTypes[n - 1].GetGenericArguments())) as IInvokeHelper1;
                Delegate callback = (Delegate)args[n - 2];
                HproseErrorEvent errorEvent = (HproseErrorEvent)args[n - 1];
                object[] tmpargs = new object[n - 2];
                Array.Copy(args, 0, tmpargs, 0, n - 2);
                helper.Invoke(invoker, methodName, tmpargs, callback, errorEvent, resultMode, simple);
                return null;
            }
#endif
            return invoker.Invoke(methodName, args, returnType, byRef, resultMode, simple);
        }
    }
}
#endif