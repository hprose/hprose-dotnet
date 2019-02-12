/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  PropertiesAccessor.cs                                   |
|                                                          |
|  PropertiesAccessor class for C#.                        |
|                                                          |
|  LastModified: Feb 8, 2019                               |
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
    internal static class PropertiesAccessor {
        public static Dictionary<string, MemberInfo> GetProperties(Type type) {
            var members = new Dictionary<string, MemberInfo>();
            if (!type.IsSerializable) {
                return members;
            }
            var flags = Public | Instance;
            var ignoreDataMember = typeof(IgnoreDataMemberAttribute);
            var properties = type.GetProperties(flags);
            foreach (var property in properties) {
                var dataMember = Attribute.GetCustomAttribute(property, typeof(DataMemberAttribute), false) as DataMemberAttribute;
                string name;
                if (property.CanRead && property.CanWrite &&
                    !property.IsDefined(ignoreDataMember, false) &&
                    property.GetIndexParameters().Length == 0 &&
                    !members.ContainsKey(name = Accessor.UnifiedName(dataMember?.Name ?? property.Name))) {
                    members[name] = property;
                }
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
    public static class PropertiesAccessor<T> {
        public static Dictionary<string, MemberInfo> Properties { get; } = PropertiesAccessor.GetProperties(typeof(T));
    }
}
