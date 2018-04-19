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
 * Accessor.cs                                            *
 *                                                        *
 * Accessor class for C#.                                 *
 *                                                        *
 * LastModified: Apr 19, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Hprose.IO.Accessors {
    public static class Accessor {
        public static Dictionary<string, MemberInfo> GetMembers<T>() => MembersAccessor<T>.Members;
        public static Dictionary<string, MemberInfo> GetFields<T>() => FieldsAccessor<T>.Fields;
        public static Dictionary<string, MemberInfo> GetProperties<T>() => PropertiesAccessor<T>.Properties;
        public static string UnifiedName(string name) => char.ToLowerInvariant(name[0]) + name.Substring(1);

        private static readonly ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>> _members = new ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>>();
        private static readonly ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>> _fields = new ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>>();
        private static readonly ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>> _properties = new ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>>();
        public static Dictionary<string, MemberInfo> GetMembers(Type type, HproseMode mode) {
            if (type.IsSerializable) {
                switch (mode) {
                    case HproseMode.FieldMode:
                        return _fields.GetOrAdd(type, (t) => new Lazy<Dictionary<string, MemberInfo>>(() => FieldsAccessor.GetFields(t))).Value;
                    case HproseMode.PropertyMode:
                        return _properties.GetOrAdd(type, (t) => new Lazy<Dictionary<string, MemberInfo>>(() => PropertiesAccessor.GetProperties(t))).Value;
                }
            }
            return _members.GetOrAdd(type, (t) => new Lazy<Dictionary<string, MemberInfo>>(() => MembersAccessor.GetMembers(t))).Value;
        }
    }
}
