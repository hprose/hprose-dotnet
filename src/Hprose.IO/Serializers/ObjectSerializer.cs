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
 * ObjectSerializer.cs                                    *
 *                                                        *
 * ObjectSerializer class for C#.                         *
 *                                                        *
 * LastModified: Apr 3, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.IO;
using Hprose.IO.Accessors;

using static Hprose.IO.HproseTags;
using static Hprose.IO.HproseMode;

namespace Hprose.IO.Serializers {

    class MembersWriter {
        public Delegate WriteMembers;
        public int Count;
        public byte[] MetaData;
        public static byte[] GetMetaData(string typeName, IEnumerable<string> memberNames, int count) {
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
                return stream.ToArray();
            }
        }
        public static Delegate GetDelegate(Type type, IEnumerable<MemberInfo> members) {
            var writer = Expression.Variable(typeof(Writer), "writer");
            var obj = Expression.Variable(type, "obj");
            List<Expression> expressions = new List<Expression>();
            foreach (MemberInfo member in members) {
                Type memberType = member is FieldInfo ? ((FieldInfo)member).FieldType : ((PropertyInfo)member).PropertyType;
                var elemSerializerType = typeof(Serializer<>).MakeGenericType(memberType);
                expressions.Add(
                    Expression.Call(
                        Expression.Property(null, elemSerializerType, "Instance"),
                        elemSerializerType.GetMethod("Write"),
                        writer,
                        Expression.PropertyOrField(obj, member.Name)
                    )
                );
            }
            return Expression.Lambda(Expression.Block(expressions), writer, obj).Compile();
        }
        public static MembersWriter GetMembersWriter<T>(HproseMode mode) {
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
            var members = Accessor.GetMembers<T>();
            Count = members.Count;
            MetaData = GetMetaData(ClassManager.GetName<T>(), members.Keys, Count);
            WriteMembers = GetDelegate(typeof(T), members.Values);
        }
    }

    class FieldsWriter<T> : MembersWriter {
        public static readonly MembersWriter Instance = new FieldsWriter<T>();
        private FieldsWriter() {
            var fields = Accessor.GetFields<T>();
            Count = fields.Count;
            MetaData = GetMetaData(ClassManager.GetName<T>(), fields.Keys, Count);
            WriteMembers = GetDelegate(typeof(T), fields.Values);
        }
    }

    class PropertiesWriter<T> : MembersWriter {
        public static readonly MembersWriter Instance = new PropertiesWriter<T>();
        private PropertiesWriter() {
            var properties = Accessor.GetProperties<T>();
            Count = properties.Count;
            MetaData = GetMetaData(ClassManager.GetName<T>(), properties.Keys, Count);
            WriteMembers = GetDelegate(typeof(T), properties.Values);
        }
    }

    class ObjectSerializer<T> : ReferenceSerializer<T> {
        public override void Serialize(Writer writer, T obj) {
            MembersWriter membersWriter = MembersWriter.GetMembersWriter<T>(writer.Mode);
            int count = membersWriter.Count;
            var stream = writer.Stream;
            var type = typeof(T);
            int r = writer.WriteMetaData(type, () => {
                byte[] data = membersWriter.MetaData;
                stream.Write(data, 0, data.Length);
                writer.AddCount(count);
            });
            base.Serialize(writer, obj);
            stream.WriteByte(TagObject);
            ValueWriter.WriteInt(stream, r);
            stream.WriteByte(TagOpenbrace);
            if (count > 0) {
                ((Action<Writer, T>)membersWriter.WriteMembers)(writer, obj);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
}
