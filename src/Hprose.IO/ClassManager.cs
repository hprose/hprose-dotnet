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
 * ClassManager.cs                                        *
 *                                                        *
 * hprose ClassManager class for C#.                      *
 *                                                        *
 * LastModified: Apr 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;

namespace Hprose.IO {
    public static class ClassManager {
        static class ClassName<T> {
            private static volatile string _name;
            public static string Name {
                get => _name;
                set => _name = value;
            }
        }
        private static readonly Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        private static readonly ConcurrentDictionary<string, Lazy<Type>> classCache = new ConcurrentDictionary<string, Lazy<Type>>();
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
            classCache.AddOrUpdate(name,
                (alias) => { ClassName<T>.Name = alias; return new Lazy<Type>(() => type); },
                (alias, _) => { ClassName<T>.Name = alias; return new Lazy<Type>(() => type); }
            );
        }
        public static bool IsRegistered(string name) {
            return classCache.ContainsKey(name);
        }
        public static string GetName<T>() {
            if (ClassName<T>.Name == null) {
                Register<T>();
            }
            return ClassName<T>.Name;
        }
        public static Type GetType(string name) {
            return classCache.GetOrAdd(name, (alias) => new Lazy<Type>(() => LoadType(alias))).Value;
        }
        private static Type LoadType(string alias) {
            Type type;
            int length = alias.Length - alias.Replace("_", "").Length;
            if (length > 0) {
                int[] positions = new int[length];
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
        private static Type GetNestedType(char[] name, int[] positions, int i, char c) {
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
        private static Type GetType(char[] name, int[] positions, int i, char c) {
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