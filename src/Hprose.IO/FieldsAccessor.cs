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
 * FieldsAccessor.cs                                      *
 *                                                        *
 * FieldsAccessor class for C#.                           *
 *                                                        *
 * LastModified: Apr 25, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

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
            if (!type.IsSerializable) {
                return members;
            }
            var flags = Public | NonPublic | DeclaredOnly | Instance;
            var ignoreDataMember = typeof(IgnoreDataMemberAttribute);
            while (type != typeof(object) && type.IsSerializable) {
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
        private static readonly Dictionary<string, MemberInfo> fields = FieldsAccessor.GetFields(typeof(T));
        public static Dictionary<string, MemberInfo> Fields => fields;
    }
}
