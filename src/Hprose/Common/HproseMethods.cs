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
 * HproseMethods.cs                                       *
 *                                                        *
 * hprose remote methods class for C#.                    *
 *                                                        *
 * LastModified: Apr 7, 2014                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;
using System.Collections;
#if !(dotNET10 || dotNET11 || dotNETCF10)
using System.Collections.Generic;
#endif
using System.Reflection;

namespace Hprose.Common {
    public class HproseMethods {

#if !(dotNET10 || dotNET11 || dotNETCF10)
        internal Dictionary<string, Dictionary<int, HproseMethod>> remoteMethods = new Dictionary<string, Dictionary<int, HproseMethod>>(StringComparer.OrdinalIgnoreCase);
#elif MONO
        internal Hashtable remoteMethods = new Hashtable(StringComparer.OrdinalIgnoreCase);
#else
        internal Hashtable remoteMethods = new Hashtable(new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());
#endif
        public HproseMethods() {
        }

        internal HproseMethod GetMethod(string aliasName, int paramCount) {
            if (!remoteMethods.ContainsKey(aliasName)) {
                return null;
            }
#if !(dotNET10 || dotNET11 || dotNETCF10)
            Dictionary<int, HproseMethod> methods = remoteMethods[aliasName];
#else
            Hashtable methods = (Hashtable)remoteMethods[aliasName];
#endif
            if (!methods.ContainsKey(paramCount)) {
                return null;
            }
#if !(dotNET10 || dotNET11 || dotNETCF10)
            return methods[paramCount];
#else
            return (HproseMethod)methods[paramCount];
#endif
        }

#if !(dotNET10 || dotNET11 || dotNETCF10)
        public ICollection<string> AllNames {
#else
        public ICollection AllNames {
#endif
            get {
                return remoteMethods.Keys;
            }
        }

        public int Count {
            get {
                return remoteMethods.Count;
            }
        }

        protected virtual int GetCount(Type[] paramTypes) {
            int i = paramTypes.Length;
            if (i > 0) {
                Type paramType = paramTypes[i - 1];
                if (paramType == typeof(HproseContext)) {
                    --i;
                }
            }
            return i;
        }

        internal void AddMethod(string aliasName, HproseMethod method) {
#if !(dotNET10 || dotNET11 || dotNETCF10)
            Dictionary<int, HproseMethod> methods;
            if (remoteMethods.ContainsKey(aliasName)) {
                methods = remoteMethods[aliasName];
            }
            else {
                methods = new Dictionary<int, HproseMethod>();
            }
#else
            Hashtable methods;
            if (remoteMethods.ContainsKey(aliasName)) {
                methods = (Hashtable)remoteMethods[aliasName];
            }
            else {
                methods = new Hashtable();
            }
#endif
            if (aliasName == "*" &&
                (!((method.paramTypes.Length == 2) &&
                   method.paramTypes[0] == typeof(string) &&
                   method.paramTypes[1] == typeof(object[])))) {
                return;
            }
            int i = GetCount(method.paramTypes);
            methods[i] = method;
            remoteMethods[aliasName] = methods;
        }

        public void AddMethod(MethodInfo method, object obj, string aliasName) {
            AddMethod(aliasName, new HproseMethod(method, obj));
        }

        public void AddMethod(MethodInfo method, object obj, string aliasName, HproseResultMode mode) {
            AddMethod(aliasName, new HproseMethod(method, obj, mode));
        }

        public void AddMethod(MethodInfo method, object obj, string aliasName, bool simple) {
            AddMethod(aliasName, new HproseMethod(method, obj, simple));
        }

        public void AddMethod(MethodInfo method, object obj, string aliasName, HproseResultMode mode, bool simple) {
            AddMethod(aliasName, new HproseMethod(method, obj, mode, simple));
        }

        public void AddMethod(string methodName, object obj, Type[] paramTypes, string aliasName) {
            AddMethod(aliasName, new HproseMethod(methodName, obj, paramTypes));
        }

        public void AddMethod(string methodName, object obj, Type[] paramTypes, string aliasName, HproseResultMode mode) {
            AddMethod(aliasName, new HproseMethod(methodName, obj, paramTypes, mode));
        }

        public void AddMethod(string methodName, object obj, Type[] paramTypes, string aliasName, bool simple) {
            AddMethod(aliasName, new HproseMethod(methodName, obj, paramTypes, simple));
        }

        public void AddMethod(string methodName, object obj, Type[] paramTypes, string aliasName, HproseResultMode mode, bool simple) {
            AddMethod(aliasName, new HproseMethod(methodName, obj, paramTypes, mode, simple));
        }

        public void AddMethod(string methodName, Type type, Type[] paramTypes, string aliasName) {
            AddMethod(aliasName, new HproseMethod(methodName, type, paramTypes));
        }

        public void AddMethod(string methodName, Type type, Type[] paramTypes, string aliasName, HproseResultMode mode) {
            AddMethod(aliasName, new HproseMethod(methodName, type, paramTypes, mode));
        }

        public void AddMethod(string methodName, Type type, Type[] paramTypes, string aliasName, bool simple) {
            AddMethod(aliasName, new HproseMethod(methodName, type, paramTypes, simple));
        }

        public void AddMethod(string methodName, Type type, Type[] paramTypes, string aliasName, HproseResultMode mode, bool simple) {
            AddMethod(aliasName, new HproseMethod(methodName, type, paramTypes, mode, simple));
        }

        public void AddMethod(string methodName, object obj, Type[] paramTypes) {
            AddMethod(methodName, new HproseMethod(methodName, obj, paramTypes));
        }

        public void AddMethod(string methodName, object obj, Type[] paramTypes, HproseResultMode mode) {
            AddMethod(methodName, new HproseMethod(methodName, obj, paramTypes, mode));
        }

        public void AddMethod(string methodName, object obj, Type[] paramTypes, bool simple) {
            AddMethod(methodName, new HproseMethod(methodName, obj, paramTypes, simple));
        }

        public void AddMethod(string methodName, object obj, Type[] paramTypes, HproseResultMode mode, bool simple) {
            AddMethod(methodName, new HproseMethod(methodName, obj, paramTypes, mode, simple));
        }

        public void AddMethod(string methodName, Type type, Type[] paramTypes) {
            AddMethod(methodName, new HproseMethod(methodName, type, paramTypes));
        }

        public void AddMethod(string methodName, Type type, Type[] paramTypes, HproseResultMode mode) {
            AddMethod(methodName, new HproseMethod(methodName, type, paramTypes, mode));
        }

        public void AddMethod(string methodName, Type type, Type[] paramTypes, bool simple) {
            AddMethod(methodName, new HproseMethod(methodName, type, paramTypes, simple));
        }

        public void AddMethod(string methodName, Type type, Type[] paramTypes, HproseResultMode mode, bool simple) {
            AddMethod(methodName, new HproseMethod(methodName, type, paramTypes, mode, simple));
        }

        private void AddMethod(string methodName, object obj, Type type, string aliasName) {
            AddMethod(methodName, obj, type, aliasName, HproseResultMode.Normal, false);
        }

        private void AddMethod(string methodName, object obj, Type type, string aliasName, HproseResultMode mode) {
            AddMethod(methodName, obj, type, aliasName, mode, false);
        }

        private void AddMethod(string methodName, object obj, Type type, string aliasName, bool simple) {
            AddMethod(methodName, obj, type, aliasName, HproseResultMode.Normal, simple);
        }

        private void AddMethod(string methodName, object obj, Type type, string aliasName, HproseResultMode mode, bool simple) {
#if dotNET45
            IEnumerable<MethodInfo> methods = type.GetRuntimeMethods();
            foreach (MethodInfo method in methods) {
                if (method.IsPublic && (method.IsStatic == (obj == null)) && (methodName == method.Name)) {
                    AddMethod(aliasName, new HproseMethod(method, obj, mode, simple));
                }
            }
#else
            BindingFlags flags = (obj == null) ? BindingFlags.Static : BindingFlags.Instance;
            MethodInfo[] methods = type.GetMethods(flags | BindingFlags.Public);
            for (int i = 0; i < methods.Length; ++i) {
                if (methodName == methods[i].Name) {
                    AddMethod(aliasName, new HproseMethod(methods[i], obj, mode, simple));
                }
            }
#endif
        }

        public void AddMethod(string methodName, object obj, string aliasName) {
            AddMethod(methodName, obj, obj.GetType(), aliasName);
        }

        public void AddMethod(string methodName, object obj, string aliasName, HproseResultMode mode) {
            AddMethod(methodName, obj, obj.GetType(), aliasName, mode);
        }

        public void AddMethod(string methodName, object obj, string aliasName, bool simple) {
            AddMethod(methodName, obj, obj.GetType(), aliasName, simple);
        }

        public void AddMethod(string methodName, object obj, string aliasName, HproseResultMode mode, bool simple) {
            AddMethod(methodName, obj, obj.GetType(), aliasName, mode, simple);
        }

        public void AddMethod(string methodName, Type type, string aliasName) {
            AddMethod(methodName, null, type, aliasName);
        }

        public void AddMethod(string methodName, Type type, string aliasName, HproseResultMode mode) {
            AddMethod(methodName, null, type, aliasName, mode);
        }

        public void AddMethod(string methodName, Type type, string aliasName, bool simple) {
            AddMethod(methodName, null, type, aliasName, simple);
        }

        public void AddMethod(string methodName, Type type, string aliasName, HproseResultMode mode, bool simple) {
            AddMethod(methodName, null, type, aliasName, mode, simple);
        }

        public void AddMethod(string methodName, object obj) {
            AddMethod(methodName, obj, methodName);
        }

        public void AddMethod(string methodName, object obj, HproseResultMode mode) {
            AddMethod(methodName, obj, methodName, mode);
        }

        public void AddMethod(string methodName, object obj, bool simple) {
            AddMethod(methodName, obj, methodName, simple);
        }

        public void AddMethod(string methodName, object obj, HproseResultMode mode, bool simple) {
            AddMethod(methodName, obj, methodName, mode, simple);
        }

        public void AddMethod(string methodName, Type type) {
            AddMethod(methodName, type, methodName);
        }

        public void AddMethod(string methodName, Type type, HproseResultMode mode) {
            AddMethod(methodName, type, methodName, mode);
        }

        public void AddMethod(string methodName, Type type, bool simple) {
            AddMethod(methodName, type, methodName, simple);
        }

        public void AddMethod(string methodName, Type type, HproseResultMode mode, bool simple) {
            AddMethod(methodName, type, methodName, mode, simple);
        }

        private void AddMethods(string[] methodNames, object obj, Type type, string[] aliasNames) {
            AddMethods(methodNames, obj, type, aliasNames, HproseResultMode.Normal, false);
        }

        private void AddMethods(string[] methodNames, object obj, Type type, string[] aliasNames, HproseResultMode mode) {
            AddMethods(methodNames, obj, type, aliasNames, mode, false);
        }

        private void AddMethods(string[] methodNames, object obj, Type type, string[] aliasNames, bool simple) {
            AddMethods(methodNames, obj, type, aliasNames, HproseResultMode.Normal, simple);
        }

        private void AddMethods(string[] methodNames, object obj, Type type, string[] aliasNames, HproseResultMode mode, bool simple) {
#if dotNET45
            IEnumerable<MethodInfo> methods = type.GetRuntimeMethods();
            for (int i = 0; i < methodNames.Length; ++i) {
                string methodName = methodNames[i];
                string aliasName = aliasNames[i];
                foreach (MethodInfo method in methods) {
                    if (method.IsPublic && (method.IsStatic == (obj == null)) && (methodName == method.Name)) {
                        AddMethod(aliasName, new HproseMethod(method, obj, mode, simple));
                    }
                }
            }
#else
            BindingFlags flags = (obj == null) ? BindingFlags.Static : BindingFlags.Instance;
            MethodInfo[] methods = type.GetMethods(flags | BindingFlags.Public);
            for (int i = 0; i < methodNames.Length; ++i) {
                string methodName = methodNames[i];
                string aliasName = aliasNames[i];
                for (int j = 0; j < methods.Length; j++) {
                    if (methodName == methods[j].Name) {
                        AddMethod(aliasName, new HproseMethod(methods[j], obj, mode, simple));
                    }
                }
            }
#endif
        }

        private void AddMethods(string[] methodNames, object obj, Type type, string aliasPrefix) {
            AddMethods(methodNames, obj, type, aliasPrefix, HproseResultMode.Normal, false);
        }

        private void AddMethods(string[] methodNames, object obj, Type type, string aliasPrefix, HproseResultMode mode) {
            AddMethods(methodNames, obj, type, aliasPrefix, mode, false);
        }

        private void AddMethods(string[] methodNames, object obj, Type type, string aliasPrefix, bool simple) {
            AddMethods(methodNames, obj, type, aliasPrefix, HproseResultMode.Normal, simple);
        }

        private void AddMethods(string[] methodNames, object obj, Type type, string aliasPrefix, HproseResultMode mode, bool simple) {
            string[] aliasNames = new string[methodNames.Length];
            for (int i = 0; i < methodNames.Length; ++i) {
                aliasNames[i] = aliasPrefix + "_" + methodNames[i];
            }
            AddMethods(methodNames, obj, type, aliasNames, mode, simple);
        }

        private void AddMethods(string[] methodNames, object obj, Type type) {
            AddMethods(methodNames, obj, type, methodNames, HproseResultMode.Normal, false);
        }

        private void AddMethods(string[] methodNames, object obj, Type type, HproseResultMode mode) {
            AddMethods(methodNames, obj, type, methodNames, mode, false);
        }

        private void AddMethods(string[] methodNames, object obj, Type type, bool simple) {
            AddMethods(methodNames, obj, type, methodNames, HproseResultMode.Normal, simple);
        }

        private void AddMethods(string[] methodNames, object obj, Type type, HproseResultMode mode, bool simple) {
            AddMethods(methodNames, obj, type, methodNames, mode, simple);
        }

        public void AddMethods(string[] methodNames, object obj, string[] aliasNames) {
            AddMethods(methodNames, obj, obj.GetType(), aliasNames);
        }

        public void AddMethods(string[] methodNames, object obj, string[] aliasNames, HproseResultMode mode) {
            AddMethods(methodNames, obj, obj.GetType(), aliasNames, mode);
        }

        public void AddMethods(string[] methodNames, object obj, string[] aliasNames, bool simple) {
            AddMethods(methodNames, obj, obj.GetType(), aliasNames, simple);
        }

        public void AddMethods(string[] methodNames, object obj, string[] aliasNames, HproseResultMode mode, bool simple) {
            AddMethods(methodNames, obj, obj.GetType(), aliasNames, mode, simple);
        }

        public void AddMethods(string[] methodNames, object obj, string aliasPrefix) {
            AddMethods(methodNames, obj, obj.GetType(), aliasPrefix);
        }

        public void AddMethods(string[] methodNames, object obj, string aliasPrefix, HproseResultMode mode) {
            AddMethods(methodNames, obj, obj.GetType(), aliasPrefix, mode);
        }

        public void AddMethods(string[] methodNames, object obj, string aliasPrefix, bool simple) {
            AddMethods(methodNames, obj, obj.GetType(), aliasPrefix, simple);
        }

        public void AddMethods(string[] methodNames, object obj, string aliasPrefix, HproseResultMode mode, bool simple) {
            AddMethods(methodNames, obj, obj.GetType(), aliasPrefix, mode, simple);
        }

        public void AddMethods(string[] methodNames, object obj) {
            AddMethods(methodNames, obj, obj.GetType());
        }

        public void AddMethods(string[] methodNames, object obj, HproseResultMode mode) {
            AddMethods(methodNames, obj, obj.GetType(), mode);
        }

        public void AddMethods(string[] methodNames, object obj, bool simple) {
            AddMethods(methodNames, obj, obj.GetType(), simple);
        }

        public void AddMethods(string[] methodNames, object obj, HproseResultMode mode, bool simple) {
            AddMethods(methodNames, obj, obj.GetType(), mode, simple);
        }

        public void AddMethods(string[] methodNames, Type type, string[] aliasNames) {
            AddMethods(methodNames, null, type, aliasNames);
        }

        public void AddMethods(string[] methodNames, Type type, string[] aliasNames, HproseResultMode mode) {
            AddMethods(methodNames, null, type, aliasNames, mode);
        }

        public void AddMethods(string[] methodNames, Type type, string[] aliasNames, bool simple) {
            AddMethods(methodNames, null, type, aliasNames, simple);
        }

        public void AddMethods(string[] methodNames, Type type, string[] aliasNames, HproseResultMode mode, bool simple) {
            AddMethods(methodNames, null, type, aliasNames, mode, simple);
        }

        public void AddMethods(string[] methodNames, Type type, string aliasPrefix) {
            AddMethods(methodNames, null, type, aliasPrefix);
        }

        public void AddMethods(string[] methodNames, Type type, string aliasPrefix, HproseResultMode mode) {
            AddMethods(methodNames, null, type, aliasPrefix, mode);
        }

        public void AddMethods(string[] methodNames, Type type, string aliasPrefix, bool simple) {
            AddMethods(methodNames, null, type, aliasPrefix, simple);
        }

        public void AddMethods(string[] methodNames, Type type, string aliasPrefix, HproseResultMode mode, bool simple) {
            AddMethods(methodNames, null, type, aliasPrefix, mode, simple);
        }

        public void AddMethods(string[] methodNames, Type type) {
            AddMethods(methodNames, null, type);
        }

        public void AddMethods(string[] methodNames, Type type, HproseResultMode mode) {
            AddMethods(methodNames, null, type, mode);
        }

        public void AddMethods(string[] methodNames, Type type, bool simple) {
            AddMethods(methodNames, null, type, simple);
        }

        public void AddMethods(string[] methodNames, Type type, HproseResultMode mode, bool simple) {
            AddMethods(methodNames, null, type, mode, simple);
        }

        public void AddInstanceMethods(object obj, Type type, string aliasPrefix) {
            AddInstanceMethods(obj, type, aliasPrefix, HproseResultMode.Normal, false);
        }

        public void AddInstanceMethods(object obj, Type type, string aliasPrefix, HproseResultMode mode) {
            AddInstanceMethods(obj, type, aliasPrefix, mode, false);
        }

        public void AddInstanceMethods(object obj, Type type, string aliasPrefix, bool simple) {
            AddInstanceMethods(obj, type, aliasPrefix, HproseResultMode.Normal, simple);
        }

        public void AddInstanceMethods(object obj, Type type, string aliasPrefix, HproseResultMode mode, bool simple) {
            if (obj != null) {
#if dotNET45
                IEnumerable<MethodInfo> methods = type.GetTypeInfo().DeclaredMethods;
                foreach (MethodInfo method in methods) {
                    if (method.IsPublic && !(method.IsStatic)) {
                        AddMethod(method, obj, aliasPrefix + "_" + method.Name, mode, simple);
                    }
                }
#else
                MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly |
                                                       BindingFlags.Instance |
                                                       BindingFlags.Public);
                for (int i = 0; i < methods.Length; ++i) {
                    AddMethod(methods[i], obj, aliasPrefix + "_" + methods[i].Name, mode, simple);
                }
#endif
            }
        }

