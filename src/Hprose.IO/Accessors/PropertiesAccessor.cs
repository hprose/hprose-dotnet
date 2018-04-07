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
 * PropertiesAccessor.cs                                  *
 *                                                        *
 * PropertiesAccessor class for C#.                       *
 *                                                        *
 * LastModified: Apr 7, 2018                              *
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
    internal static class PropertiesAccessor {
        public static Dictionary<string, PropertyInfo> GetProperties(Type type) {
            var members = new Dictionary<string, PropertyInfo>();
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
        public static readonly Dictionary<string, PropertyInfo> Properties;
        static PropertiesAccessor() {
            Properties = PropertiesAccessor.GetProperties(typeof(T));
        }
    }
}
