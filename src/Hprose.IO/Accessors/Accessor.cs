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
 * LastModified: Apr 6, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hprose.IO.Accessors {
    public static class Accessor {
        public static Dictionary<string, MemberInfo> GetMembers<T>() => MembersAccessor<T>.Members;
        public static Dictionary<string, FieldInfo> GetFields<T>() => FieldsAccessor<T>.Fields;
        public static Dictionary<string, PropertyInfo> GetProperties<T>() => PropertiesAccessor<T>.Properties;
        public static string UnifiedName(string name) => char.ToLowerInvariant(name[0]) + name.Substring(1);
    }
}
