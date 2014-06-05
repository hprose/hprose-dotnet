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
 * hprose ClassManager for C#.                            *
 *                                                        *
 * LastModified: Mar 15, 2014                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;
using System.Collections;
#if !(dotNET10 || dotNET11 || dotNETCF10)
using System.Collections.Generic;
#endif

namespace Hprose.IO {
    public sealed class HproseClassManager {
#if (dotNET10 || dotNET11 || dotNETCF10)
        private static readonly Hashtable classCache1 = new Hashtable();
        private static readonly Hashtable classCache2 = new Hashtable();
#else
        private static readonly Dictionary<Type, string> classCache1 = new Dictionary<Type, string>();
        private static readonly Dictionary<string, Type> classCache2 = new Dictionary<string, Type>();
#endif
        private static readonly object syncRoot = new object();
        public static void Register(Type type, string alias) {
            lock (syncRoot) {
                if (type != null) {
                    classCache1[type] = alias;
                }
                classCache2[alias] = type;
            }
        }
#if !(dotNET10 || dotNET11 || dotNETCF10)
        public static void Register<T>(string alias) {
            Register(typeof(T), alias);
        }
#endif
        public static string GetClassAlias(Type type) {
            lock (syncRoot) {
#if (dotNET10 || dotNET11 || dotNETCF10)
                return (string)classCache1[type];
#else
                string alias = null;
                classCache1.TryGetValue(type, out alias);
                return alias;
#endif
            }
        }

        public static Type GetClass(string alias) {
            lock (syncRoot) {
#if (dotNET10 || dotNET11 || dotNETCF10)
                return (Type)classCache2[alias];
#else
                Type type = null;
                classCache2.TryGetValue(alias, out type);
                return type;
#endif
            }
        }

        public static bool ContainsClass(string alias) {
            lock (syncRoot) {
                return classCache2.ContainsKey(alias);
            }
        }
    }
}