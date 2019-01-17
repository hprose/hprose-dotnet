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
 * TypeManager.cs                                         *
 *                                                        *
 * hprose TypeManager class for C#.                       *
 *                                                        *
 * LastModified: Jan 17, 2019                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

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
        private static readonly Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        private static readonly ConcurrentDictionary<string, Lazy<Type>> typeCache = new ConcurrentDictionary<string, Lazy<Type>>();
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
        private static readonly Func<string, Lazy<Type>> typeFactory = (name) => new Lazy<Type>(() => LoadType(name));
        public static Type GetType(string name) => typeCache.GetOrAdd(name, typeFactory).Value;
        private static Type LoadType(string alias) {
            Type type;
            int length = alias.Length - alias.Replace("_", "").Length;
            if (length > 0) {
#if NETCOREAPP2_1 || NETCOREAPP2_2
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
#if NETCOREAPP2_1 || NETCOREAPP2_2
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
#if NETCOREAPP2_1 || NETCOREAPP2_2
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