        public void AddInstanceMethods(object obj, Type type) {
            AddInstanceMethods(obj, type, HproseResultMode.Normal, false);
        }

        public void AddInstanceMethods(object obj, Type type, HproseResultMode mode) {
            AddInstanceMethods(obj, type, mode, false);
        }

        public void AddInstanceMethods(object obj, Type type, bool simple) {
            AddInstanceMethods(obj, type, HproseResultMode.Normal, simple);
        }

        public void AddInstanceMethods(object obj, Type type, HproseResultMode mode, bool simple) {
            if (obj != null) {
#if dotNET45
                IEnumerable<MethodInfo> methods = type.GetTypeInfo().DeclaredMethods;
                foreach (MethodInfo method in methods) {
                    if (method.IsPublic && !(method.IsStatic)) {
                        AddMethod(method, obj, method.Name, mode, simple);
                    }
                }
#else
                MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly |
                                                       BindingFlags.Instance |
                                                       BindingFlags.Public);
                for (int i = 0; i < methods.Length; ++i) {
                    AddMethod(methods[i], obj, methods[i].Name, mode, simple);
                }
#endif
            }
        }

        public void AddInstanceMethods(object obj, string aliasPrefix) {
            AddInstanceMethods(obj, obj.GetType(), aliasPrefix);
        }

