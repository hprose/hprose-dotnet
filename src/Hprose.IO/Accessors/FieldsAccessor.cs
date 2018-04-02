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
 * LastModified: Apr 2, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using static System.Reflection.BindingFlags;

namespace Hprose.IO.Accessors {
    internal static class FieldsAccessor {
        public static IReadOnlyDictionary<string, FieldInfo> GetFields(Type type) {
            var members = new Dictionary<string, FieldInfo>(StringComparer.OrdinalIgnoreCase);
            if (!type.IsSerializable) {
                return members;
            }
            var flags = Public | NonPublic | DeclaredOnly | Instance;
            var ignoreDataMember = typeof(IgnoreDataMemberAttribute);
            while (type != typeof(object) && type.IsSerializable) {
                var fields = type.GetFields(flags);
                foreach (var field in fields) {
                    var dataMember = field.GetCustomAttribute<DataMemberAttribute>(false);
                    string name;
                    if (!field.IsDefined(ignoreDataMember, false) &&
                        !field.IsNotSerialized &&
                        !members.ContainsKey(name = dataMember?.Name ?? field.Name)) {
                        name = char.ToLower(name[0]) + name.Substring(1);
                        members[name] = field;
                    }
                }
                type = type.BaseType;
            }
            return (from entry in members
                    orderby entry.Value.GetCustomAttribute<DataMemberAttribute>(false)?.Order ?? 0
                    select entry).ToDictionary(
                        pair => pair.Key,
                        pair => pair.Value,
                        StringComparer.OrdinalIgnoreCase
                    );
        }
    }
    public static class FieldsAccessor<T> {
        public static readonly IReadOnlyDictionary<string, FieldInfo> Fields;
        static FieldsAccessor() {
            Fields = FieldsAccessor.GetFields(typeof(T));
        }
    }
}
