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
    class MembersReader {
        public static Delegate GetDelegate(Type type, Dictionary<string, MemberInfo> members, string[] names) {
            var reader = Expression.Variable(typeof(Reader), "reader");
            var obj = Expression.Variable(type, "obj");
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
            return Expression.Lambda(Expression.Block(expressions), reader, obj).Compile();
        }
        public static Action<Reader, T> GetReadAction<T>(HproseMode mode, string[] names) {
            if (typeof(T).IsSerializable) {
                switch (mode) {
                    case FieldMode: return FieldsReader<T>.GetReadAction(names) as Action<Reader, T>;
                    case PropertyMode: return PropertiesReader<T>.GetReadAction(names) as Action<Reader, T>;
                }
            }
            return MembersReader<T>.GetReadAction(names) as Action<Reader, T>;
        }
    }

    class MembersReader<T> {
        private static readonly ConcurrentDictionary<string, Lazy<Delegate>> _readActions = new ConcurrentDictionary<string, Lazy<Delegate>>();
        public static Delegate GetReadAction(string[] names) => _readActions.GetOrAdd(string.Join(" ", names), (_) => new Lazy<Delegate>(() => MembersReader.GetDelegate(typeof(T), MembersAccessor<T>.Members, names))).Value;
    }

    class FieldsReader<T> {
        private static readonly ConcurrentDictionary<string, Lazy<Delegate>> _readActions = new ConcurrentDictionary<string, Lazy<Delegate>>();
        public static Delegate GetReadAction(string[] names) => _readActions.GetOrAdd(string.Join(" ", names), (_) => new Lazy<Delegate>(() => MembersReader.GetDelegate(typeof(T), FieldsAccessor<T>.Fields, names))).Value;
    }

    class PropertiesReader<T> {
        private static readonly ConcurrentDictionary<string, Lazy<Delegate>> _readActions = new ConcurrentDictionary<string, Lazy<Delegate>>();
        public static Delegate GetReadAction(string[] names) => _readActions.GetOrAdd(string.Join(" ", names), (_) => new Lazy<Delegate>(() => MembersReader.GetDelegate(typeof(T), PropertiesAccessor<T>.Properties, names))).Value;
    }

    class ObjectDeserializer<T> : Deserializer<T> where T : new() {
        public static T Read(Reader reader) {
            Stream stream = reader.Stream;
            T obj = new T();
            reader.SetRef(obj);
            var deserializer = Deserializer.Instance;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            MembersReader.GetReadAction<T>(reader.Mode, reader[index].Members)(reader, obj);
            stream.ReadByte();
            return obj;
        }
        public static T ReadMapAsObject(Reader reader) {
            Stream stream = reader.Stream;
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