        public void AddInstanceMethods(object obj, string aliasPrefix, HproseResultMode mode) {
            AddInstanceMethods(obj, obj.GetType(), aliasPrefix, mode);
        }

        public void AddInstanceMethods(object obj, string aliasPrefix, bool simple) {
            AddInstanceMethods(obj, obj.GetType(), aliasPrefix, simple);
        }

        public void AddInstanceMethods(object obj, string aliasPrefix, HproseResultMode mode, bool simple) {
            AddInstanceMethods(obj, obj.GetType(), aliasPrefix, mode, simple);
        }

        public void AddInstanceMethods(object obj) {
            AddInstanceMethods(obj, obj.GetType());
        }

        public void AddInstanceMethods(object obj, HproseResultMode mode) {
            AddInstanceMethods(obj, obj.GetType(), mode);
        }

        public void AddInstanceMethods(object obj, bool simple) {
            AddInstanceMethods(obj, obj.GetType(), simple);
        }

        public void AddInstanceMethods(object obj, HproseResultMode mode, bool simple) {
            AddInstanceMethods(obj, obj.GetType(), mode, simple);
        }

        public void AddStaticMethods(Type type, string aliasPrefix) {
            AddStaticMethods(type, aliasPrefix, HproseResultMode.Normal, false);
        }

