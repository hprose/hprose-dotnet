/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  MethodManager.cs                                        |
|                                                          |
|  MethodManager class for C#.                             |
|                                                          |
|  LastModified: Mar 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class MethodManager {
        private readonly string[] instanceMethodsOnObject = { "Equals", "GetHashCode", "GetType", "ToString" };
        private readonly string[] staticMethodsOnObject = { "Equals", "ReferenceEquals" };
        public ConcurrentDictionary<string, ConcurrentDictionary<int, Method>> Methods { get; } = new ConcurrentDictionary<string, ConcurrentDictionary<int, Method>>(StringComparer.OrdinalIgnoreCase);
        public Method Get(string name, int paramCount) {
            if (Methods.TryGetValue(name, out var methods)) {
                if (methods.TryGetValue(paramCount, out var method)) {
                    return method;
                }
            }
            if (name != "*") {
                return Get("*", 2);
            }
            return null;
        }
        public ICollection<string> GetNames() {
            return Methods.Keys;
        }
        public void Remove(string name, int paramCount = -1) {
            if (paramCount < 0) {
                Methods.TryRemove(name, out var temp);
            }
            else {
                if (Methods.TryGetValue(name, out var methods)) {
                    methods.TryRemove(paramCount, out var method);
                }
            }
        }
        public void Add(Method method) {
            ConcurrentDictionary<int, Method> methods;
            if (!Methods.ContainsKey(method.Name)) {
                Methods.TryAdd(method.Name, new ConcurrentDictionary<int, Method>());
            }
            methods = Methods[method.Name];
            var parameters = method.Parameters;
            var n = parameters.Length;
            var autoParams = 0;
            for (int i = 0; i < n; i++) {
                var p = parameters[i];
                if (typeof(Context).IsAssignableFrom(p.ParameterType)) {
                    autoParams = 1;
                }
#if NET35_CF
                else if ((p.Attributes & ParameterAttributes.Optional) != ParameterAttributes.None && (p.Attributes & ParameterAttributes.HasDefault) != ParameterAttributes.None) {
#elif NET40
                else if (p.IsOptional && (p.Attributes & ParameterAttributes.HasDefault) != ParameterAttributes.None) {
#else
                else if (p.IsOptional && p.HasDefaultValue) {
#endif
                    methods.AddOrUpdate(i - autoParams, method, (key, value) => method);
                }
            }
            methods.AddOrUpdate(n - autoParams, method, (key, value) => method);
        }
        public void Add(MethodInfo methodInfo, string name, object target = null) {
            Add(new Method(methodInfo, name, target));
        }
        public void Add(Action action, string name = null) {
            Add(new Method(action.Method, name, action.Target));
        }
        public void Add<T>(Action<T> action, string name = null) {
            Add(new Method(action.Method, name, action.Target));
        }
        public void Add<T1, T2>(Action<T1, T2> action, string name = null) {
            Add(new Method(action.Method, name, action.Target));
        }
        public void Add<T1, T2, T3>(Action<T1, T2, T3> action, string name = null) {
            Add(new Method(action.Method, name, action.Target));
        }
        public void Add<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, string name = null) {
            Add(new Method(action.Method, name, action.Target));
        }
        public void Add<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, string name = null) {
            Add(new Method(action.Method, name, action.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action, string name = null) {
            Add(new Method(action.Method, name, action.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action, string name = null) {
            Add(new Method(action.Method, name, action.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, string name = null) {
            Add(new Method(action.Method, name, action.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, string name = null) {
            Add(new Method(action.Method, name, action.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, string name = null) {
            Add(new Method(action.Method, name, action.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, string name = null) {
            Add(new Method(action.Method, name, action.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, string name = null) {
            Add(new Method(action.Method, name, action.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, string name = null) {
            Add(new Method(action.Method, name, action.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, string name = null) {
            Add(new Method(action.Method, name, action.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, string name = null) {
            Add(new Method(action.Method, name, action.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, string name = null) {
            Add(new Method(action.Method, name, action.Target));
        }
        public void Add<TResult>(Func<TResult> func, string name = null) {
            Add(new Method(func.Method, name, func.Target));
        }
        public void Add<T1, TResult>(Func<T1, TResult> func, string name = null) {
            Add(new Method(func.Method, name, func.Target));
        }
        public void Add<T1, T2, TResult>(Func<T1, T2, TResult> func, string name = null) {
            Add(new Method(func.Method, name, func.Target));
        }
        public void Add<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, string name = null) {
            Add(new Method(func.Method, name, func.Target));
        }
        public void Add<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, string name = null) {
            Add(new Method(func.Method, name, func.Target));
        }
        public void Add<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func, string name = null) {
            Add(new Method(func.Method, name, func.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> func, string name = null) {
            Add(new Method(func.Method, name, func.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, string name = null) {
            Add(new Method(func.Method, name, func.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, string name = null) {
            Add(new Method(func.Method, name, func.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, string name = null) {
            Add(new Method(func.Method, name, func.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, string name = null) {
            Add(new Method(func.Method, name, func.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, string name = null) {
            Add(new Method(func.Method, name, func.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, string name = null) {
            Add(new Method(func.Method, name, func.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, string name = null) {
            Add(new Method(func.Method, name, func.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, string name = null) {
            Add(new Method(func.Method, name, func.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, string name = null) {
            Add(new Method(func.Method, name, func.Target));
        }
        public void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, string name = null) {
            Add(new Method(func.Method, name, func.Target));
        }
        public void AddMethod(string name, object target, string alias = "") {
#if NET35_CF || NET40
            var methodInfos = target.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
#else
            var methodInfos = target.GetType().GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.Instance);
#endif
            if (string.IsNullOrEmpty(alias)) {
                alias = name;
            }
            foreach (var methodInfo in methodInfos) {
                if (methodInfo.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) {
                    Add(methodInfo, alias, target);
                }
            }
        }
        public void AddMethod(string name, Type type, string alias = "") {
#if NET35_CF || NET40
            var methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
#else
            var methodInfos = type.GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.Static);
#endif
            if (string.IsNullOrEmpty(alias)) {
                alias = name;
            }
            foreach (var methodInfo in methodInfos) {
                if (methodInfo.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) {
                    Add(methodInfo, alias);
                }
            }
        }
        public void AddMethods(string[] names, object target, string ns = "") {
            foreach (var name in names) {
                if (string.IsNullOrEmpty(ns)) {
                    AddMethod(name, target, name);
                }
                else {
                    AddMethod(name, target, ns + "_" + name);
                }
            }
        }
        public void AddMethods(string[] names, Type type, string ns = "") {
            foreach (var name in names) {
                if (string.IsNullOrEmpty(ns)) {
                    AddMethod(name, type, name);
                }
                else {
                    AddMethod(name, type, ns + "_" + name);
                }
            }
        }
        public void AddInstanceMethods(object target, string ns = "") {
#if NET35_CF || NET40
            var methodInfos = target.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
#else
            var methodInfos = target.GetType().GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.Instance);
#endif
            foreach (var methodInfo in methodInfos) {
                if (Array.IndexOf(instanceMethodsOnObject, methodInfo.Name) == -1) {
                    var name = methodInfo.Name;
                    if (!string.IsNullOrEmpty(ns)) {
                        name = ns + "_" + name;
                    }
                    Add(methodInfo, name, target);
                }
            }
        }
        public void AddStaticMethods(Type type, string ns = "") {
#if NET35_CF || NET40
            var methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
#else
            var methodInfos = type.GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.Static);
#endif
            foreach (var methodInfo in methodInfos) {
                if (Array.IndexOf(staticMethodsOnObject, methodInfo.Name) == -1) {
                    var name = methodInfo.Name;
                    if (!string.IsNullOrEmpty(ns)) {
                        name = ns + "_" + name;
                    }
                    Add(methodInfo, name);
                }
            }
        }
        public void AddMissingMethod(Func<string, object[], Task<object>> method) {
            Add(new Method(method.Method, "*", method.Target) {
                Missing = true
            });
        }
        public void AddMissingMethod(Func<string, object[], object> method) {
            Add(new Method(method.Method, "*", method.Target) {
                Missing = true
            });
        }
        public void AddMissingMethod(Func<string, object[], Context, Task<object>> method) {
            Add(new Method(method.Method, "*", method.Target) {
                Missing = true
            });
        }
        public void AddMissingMethod(Func<string, object[], Context, object> method) {
            Add(new Method(method.Method, "*", method.Target) {
                Missing = true
            });
        }
    }
}