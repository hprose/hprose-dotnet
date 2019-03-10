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
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
#if NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0
using System.Text;
#endif

namespace Hprose.IO {
    public static class Accessor {
#if NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0
        public static string UnifiedName(string name) => string.Create(name.Length, name, (Span<char> dist, string src) => {
            src.AsSpan().CopyTo(dist);
            dist[0] = char.ToLower(src[0]);
        });
        public static string TitleCaseName(string name) => string.Create(name.Length, name, (Span<char> dist, string src) => {
            src.AsSpan().CopyTo(dist);
            dist[0] = char.ToUpper(src[0]);
        });
#else
        public static string UnifiedName(string name) => char.ToLower(name[0]) + name.Substring(1);
        public static string TitleCaseName(string name) => char.ToUpper(name[0]) + name.Substring(1);
#endif
        public static Type GetMemberType(MemberInfo member) => member is FieldInfo ? ((FieldInfo)member).FieldType : ((PropertyInfo)member).PropertyType;

        private static readonly ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>> members = new ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>>();
        private static readonly ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>> fields = new ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>>();
        private static readonly ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>> properties = new ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>>();

#if !NET35_CF
        private static readonly Func<Type, Lazy<Dictionary<string, MemberInfo>>> fieldsFactory = (type) => new Lazy<Dictionary<string, MemberInfo>>(() => FieldsAccessor.GetFields(type));
        private static readonly Func<Type, Lazy<Dictionary<string, MemberInfo>>> propertiesFactory = (type) => new Lazy<Dictionary<string, MemberInfo>>(() => PropertiesAccessor.GetProperties(type));
        private static readonly Func<Type, Lazy<Dictionary<string, MemberInfo>>> membersFactory = (type) => new Lazy<Dictionary<string, MemberInfo>>(() => MembersAccessor.GetMembers(type));
#else
        private static readonly Func2<Type, Lazy<Dictionary<string, MemberInfo>>> fieldsFactory = (type) => new Lazy<Dictionary<string, MemberInfo>>(() => FieldsAccessor.GetFields(type));
        private static readonly Func2<Type, Lazy<Dictionary<string, MemberInfo>>> propertiesFactory = (type) => new Lazy<Dictionary<string, MemberInfo>>(() => PropertiesAccessor.GetProperties(type));
        private static readonly Func2<Type, Lazy<Dictionary<string, MemberInfo>>> membersFactory = (type) => new Lazy<Dictionary<string, MemberInfo>>(() => MembersAccessor.GetMembers(type));
#endif
        public static Dictionary<string, MemberInfo> GetMembers(Type type, Mode mode) {
#if !NET35_CF
            if (type.IsSerializable) {
#else
            if ((type.Attributes & TypeAttributes.Serializable) == TypeAttributes.Serializable) {
#endif
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
