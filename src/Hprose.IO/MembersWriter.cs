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
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

namespace Hprose.IO {
    using static Mode;
    using static Tags;

    class MembersWriter {
        public Delegate write;
        public int count;
        public ArraySegment<byte> data;
        public static ArraySegment<byte> GetMetaData(string typeName, IEnumerable<string> memberNames, int count) {
            using (var stream = new MemoryStream()) {
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
                return stream.GetArraySegment();
            }
        }
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

    class MembersWriter<T> : MembersWriter {
        public static readonly MembersWriter Instance = new MembersWriter<T>();
        private MembersWriter() {
            var members = MembersAccessor<T>.Members;
            count = members.Count;
            data = GetMetaData(TypeManager.GetName<T>(), members.Keys, count);
            write = CreateWriteAction<T>(members.Values);
        }
    }

    class FieldsWriter<T> : MembersWriter {
        public static readonly MembersWriter Instance = new FieldsWriter<T>();
        private FieldsWriter() {
            var fields = FieldsAccessor<T>.Fields;
            count = fields.Count;
            data = GetMetaData(TypeManager.GetName<T>(), fields.Keys, count);
            write = CreateWriteAction<T>(fields.Values);
        }
    }

    class PropertiesWriter<T> : MembersWriter {
        public static readonly MembersWriter Instance = new PropertiesWriter<T>();
        private PropertiesWriter() {
            var properties = PropertiesAccessor<T>.Properties;
            count = properties.Count;
            data = GetMetaData(TypeManager.GetName<T>(), properties.Keys, count);
            write = CreateWriteAction<T>(properties.Values);
        }
    }
}
