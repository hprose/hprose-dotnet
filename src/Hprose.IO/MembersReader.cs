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
 * MembersReader.cs                                       *
 *                                                        *
 * MembersReader class for C#.                            *
 *                                                        *
 * LastModified: Apr 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Hprose.IO {
    using static Mode;

    internal delegate void ReadAction<T>(Reader reader, ref T obj);

    static class MembersReader {
        private const BindingFlags BindingAttr = BindingFlags.Instance | BindingFlags.Public;

        private static BlockExpression CreateReadBlock(Dictionary<string, MemberInfo> members, string[] names, ParameterExpression reader, ParameterExpression obj) {
            var length = names.Length;
            Expression[] expressions = new Expression[length + 1];
            for (var i = 0; i < length; ++i) {
                expressions[i] = CreateReadMemberExpression(members[names[i]], reader, obj);
            }
            expressions[length] = Expression.Empty();
            return Expression.Block(expressions);
        }

        internal static ReadAction<T> CreateReadAction<T>(Dictionary<string, MemberInfo> members, string[] names) {
            var reader = Expression.Parameter(typeof(Reader), "reader");
            var obj = Expression.Parameter(typeof(T).MakeByRefType(), "obj");
            var block = CreateReadBlock(members, names, reader, obj);
            return Expression.Lambda<ReadAction<T>>(block, reader, obj).Compile();
        }

        private static Expression CreateReadMemberExpression(MemberInfo member, ParameterExpression reader, ParameterExpression obj) {
            if (member != null) {
                Type memberType = Accessor.GetMemberType(member);
                var deserializerType = typeof(Deserializer<>).MakeGenericType(memberType);
                var deserializer = deserializerType.GetProperty("Instance").GetValue(null, null);
                return Expression.Assign(
                    member is FieldInfo ?
                        Expression.Field(obj, (FieldInfo)member) :
                        Expression.Property(obj, (PropertyInfo)member),
                    Expression.Call(
                        Expression.Constant(deserializer),
                        deserializerType.GetMethod("Deserialize", BindingAttr),
                        reader
                    )
                );
            }
            return Expression.Call(
                Expression.Constant(Deserializer.Instance),
                typeof(Deserializer).GetMethod("Deserialize", BindingAttr),
                reader
            );
        }

        internal static ReadAction<T> CreateReadMemberAction<T>(MemberInfo member) {
            var reader = Expression.Parameter(typeof(Reader), "reader");
            var obj = Expression.Parameter(typeof(T).MakeByRefType(), "obj");
            BlockExpression body = Expression.Block(new Expression[] {
                CreateReadMemberExpression(member, reader, obj),
                Expression.Empty()
            });
            return Expression.Lambda<ReadAction<T>>(body, reader, obj).Compile();
        }

        public static void ReadAllMembers<T>(Reader reader, string key, ref T obj) {
            if (typeof(T).IsSerializable) {
                switch (reader.Mode) {
                    case FieldMode:
                        FieldsReader<T>.ReadAllMembers(reader, key, ref obj);
                        return;
                    case PropertyMode:
                        PropertiesReader<T>.ReadAllMembers(reader, key, ref obj);
                        return;
                }
            }
            MembersReader<T>.ReadAllMembers(reader, key, ref obj);
        }

        public static void ReadMember<T>(Reader reader, string name, ref T obj) {
            if (typeof(T).IsSerializable) {
                switch (reader.Mode) {
                    case FieldMode:
                        FieldsReader<T>.ReadMember(reader, name, ref obj);
                        return;
                    case PropertyMode:
                        PropertiesReader<T>.ReadMember(reader, name, ref obj);
                        return;
                }
            }
            MembersReader<T>.ReadMember(reader, name, ref obj);
        }
    }

    static class MembersReader<T> {
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> readActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> readMemberActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
        private static readonly Func<string, Lazy<ReadAction<T>>> readActionFactory = (key) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadAction<T>(MembersAccessor<T>.Members, key.Split(' ')));
        private static readonly Func<string, Lazy<ReadAction<T>>> readMemberActionFactory = (name) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadMemberAction<T>(MembersAccessor<T>.Members[name]));
        internal static void ReadAllMembers(Reader reader, string key, ref T obj) => readActions.GetOrAdd(key, readActionFactory).Value(reader, ref obj);
        internal static void ReadMember(Reader reader, string name, ref T obj) => readActions.GetOrAdd(name, readMemberActionFactory).Value(reader, ref obj);
    }

    static class FieldsReader<T> {
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> readActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> readMemberActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
        private static readonly Func<string, Lazy<ReadAction<T>>> readActionFactory = (key) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadAction<T>(FieldsAccessor<T>.Fields, key.Split(' ')));
        private static readonly Func<string, Lazy<ReadAction<T>>> readMemberActionFactory = (name) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadMemberAction<T>(FieldsAccessor<T>.Fields[name]));
        internal static void ReadAllMembers(Reader reader, string key, ref T obj) => readActions.GetOrAdd(key, readActionFactory).Value(reader, ref obj);
        internal static void ReadMember(Reader reader, string name, ref T obj) => readActions.GetOrAdd(name, readMemberActionFactory).Value(reader, ref obj);
    }

    static class PropertiesReader<T> {
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> readActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> readMemberActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
        private static readonly Func<string, Lazy<ReadAction<T>>> readActionFactory = (key) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadAction<T>(PropertiesAccessor<T>.Properties, key.Split(' ')));
        private static readonly Func<string, Lazy<ReadAction<T>>> readMemberActionFactory = (name) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadMemberAction<T>(PropertiesAccessor<T>.Properties[name]));
        internal static void ReadAllMembers(Reader reader, string key, ref T obj) => readActions.GetOrAdd(key, readActionFactory).Value(reader, ref obj);
        internal static void ReadMember(Reader reader, string name, ref T obj) => readActions.GetOrAdd(name, readMemberActionFactory).Value(reader, ref obj);
    }
}