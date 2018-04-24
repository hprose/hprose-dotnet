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
 * LastModified: Apr 24, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

using Hprose.IO.Accessors;

using static Hprose.IO.HproseMode;
using static Hprose.IO.HproseTags;

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
        public static Action<Writer, T> GetDelegate<T>(IEnumerable<MemberInfo> members) {
            var writer = Expression.Variable(typeof(Writer), "writer");
            var obj = Expression.Variable(typeof(T), "obj");
            List<Expression> expressions = new List<Expression>();
            foreach (MemberInfo member in members) {
                Type memberType = member is FieldInfo ? ((FieldInfo)member).FieldType : ((PropertyInfo)member).PropertyType;
                var elemSerializerType = typeof(Serializer<>).MakeGenericType(memberType);
                expressions.Add(
                    Expression.Call(
                        Expression.Property(null, elemSerializerType, "Instance"),
                        elemSerializerType.GetMethod("Serialize"),
                        writer,
                        Expression.PropertyOrField(obj, member.Name)
                    )
                );
            }
            return Expression.Lambda<Action<Writer, T>>(Expression.Block(expressions), writer, obj).Compile();
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
            WriteMembers = GetDelegate<T>(members.Values);
        }
    }

    class FieldsWriter<T> : MembersWriter {
        public static readonly MembersWriter Instance = new FieldsWriter<T>();
        private FieldsWriter() {
            var fields = Accessor.GetFields<T>();
            Count = fields.Count;
            MetaData = GetMetaData(ClassManager.GetName<T>(), fields.Keys, Count);
            WriteMembers = GetDelegate<T>(fields.Values);
        }
    }

    class PropertiesWriter<T> : MembersWriter {
        public static readonly MembersWriter Instance = new PropertiesWriter<T>();
        private PropertiesWriter() {
            var properties = Accessor.GetProperties<T>();
            Count = properties.Count;
            MetaData = GetMetaData(ClassManager.GetName<T>(), properties.Keys, Count);
            WriteMembers = GetDelegate<T>(properties.Values);
        }
    }

    class ObjectSerializer<T> : ReferenceSerializer<T> {
        public override void Write(Writer writer, T obj) {
            MembersWriter membersWriter = MembersWriter.GetMembersWriter<T>(writer.Mode);
            int count = membersWriter.Count;
            var stream = writer.Stream;
            var type = typeof(T);
            int r = writer.WriteClass(type, () => {
                byte[] data = membersWriter.MetaData;
                stream.Write(data, 0, data.Length);
                writer.AddCount(count);
            });
            base.Write(writer, obj);
            stream.WriteByte(TagObject);
            ValueWriter.WriteInt(stream, r);
            stream.WriteByte(TagOpenbrace);
            if (count > 0) {
                ((Action<Writer, T>)(membersWriter.WriteMembers))(writer, obj);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
}
