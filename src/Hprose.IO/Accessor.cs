/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Accessor.cs                                             |
|                                                          |
|  Accessor class for C#.                                  |
|                                                          |
|  LastModified: Jan 19, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
#if NETCOREAPP2_1 || NETCOREAPP2_2
using System.Text;
#endif

namespace Hprose.IO {
    public static class Accessor {
#if NETCOREAPP2_1 || NETCOREAPP2_2
        public static string UnifiedName(string name) => string.Create(name.Length, name, (Span<char> dist, string src) => {
            src.AsSpan().CopyTo(dist);
            dist[0] = char.ToLowerInvariant(src[0]);
        });
        public static string TitleCaseName(string name) => string.Create(name.Length, name, (Span<char> dist, string src) => {
            src.AsSpan().CopyTo(dist);
            dist[0] = char.ToUpperInvariant(src[0]);
        });
#else
        public static string UnifiedName(string name) => char.ToLowerInvariant(name[0]) + name.Substring(1);
        public static string TitleCaseName(string name) => char.ToUpperInvariant(name[0]) + name.Substring(1);
#endif
        public static Type GetMemberType(MemberInfo member) => member is FieldInfo ? ((FieldInfo)member).FieldType : ((PropertyInfo)member).PropertyType;

        private static readonly ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>> members = new ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>>();
        private static readonly ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>> fields = new ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>>();
        private static readonly ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>> properties = new ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>>();

        private static readonly Func<Type, Lazy<Dictionary<string, MemberInfo>>> fieldsFactory = (type) => new Lazy<Dictionary<string, MemberInfo>>(() => FieldsAccessor.GetFields(type));
        private static readonly Func<Type, Lazy<Dictionary<string, MemberInfo>>> propertiesFactory = (type) => new Lazy<Dictionary<string, MemberInfo>>(() => PropertiesAccessor.GetProperties(type));
        private static readonly Func<Type, Lazy<Dictionary<string, MemberInfo>>> membersFactory = (type) => new Lazy<Dictionary<string, MemberInfo>>(() => MembersAccessor.GetMembers(type));

        public static Dictionary<string, MemberInfo> GetMembers(Type type, Mode mode) {
            if (type.IsSerializable) {
                switch (mode) {
                    case Mode.FieldMode:
                        return fields.GetOrAdd(type, fieldsFactory).Value;
                    case Mode.PropertyMode:
                        return properties.GetOrAdd(type, propertiesFactory).Value;
                }
            }
            return members.GetOrAdd(type, membersFactory).Value;
        }
    }
}
