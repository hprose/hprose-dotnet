/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  MembersReader.cs                                        |
|                                                          |
|  MembersReader class for C#.                             |
|                                                          |
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Hprose.IO {
    using static Mode;

    internal delegate void ReadAction<T>(Reader reader, ref T obj);

    internal static class MembersReader {
        private const BindingFlags BindingAttr = BindingFlags.Instance | BindingFlags.Public;
#if !NET35
        private static BlockExpression CreateReadBlock(Dictionary<string, MemberInfo> members, string[] names, ParameterExpression reader, ParameterExpression obj) {
            var length = names.Length;
            if (length == 1 && names[0].Length == 0) {
                length = 0;
            }
            Expression[] expressions = new Expression[length + 1];
            for (var i = 0; i < length; ++i) {
                expressions[i] = members.ContainsKey(names[i]) ? CreateReadMemberExpression(members[names[i]], reader, obj) : Expression.Empty();
            }
            expressions[length] = Expression.Empty();
            return Expression.Block(expressions);
        }

        private static Expression CreateReadMemberExpression(MemberInfo member, ParameterExpression reader, ParameterExpression obj) {
            if (member != null) {
                Type memberType = Accessor.GetMemberType(member);
                var deserializer = Deserializer.GetInstance(memberType);
                return Expression.Assign(
                    member is FieldInfo ?
                        Expression.Field(obj, (FieldInfo)member) :
                        Expression.Property(obj, (PropertyInfo)member),
                    Expression.Call(
                        Expression.Constant(deserializer),
                        deserializer.GetType().GetMethod("Deserialize", BindingAttr),
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

        internal static ReadAction<T> CreateReadAction<T>(Dictionary<string, MemberInfo> members, string[] names) {
            var reader = Expression.Parameter(typeof(Reader), "reader");
            var obj = Expression.Parameter(typeof(T).MakeByRefType(), "obj");
            var block = CreateReadBlock(members, names, reader, obj);
            return Expression.Lambda<ReadAction<T>>(block, reader, obj).Compile();
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
#else
        internal static ReadAction<T> CreateReadAction<T>(Dictionary<string, MemberInfo> members, string[] names) {
            var n = names.Length;
            var memberReadActions = new ReadAction<T>[n];
            for (int i = 0; i < n; ++i) {
                memberReadActions[i] = CreateReadMemberAction<T>(members[names[i]]);
            }
            return (Reader reader, ref T obj) => {
               for (int i = 0; i < n; ++i) {
                    memberReadActions[i](reader, ref obj);
                }
            };
        }
        internal static ReadAction<T> CreateReadMemberAction<T>(MemberInfo member) {
            if (member != null) {
                Type memberType = Accessor.GetMemberType(member);
                var deserializer = Deserializer.GetInstance(memberType);
                if (member is FieldInfo) {
                    return (Reader reader, ref T obj) => {
                        ((FieldInfo)member).SetValue(obj, deserializer.Deserialize(reader));
                    };
                }
                else {
                    return (Reader reader, ref T obj) => {
                        ((PropertyInfo)member).SetValue(obj, deserializer.Deserialize(reader), null);
                    };
                }
            }
            return (Reader reader, ref T obj) => {
                Deserializer.Instance.Deserialize(reader);
            };
        }
#endif
        public static void ReadAllMembers<T>(Reader reader, string key, ref T obj) {
#if !NET35_CF
            if (typeof(T).IsSerializable) {
#else
            if ((typeof(T).Attributes & TypeAttributes.Serializable) == TypeAttributes.Serializable) {
#endif
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
#if !NET35_CF
            if (typeof(T).IsSerializable) {
#else
            if ((typeof(T).Attributes & TypeAttributes.Serializable) == TypeAttributes.Serializable) {
#endif
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

    internal static class MembersReader<T> {
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> readActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> readMemberActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
#if !NET35_CF
        private static readonly Func<string, Lazy<ReadAction<T>>> readActionFactory = (key) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadAction<T>(MembersAccessor<T>.Members, key.Split(' ')));
        private static readonly Func<string, Lazy<ReadAction<T>>> readMemberActionFactory = (name) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadMemberAction<T>(MembersAccessor<T>.Members[name]));
#else
        private static readonly Func2<string, Lazy<ReadAction<T>>> readActionFactory = (key) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadAction<T>(MembersAccessor<T>.Members, key.Split(' ')));
        private static readonly Func2<string, Lazy<ReadAction<T>>> readMemberActionFactory = (name) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadMemberAction<T>(MembersAccessor<T>.Members[name]));
#endif
        internal static void ReadAllMembers(Reader reader, string key, ref T obj) => readActions.GetOrAdd(key, readActionFactory).Value(reader, ref obj);
        internal static void ReadMember(Reader reader, string name, ref T obj) => readMemberActions.GetOrAdd(name, readMemberActionFactory).Value(reader, ref obj);
    }

    internal static class FieldsReader<T> {
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> readActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> readMemberActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
#if !NET35_CF
        private static readonly Func<string, Lazy<ReadAction<T>>> readActionFactory = (key) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadAction<T>(FieldsAccessor<T>.Fields, key.Split(' ')));
        private static readonly Func<string, Lazy<ReadAction<T>>> readMemberActionFactory = (name) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadMemberAction<T>(FieldsAccessor<T>.Fields[name]));
#else
        private static readonly Func2<string, Lazy<ReadAction<T>>> readActionFactory = (key) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadAction<T>(FieldsAccessor<T>.Fields, key.Split(' ')));
        private static readonly Func2<string, Lazy<ReadAction<T>>> readMemberActionFactory = (name) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadMemberAction<T>(FieldsAccessor<T>.Fields[name]));
#endif
        internal static void ReadAllMembers(Reader reader, string key, ref T obj) => readActions.GetOrAdd(key, readActionFactory).Value(reader, ref obj);
        internal static void ReadMember(Reader reader, string name, ref T obj) => readMemberActions.GetOrAdd(name, readMemberActionFactory).Value(reader, ref obj);
    }

    internal static class PropertiesReader<T> {
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> readActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> readMemberActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
#if !NET35_CF
        private static readonly Func<string, Lazy<ReadAction<T>>> readActionFactory = (key) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadAction<T>(PropertiesAccessor<T>.Properties, key.Split(' ')));
        private static readonly Func<string, Lazy<ReadAction<T>>> readMemberActionFactory = (name) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadMemberAction<T>(PropertiesAccessor<T>.Properties[name]));
#else
        private static readonly Func2<string, Lazy<ReadAction<T>>> readActionFactory = (key) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadAction<T>(PropertiesAccessor<T>.Properties, key.Split(' ')));
        private static readonly Func2<string, Lazy<ReadAction<T>>> readMemberActionFactory = (name) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadMemberAction<T>(PropertiesAccessor<T>.Properties[name]));
#endif
        internal static void ReadAllMembers(Reader reader, string key, ref T obj) => readActions.GetOrAdd(key, readActionFactory).Value(reader, ref obj);
        internal static void ReadMember(Reader reader, string name, ref T obj) => readMemberActions.GetOrAdd(name, readMemberActionFactory).Value(reader, ref obj);
    }
}