        public void AddStaticMethods(Type type, string aliasPrefix, HproseResultMode mode) {
            AddStaticMethods(type, aliasPrefix, mode, false);
        }

        public void AddStaticMethods(Type type, string aliasPrefix, bool simple) {
            AddStaticMethods(type, aliasPrefix, HproseResultMode.Normal, simple);
        }

        public void AddStaticMethods(Type type, string aliasPrefix, HproseResultMode mode, bool simple) {
#if dotNET45
            IEnumerable<MethodInfo> methods = type.GetTypeInfo().DeclaredMethods;
            foreach (MethodInfo method in methods) {
                if (method.IsPublic && method.IsStatic) {
                    AddMethod(method, null, aliasPrefix + "_" + method.Name, mode, simple);
                }
            }
#else
            MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly |
                                                   BindingFlags.Static |
                                                   BindingFlags.Public);
            for (int i = 0; i < methods.Length; ++i) {
                AddMethod(methods[i], null, aliasPrefix + "_" + methods[i].Name, mode, simple);
            }
#endif
        }

        public void AddStaticMethods(Type type) {
            AddStaticMethods(type, HproseResultMode.Normal, false);
        }

        public void AddStaticMethods(Type type, HproseResultMode mode) {
            AddStaticMethods(type, mode, false);
        }

        public void AddStaticMethods(Type type, bool simple) {
            AddStaticMethods(type, HproseResultMode.Normal, simple);
        }

        public void AddStaticMethods(Type type, HproseResultMode mode, bool simple) {
#if dotNET45
            IEnumerable<MethodInfo> methods = type.GetTypeInfo().DeclaredMethods;
            foreach (MethodInfo method in methods) {
                if (method.IsPublic && method.IsStatic) {
                    AddMethod(method, null, method.Name, mode, simple);
                }
            }
#else
            MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly |
                                                   BindingFlags.Static |
                                                   BindingFlags.Public);
            for (int i = 0; i < methods.Length; ++i) {
                AddMethod(methods[i], null, methods[i].Name, mode, simple);
            }
