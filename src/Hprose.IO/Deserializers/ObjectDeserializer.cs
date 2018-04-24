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
 * LastModified: Apr 24, 2018                             *
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
        public static ReadAction<T> CreateReadAction<T>(Dictionary<string, MemberInfo> members, string[] names) {
            var reader = Expression.Parameter(typeof(Reader), "reader");
            var obj = Expression.Parameter(typeof(T).MakeByRefType(), "obj");
            var deserializer = Expression.Constant(Deserializer.Instance);
            List<Expression> expressions = new List<Expression>();
            foreach (string name in names) {
                var member = members[name];
                if (member != null) {
                    Type memberType = member is FieldInfo ? ((FieldInfo)member).FieldType : ((PropertyInfo)member).PropertyType;
                    var elemDeserializerType = typeof(Deserializer<>).MakeGenericType(memberType);
                    expressions.Add(
                        Expression.Assign(
                            Expression.PropertyOrField(obj, member.Name),
                            Expression.Call(
                                Expression.Property(null, elemDeserializerType, "Instance"),
                                elemDeserializerType.GetMethod("Deserialize", BindingFlags.Instance | BindingFlags.Public),
                                reader
                            )
                        )
                    );
                }
                else {
                    expressions.Add(
                        Expression.Call(
                            deserializer,
                            typeof(Deserializer).GetMethod("Deserialize", BindingFlags.Instance | BindingFlags.Public),
                            reader
                        )
                    );
                }
            }
            expressions.Add(Expression.Empty());
            return Expression.Lambda<ReadAction<T>>(Expression.Block(expressions), new ParameterExpression[] { reader, obj }).Compile();
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
    }

    static class MembersReader<T> {
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> _readActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
        public static ReadAction<T> GetReadAction(string[] names) => _readActions.GetOrAdd(string.Join(" ", names), (_) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadAction<T>(MembersAccessor<T>.Members, names))).Value;
    }

    static class FieldsReader<T> {
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> _readActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
        public static ReadAction<T> GetReadAction(string[] names) => _readActions.GetOrAdd(string.Join(" ", names), (_) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadAction<T>(FieldsAccessor<T>.Fields, names))).Value;
    }

    static class PropertiesReader<T> {
        private static readonly ConcurrentDictionary<string, Lazy<ReadAction<T>>> _readActions = new ConcurrentDictionary<string, Lazy<ReadAction<T>>>();
        public static ReadAction<T> GetReadAction(string[] names) => _readActions.GetOrAdd(string.Join(" ", names), (_) => new Lazy<ReadAction<T>>(() => MembersReader.CreateReadAction<T>(PropertiesAccessor<T>.Properties, names))).Value;
    }

    class ObjectDeserializer<T> : Deserializer<T> where T : class {
        public static T Read(Reader reader) {
            Stream stream = reader.Stream;
            T obj = (T)Activator.CreateInstance(typeof(T), true);
            reader.SetRef(obj);
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            MembersReader.GetReadAction<T>(reader.Mode, reader[index].Members)(reader, ref obj);
            stream.ReadByte();
            return obj;
        }
        public static T ReadMapAsObject(Reader reader) {
            Stream stream = reader.Stream;
            T obj = (T)Activator.CreateInstance(typeof(T), true);
            reader.SetRef(obj);

            return default;
        }
        public override T Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagEmpty:
                    return (T)Activator.CreateInstance(typeof(T), true);
                case TagObject:
                    return Read(reader);
                case TagMap:
                    return ReadMapAsObject(reader);
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
            MembersReader.GetReadAction<T>(reader.Mode, reader[index].Members)(reader, ref obj);
            reader.SetRef(refIndex, obj);
            stream.ReadByte();
            return obj;
        }
        public static T ReadMapAsObject(Reader reader) {
            Stream stream = reader.Stream;
            T obj = new T();
            reader.SetRef(obj);

            return default;
        }
        public override T Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagEmpty:
                    return new T();
                case TagObject:
                    return Read(reader);
                case TagMap:
                    return ReadMapAsObject(reader);
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}