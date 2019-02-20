/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  FieldsAccessor.cs                                       |
|                                                          |
|  FieldsAccessor class for C#.                            |
|                                                          |
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using static System.Reflection.BindingFlags;

namespace Hprose.IO {
    internal static class FieldsAccessor {
        public static Dictionary<string, MemberInfo> GetFields(Type type) {
            var members = new Dictionary<string, MemberInfo>();
#if !NET35_CF
            if (!type.IsSerializable) {
#else
            if ((type.Attributes & TypeAttributes.Serializable) != TypeAttributes.Serializable) {
#endif
                return members;
            }
            var flags = Public | NonPublic | DeclaredOnly | Instance;
            var ignoreDataMember = typeof(IgnoreDataMemberAttribute);
            while (type != typeof(object)
#if !NET35_CF
                && type.IsSerializable) {
#else
                && (type.Attributes & TypeAttributes.Serializable) == TypeAttributes.Serializable) {
#endif
                var fields = type.GetFields(flags);
                foreach (var field in fields) {
                    var dataMember = Attribute.GetCustomAttribute(field, typeof(DataMemberAttribute), false) as DataMemberAttribute;
                    string name;
                    if (!field.IsDefined(ignoreDataMember, false) &&
                        !field.IsNotSerialized &&
                        !members.ContainsKey(name = Accessor.UnifiedName(dataMember?.Name ?? field.Name))) {
                        members[name] = field;
                    }
                }
                type = type.BaseType;
            }
            return (from entry in members
                    orderby (Attribute.GetCustomAttribute(entry.Value, typeof(DataMemberAttribute), false) as DataMemberAttribute)?.Order ?? 0
                    select entry).ToDictionary(
                        pair => pair.Key,
                        pair => pair.Value,
                        StringComparer.OrdinalIgnoreCase
                    );
        }
    }
    public static class FieldsAccessor<T> {
        public static Dictionary<string, MemberInfo> Fields { get; } = FieldsAccessor.GetFields(typeof(T));
    }
}