#endif
        }

        public void AddMissingMethod(string methodName, object obj) {
            AddMethod(methodName, obj, new Type[] { typeof(string), typeof(object[]) }, "*");
        }

        public void AddMissingMethod(string methodName, object obj, HproseResultMode mode) {
            AddMethod(methodName, obj, new Type[] { typeof(string), typeof(object[]) }, "*", mode);
        }

        public void AddMissingMethod(string methodName, object obj, bool simple) {
            AddMethod(methodName, obj, new Type[] { typeof(string), typeof(object[]) }, "*", simple);
        }

        public void AddMissingMethod(string methodName, object obj, HproseResultMode mode, bool simple) {
            AddMethod(methodName, obj, new Type[] { typeof(string), typeof(object[]) }, "*", mode, simple);
        }

        public void AddMissingMethod(string methodName, Type type) {
            AddMethod(methodName, type, new Type[] { typeof(string), typeof(object[]) }, "*");
        }

        public void AddMissingMethod(string methodName, Type type, HproseResultMode mode) {
            AddMethod(methodName, type, new Type[] { typeof(string), typeof(object[]) }, "*", mode);
        }

        public void AddMissingMethod(string methodName, Type type, bool simple) {
            AddMethod(methodName, type, new Type[] { typeof(string), typeof(object[]) }, "*", simple);
        }

        public void AddMissingMethod(string methodName, Type type, HproseResultMode mode, bool simple) {
            AddMethod(methodName, type, new Type[] { typeof(string), typeof(object[]) }, "*", mode, simple);
        }
    }
}
