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
|  LastModified: Jan 27, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
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
        public Method Get(string fullname, int paramCount) {
            if (Methods.TryGetValue(fullname, out var methods)) {
                if (methods.TryGetValue(paramCount, out var method)) {
                    return method;
                }
            }
            return null;
        }
        public ICollection<string> GetNames() {
            return Methods.Keys;
        }
        public void Add(Method method) {
            ConcurrentDictionary<int, Method> methods;
            if (!Methods.ContainsKey(method.Fullname)) {
                Methods.TryAdd(method.Fullname, new ConcurrentDictionary<int, Method>());
            }
            methods = Methods[method.Fullname];
            var parameters = method.Parameters;
            var n = parameters.Length;
            var autoParams = 0;
            for (int i = 0; i < n; i++) {
                var p = parameters[i];
                if (p.ParameterType.IsSubclassOf(typeof(Context))) {
                    autoParams = 1;
                }
#if NET40
                else if (p.IsOptional && (p.Attributes & ParameterAttributes.HasDefault) == ParameterAttributes.HasDefault) {
#else
                else if (p.IsOptional && p.HasDefaultValue) {
#endif
                    methods.AddOrUpdate(i - autoParams, method, (key, value) => method);
                }
            }
            methods.AddOrUpdate(n - autoParams, method, (key, value) => method);
        }
        public void Add(MethodInfo methodInfo, string fullname, object target = null) {
            Add(new Method(methodInfo, fullname, target));
        }
        public void AddMethod(string name, object target, string fullname = "") {
#if NET40
            var methodInfos = target.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
#else
            var methodInfos = target.GetType().GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.Instance);
#endif
            if (fullname == null || fullname == "") {
                fullname = name;
            }
            foreach (MethodInfo methodInfo in methodInfos) {
                if (methodInfo.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) {
                    Add(methodInfo, fullname, target);
                }
            }
        }
        public void AddMethod(string name, Type type, string fullname = "") {
#if NET40
            var methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
#else
            var methodInfos = type.GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.Static);
#endif
            if (fullname == null || fullname == "") {
                fullname = name;
            }
            foreach (MethodInfo methodInfo in methodInfos) {
                if (methodInfo.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) {
                    Add(methodInfo, fullname);
                }
            }
        }
        public void AddMethods(string[] names, object target, string ns = "") {
            foreach (string name in names) {
                if (ns == null || ns == "") {
                    AddMethod(name, target, name);
                }
                else {
                    AddMethod(name, target, ns + "_" + name);
                }
            }
        }
        public void AddMethods(string[] names, Type type, string ns = "") {
            foreach (string name in names) {
                if (ns == null || ns == "") {
                    AddMethod(name, type, name);
                }
                else {
                    AddMethod(name, type, ns + "_" + name);
                }
            }
        }
        public void AddInstanceMethods(object target, string ns = "") {
#if NET40
            var methodInfos = target.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
#else
            var methodInfos = target.GetType().GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.Instance);
#endif
            foreach (MethodInfo methodInfo in methodInfos) {
                if (Array.IndexOf(instanceMethodsOnObject, methodInfo.Name) != -1) {
                    var fullname = methodInfo.Name;
                    if (ns == null || ns == "") {
                        fullname = ns + "_" + fullname;
                    }
                    Add(methodInfo, fullname, target);
                }
            }
        }
        public void AddStaticMethods(Type type, string ns = "") {
#if NET40
            var methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
#else
            var methodInfos = type.GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.Static);
#endif
            foreach (MethodInfo methodInfo in methodInfos) {
                if (Array.IndexOf(staticMethodsOnObject, methodInfo.Name) != -1) {
                    var fullname = methodInfo.Name;
                    if (ns == null || ns == "") {
                        fullname = ns + "_" + fullname;
                    }
                    Add(methodInfo, fullname);
                }
            }
        }
        private void AddMissingMethod(Delegate method) {
            Add(new Method(method.Method, "*", method.Target) {
                Missing = true
            });
        }
        public void AddMissingMethod(Func<string, object[], Task<object>> method) {
            AddMissingMethod(method);
        }
        public void AddMissingMethod(Func<string, object[], object> method) {
            AddMissingMethod(method);
        }
        public void AddMissingMethod(Func<string, object[], Context, Task<object>> method) {
            AddMissingMethod(method);
        }
        public void AddMissingMethod(Func<string, object[], Context, object> method) {
            AddMissingMethod(method);
        }
    }
}