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
 * ObjectDeserializer.cs                                  *
 *                                                        *
 * ObjectDeserializer class for C#.                       *
 *                                                        *
 * LastModified: Apr 27, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

using Hprose.IO.Accessors;

using static Hprose.IO.HproseMode;
using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    delegate void ReadAction<T>(Reader reader, ref T obj);

    static class MembersReader {
        private const BindingFlags BindingAttr = BindingFlags.Instance | BindingFlags.Public;
        private static readonly ConcurrentDictionary<ValueTuple<Type, string>, Lazy<Delegate>> readMembersActions = new ConcurrentDictionary<ValueTuple<Type, string>, Lazy<Delegate>>();
        private static readonly ConcurrentDictionary<ValueTuple<Type, string>, Lazy<Delegate>> readFieldsActions = new ConcurrentDictionary<ValueTuple<Type, string>, Lazy<Delegate>>();
        private static readonly ConcurrentDictionary<ValueTuple<Type, string>, Lazy<Delegate>> readPropertiesActions = new ConcurrentDictionary<ValueTuple<Type, string>, Lazy<Delegate>>();

        private static BlockExpression CreateReadBlock(Dictionary<string, MemberInfo> members, string[] names, ParameterExpression reader, ParameterExpression obj) {
            List<Expression> expressions = new List<Expression>();
            foreach (string name in names) {
                expressions.Add(CreateReadMemberExpression(members[name], reader, obj));
            }
            expressions.Add(Expression.Empty());
            return Expression.Block(expressions);
        }

        public static Delegate CreateReadAction(Type type, Dictionary<string, MemberInfo> members, string[] names) {
            var reader = Expression.Parameter(typeof(Reader), "reader");
            var obj = Expression.Parameter(type.MakeByRefType(), "obj");
            var block = CreateReadBlock(members, names, reader, obj);
            return Expression.Lambda(block, reader, obj).Compile();
        }

        public static ReadAction<T> CreateReadAction<T>(Dictionary<string, MemberInfo> members, string[] names) {
            var reader = Expression.Parameter(typeof(Reader), "reader");
            var obj = Expression.Parameter(typeof(T).MakeByRefType(), "obj");
            var block = CreateReadBlock(members, names, reader, obj);
            return Expression.Lambda<ReadAction<T>>(block, reader, obj).Compile();
        }

        public static ReadAction<T> CreateReadMemberAction<T>(MemberInfo member) {
            var reader = Expression.Parameter(typeof(Reader), "reader");
            var obj = Expression.Parameter(typeof(T).MakeByRefType(), "obj");
            BlockExpression body = Expression.Block(new List<Expression> {
                CreateReadMemberExpression(member, reader, obj),
                Expression.Empty()
            });
            return Expression.Lambda<ReadAction<T>>(body, reader, obj).Compile();
        }

        private static Expression CreateReadMemberExpression(MemberInfo member, ParameterExpression reader, ParameterExpression obj) {
            if (member != null) {
                Type memberType = Accessor.GetMemberType(member);
                var deserializer = typeof(Deserializer<>).MakeGenericType(memberType);
                return Expression.Assign(
                    Expression.PropertyOrField(obj, member.Name),
                    Expression.Call(
                        Expression.Property(null, deserializer, "Instance"),
                        deserializer.GetMethod("Deserialize", BindingAttr),
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

        public static Delegate GetReadAction(Type type, HproseMode mode, string[] names) {
            Lazy<Delegate> delegateFactory((Type, string) _) => new Lazy<Delegate>(() => CreateReadAction(type, Accessor.GetMembers(type, mode), names));
            var key = (type, string.Join(" ", names));
            if (type.IsSerializable) {
                switch (mode) {
                    case FieldMode:
                        return readFieldsActions.GetOrAdd(key, delegateFactory).Value;
                    case PropertyMode:
                        return readPropertiesActions.GetOrAdd(key, delegateFactory).Value;
                }
            }
            return readMembersActions.GetOrAdd(key, delegateFactory).Value;
        }

        public static ReadAction<T> GetReadAction<T>(HproseMode mode, string[] names) {
            if (typeof(T).IsSerializable) {
                switch (mode) {
                    case FieldMode: return FieldsReader<T>.GetReadAction(names);
                    case PropertyMode: return PropertiesReader<T>.GetReadAction(names);
                }
            }
            return MembersReader<T>.GetReadAction(names);
        }

        public static ReadAction<T> GetReadMemberAction<T>(HproseMode mode, string name) {
            if (typeof(T).IsSerializable) {
                switch (mode) {
                    case FieldMode: return FieldsReader<T>.GetReadMemberAction(name);
                    case PropertyMode: return PropertiesReader<T>.GetReadMemberAction(name);
                }
            }
            return MembersReader<T>.GetReadMemberAction(name);
        }
    }

    static class MembersReader<T> {
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> readActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> readMemberActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
        public static ReadAction<T> GetReadAction(string[] names) => readActions.GetOrAdd(string.Join(" ", names), (_) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadAction<T>(MembersAccessor<T>.Members, names))).Value;
        private static readonly Func<string, Lazy<ReadAction<T>>> readMemberActionFactory = (name) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadMemberAction<T>(MembersAccessor<T>.Members[name]));
        public static ReadAction<T> GetReadMemberAction(string name) => readActions.GetOrAdd(name, readMemberActionFactory).Value;
    }

    static class FieldsReader<T> {
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> readActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> readMemberActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
        public static ReadAction<T> GetReadAction(string[] names) => readActions.GetOrAdd(string.Join(" ", names), (_) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadAction<T>(FieldsAccessor<T>.Fields, names))).Value;
        private static readonly Func<string, Lazy<ReadAction<T>>> readMemberActionFactory = (name) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadMemberAction<T>(FieldsAccessor<T>.Fields[name]));
        public static ReadAction<T> GetReadMemberAction(string name) => readActions.GetOrAdd(name, readMemberActionFactory).Value;
    }

    static class PropertiesReader<T> {
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> readActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> readMemberActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
        public static ReadAction<T> GetReadAction(string[] names) => readActions.GetOrAdd(string.Join(" ", names), (_) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadAction<T>(PropertiesAccessor<T>.Properties, names))).Value;
        private static readonly Func<string, Lazy<ReadAction<T>>> readMemberActionFactory = (name) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadMemberAction<T>(PropertiesAccessor<T>.Properties[name]));
        public static ReadAction<T> GetReadMemberAction(string name) => readActions.GetOrAdd(name, readMemberActionFactory).Value;
    }

    class ObjectDeserializer<T> : Deserializer<T> where T : class {
        public static T Read(Reader reader) {
            Stream stream = reader.Stream;
            T obj = (T)Activator.CreateInstance(typeof(T), true);
            reader.SetRef(obj);
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            MembersReader.GetReadAction<T>(reader.Mode, reader[index].names)(reader, ref obj);
            stream.ReadByte();
            return obj;
        }
        public static T ReadMapAsObject(Reader reader) {
            Stream stream = reader.Stream;
            T obj = (T)Activator.CreateInstance(typeof(T), true);
            reader.SetRef(obj);
            int count = ValueReader.ReadCount(stream);
            var strDeserializer = Deserializer<string>.Instance;
            for (int i = 0; i < count; ++i) {
                var name = strDeserializer.Deserialize(reader);
                MembersReader.GetReadMemberAction<T>(reader.Mode, name)(reader, ref obj);
            }
            stream.ReadByte();
            return obj;
        }
        public override T Read(Reader reader, int tag) {
            switch (tag) {
                case TagObject:
                    return Read(reader);
                case TagMap:
                    return ReadMapAsObject(reader);
                case TagEmpty:
                    return (T)Activator.CreateInstance(typeof(T), true);
                default:
                    return base.Read(reader, tag);
            }
        }
    }

    class StructDeserializer<T> : Deserializer<T> where T : struct {
        public static T Read(Reader reader) {
            Stream stream = reader.Stream;
            T obj = new T();
            reader.SetRef(null);
            int refIndex = reader.LastRefIndex;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            MembersReader.GetReadAction<T>(reader.Mode, reader[index].names)(reader, ref obj);
            reader.SetRef(refIndex, obj);
            stream.ReadByte();
            return obj;
        }
        public static T ReadMapAsObject(Reader reader) {
            Stream stream = reader.Stream;
            T obj = new T();
            reader.SetRef(null);
            int refIndex = reader.LastRefIndex;
            int count = ValueReader.ReadCount(stream);
            var strDeserializer = Deserializer<string>.Instance;
            for (int i = 0; i < count; ++i) {
                var name = strDeserializer.Deserialize(reader);
                MembersReader.GetReadMemberAction<T>(reader.Mode, name)(reader, ref obj);
            }
            reader.SetRef(refIndex, obj);
            stream.ReadByte();
            return obj;
        }
        public override T Read(Reader reader, int tag) {
            switch (tag) {
                case TagObject:
                    return Read(reader);
                case TagMap:
                    return ReadMapAsObject(reader);
                case TagEmpty:
                    return new T();
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}