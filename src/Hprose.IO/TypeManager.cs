/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  TypeManager.cs                                          |
|                                                          |
|  hprose TypeManager class for C#.                        |
|                                                          |
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;

namespace Hprose.IO {
    public static class TypeManager {
        static class TypeName<T> {
            private static volatile string name;
            public static string Name {
                get => name;
                set => name = value;
            }
        }
#if NET35_CF
        private static readonly Assembly[] assemblies = new Assembly[] {
            Assembly.GetCallingAssembly(),
            Assembly.GetExecutingAssembly()
        };
#else
        private static readonly Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
#endif
        private static readonly ConcurrentDictionary<string, Lazy<Type>> typeCache = new ConcurrentDictionary<string, Lazy<Type>>();
        private static readonly ConcurrentDictionary<Type, Type> intfCache = new ConcurrentDictionary<Type, Type>();
        public static void Register<T, I>(string name = null) where T : I {
            Type type = typeof(T);
            Type intf = typeof(I);
            if (type.IsInterface) {
                throw new ArgumentException("T must be a class or struct.");
            }
            if (!intf.IsInterface) {
                throw new ArgumentException("I must be a interface.");
            }
            intfCache.AddOrUpdate(intf, (i) => type, (i, _) => type);
            Register<T>(name);
        }
        public static void Register<T>(string name = null) {
            Type type = typeof(T);
            if (name == null || name.Length == 0) {
                name = (Attribute.GetCustomAttribute(type, typeof(DataContractAttribute), false) as DataContractAttribute)?.Name ?? type.ToString();
            }
            name = name.Replace('.', '_').Replace('+', '_');
            int index = name.IndexOf('`');
            if (index > 0) {
                name = name.Substring(0, index);
            }
            typeCache.AddOrUpdate(name,
                (alias) => { TypeName<T>.Name = alias; return new Lazy<Type>(() => type); },
                (alias, _) => { TypeName<T>.Name = alias; return new Lazy<Type>(() => type); }
            );
        }
        public static bool IsRegistered(string name) => typeCache.ContainsKey(name);
        public static string GetName<T>() {
            if (TypeName<T>.Name == null) {
                Register<T>();
            }
            return TypeName<T>.Name;
        }
#if !NET35_CF
        private static readonly Func<string, Lazy<Type>> typeFactory = (name) => new Lazy<Type>(() => LoadType(name));
#else
        private static readonly Func2<string, Lazy<Type>> typeFactory = (name) => new Lazy<Type>(() => LoadType(name));
#endif
        public static Type GetType<I>() {
            intfCache.TryGetValue(typeof(I), out Type result);
            return result;
        }
        public static Type GetType(string name) => typeCache.GetOrAdd(name, typeFactory).Value;
        private static Type LoadType(string alias) {
            Type type;
            int length = alias.Length - alias.Replace("_", "").Length;
            if (length > 0) {
#if NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0
                Span<int> positions = stackalloc int[length];
#else
                int[] positions = new int[length];
#endif
                int pos = alias.IndexOf('_');
                for (int i = 0; i < length; ++i) {
                    positions[i] = pos;
                    pos = alias.IndexOf('_', pos + 1);
                }
                char[] name = alias.ToCharArray();
                type = GetType(name, positions, 0, '.');
                if (type == null) {
                    type = GetType(name, positions, 0, '_');
                }
                if (type == null) {
                    type = GetNestedType(name, positions, 0, '+');
                }
            }
            else {
                type = GetTypeFromAssemblies(alias);
            }
            return type;
        }
        private static Type GetTypeFromAssemblies(string name) {
            Type type = null;
            for (int i = assemblies.Length - 1; type == null && i >= 0; --i) {
                type = assemblies[i].GetType(name);
            }
            return type;
        }
#if NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0
        private static Type GetNestedType(char[] name, Span<int> positions, int i, char c) {
#else
        private static Type GetNestedType(char[] name, int[] positions, int i, char c) {
#endif
            int length = positions.Length;
            Type type;
            if (i < length) {
                name[positions[i++]] = c;
                type = GetNestedType(name, positions, i, '_');
                if (i < length && type == null) {
                    type = GetNestedType(name, positions, i, '+');
                }
            }
            else {
                type = GetTypeFromAssemblies(name.ToString());
            }
            return type;
        }
#if NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0
        private static Type GetType(char[] name, Span<int> positions, int i, char c) {
#else
        private static Type GetType(char[] name, int[] positions, int i, char c) {
#endif
            int length = positions.Length;
            Type type;
            if (i < length) {
                name[positions[i++]] = c;
                type = GetType(name, positions, i, '.');
                if (i < length) {
                    if (type == null) {
                        type = GetType(name, positions, i, '_');
                    }
                    if (type == null) {
                        type = GetNestedType(name, positions, i, '+');
                    }
                }
            }
            else {
                type = GetTypeFromAssemblies(name.ToString());
            }
            return type;
        }
    }
}