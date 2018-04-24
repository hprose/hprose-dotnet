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
 * LastModified: Apr 24, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Hprose.IO.Accessors {
    public static class Accessor {
        public static string UnifiedName(string name) => char.ToLowerInvariant(name[0]) + name.Substring(1);
        public static Type GetMemberType(MemberInfo member) => member is FieldInfo ? ((FieldInfo)member).FieldType : ((PropertyInfo)member).PropertyType;

        private static readonly ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>> _members = new ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>>();
        private static readonly ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>> _fields = new ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>>();
        private static readonly ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>> _properties = new ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>>();

        private static readonly Func<Type, Lazy<Dictionary<string, MemberInfo>>> _fieldsFactory = (type) => new Lazy<Dictionary<string, MemberInfo>>(() => FieldsAccessor.GetFields(type));
        private static readonly Func<Type, Lazy<Dictionary<string, MemberInfo>>> _propertiesFactory = (type) => new Lazy<Dictionary<string, MemberInfo>>(() => PropertiesAccessor.GetProperties(type));
        private static readonly Func<Type, Lazy<Dictionary<string, MemberInfo>>> _membersFactory = (type) => new Lazy<Dictionary<string, MemberInfo>>(() => MembersAccessor.GetMembers(type));

        public static Dictionary<string, MemberInfo> GetMembers(Type type, HproseMode mode) {
            if (type.IsSerializable) {
                switch (mode) {
                    case HproseMode.FieldMode:
                        return _fields.GetOrAdd(type, _fieldsFactory).Value;
                    case HproseMode.PropertyMode:
                        return _properties.GetOrAdd(type, _propertiesFactory).Value;
                }
            }
            return _members.GetOrAdd(type, _membersFactory).Value;
        }
    }
}
