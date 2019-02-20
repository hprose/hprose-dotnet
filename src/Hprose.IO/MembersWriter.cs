/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ObjectSerializer.cs                                     |
|                                                          |
|  ObjectSerializer class for C#.                          |
|                                                          |
|  LastModified: Feb 20, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

namespace Hprose.IO {
    using System.Threading;
    using static Mode;
    using static Tags;

    internal class MembersWriter {
        private static readonly ThreadLocal<MemoryStream> memoryStream = new ThreadLocal<MemoryStream>(() => new MemoryStream());
        public Delegate write;
        public int count;
        public byte[] data;
        public static byte[] GetMetaData(string typeName, IEnumerable<string> memberNames, int count) {
            var stream = memoryStream.Value;
            stream.SetLength(0);
            stream.WriteByte(TagClass);
            ValueWriter.Write(stream, typeName);
            if (count > 0) {
                ValueWriter.WriteInt(stream, count);
            }
            stream.WriteByte(TagOpenbrace);
            foreach (string name in memberNames) {
                stream.WriteByte(TagString);
                ValueWriter.Write(stream, name);
            }
            stream.WriteByte(TagClosebrace);
            return stream.ToArray();
        }
#if !NET35
        public static Action<Writer, T> CreateWriteAction<T>(IEnumerable<MemberInfo> members) {
            var writer = Expression.Variable(typeof(Writer), "writer");
            var obj = Expression.Variable(typeof(T), "obj");
            List<Expression> expressions = new List<Expression>();
            foreach (MemberInfo member in members) {
                Type memberType = Accessor.GetMemberType(member);
                var elemSerializerType = typeof(Serializer<>).MakeGenericType(memberType);
                var serializer = elemSerializerType.GetProperty("Instance").GetValue(null, null);
                expressions.Add(
                    Expression.Call(
                        Expression.Constant(serializer),
                        elemSerializerType.GetMethod("Serialize"),
                        writer,
                        member is FieldInfo ?
                        Expression.Field(obj, (FieldInfo)member) :
                        Expression.Property(obj, (PropertyInfo)member)
                    )
                );
            }
            return Expression.Lambda<Action<Writer, T>>(Expression.Block(expressions), writer, obj).Compile();
        }
#else
        public static Action<Writer, T> CreateWriteAction<T>(IEnumerable<MemberInfo> members) {
            var actions = new List<Action<Writer, T>>();
            foreach (var member in members) {
                Type memberType = Accessor.GetMemberType(member);
                var serializer = Serializer.GetInstance(memberType);
                if (member is FieldInfo) {
                    actions.Add((writer, obj) => serializer.Serialize(writer, ((FieldInfo)member).GetValue(obj)));
                }
                else {
                    actions.Add((writer, obj) => serializer.Serialize(writer, ((PropertyInfo)member).GetValue(obj, null)));
                }
            }
            return (writer, obj) => {
                foreach (var action in actions) action(writer, obj);
            };
        }
#endif
        public static MembersWriter GetMembersWriter<T>(Mode mode) {
            if (typeof(T).IsSerializable) {
                switch (mode) {
                    case FieldMode: return FieldsWriter<T>.Instance;
                    case PropertyMode: return PropertiesWriter<T>.Instance;
                }
            }
            return MembersWriter<T>.Instance;
        }
    }

    internal class MembersWriter<T> : MembersWriter {
        public static readonly MembersWriter Instance = new MembersWriter<T>();
        private MembersWriter() {
            var members = MembersAccessor<T>.Members;
            count = members.Count;
            data = GetMetaData(TypeManager.GetName<T>(), members.Keys, count);
            write = CreateWriteAction<T>(members.Values);
        }
    }

    internal class FieldsWriter<T> : MembersWriter {
        public static readonly MembersWriter Instance = new FieldsWriter<T>();
        private FieldsWriter() {
            var fields = FieldsAccessor<T>.Fields;
            count = fields.Count;
            data = GetMetaData(TypeManager.GetName<T>(), fields.Keys, count);
            write = CreateWriteAction<T>(fields.Values);
        }
    }

    internal class PropertiesWriter<T> : MembersWriter {
        public static readonly MembersWriter Instance = new PropertiesWriter<T>();
        private PropertiesWriter() {
            var properties = PropertiesAccessor<T>.Properties;
            count = properties.Count;
            data = GetMetaData(TypeManager.GetName<T>(), properties.Keys, count);
            write = CreateWriteAction<T>(properties.Values);
        }
    }
}