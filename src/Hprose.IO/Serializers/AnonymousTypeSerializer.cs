/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  AnonymousTypeSerializer.cs                              |
|                                                          |
|  AnonymousTypeSerializer class for C#.                   |
|                                                          |
|  LastModified: Feb 18, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/
#if !NET35
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Hprose.IO.Serializers {
    using static Tags;

    internal class AnonymousTypeSerializer<T> : ReferenceSerializer<T> {
        private static readonly Action<Writer, T> write;
        private static readonly int length;
        static AnonymousTypeSerializer() {
            var properties = typeof(T).GetProperties();
            length = properties.Length;
            var writer = Expression.Variable(typeof(Writer), "writer");
            var obj = Expression.Variable(typeof(T), "obj");
            Type strSerializerType = typeof(Serializer<string>);
            var strSerializer = Expression.Property(null, strSerializerType, "Instance");
            List<Expression> expressions = new List<Expression>();
            foreach (PropertyInfo property in properties) {
                expressions.Add(
                    Expression.Call(
                        strSerializer,
                        strSerializerType.GetMethod("Serialize"),
                        writer,
                        Expression.Constant(Accessor.UnifiedName(property.Name))
                    )
                );
                var elemSerializerType = typeof(Serializer<>).MakeGenericType(property.PropertyType);
                expressions.Add(
                    Expression.Call(
                        Expression.Property(null, elemSerializerType, "Instance"),
                        elemSerializerType.GetMethod("Serialize"),
                        writer,
                        Expression.Property(obj, property.Name)
                    )
                );
            }
            write = (Action<Writer, T>)Expression.Lambda(Expression.Block(expressions), writer, obj).Compile();
        }
        public override void Write(Writer writer, T obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagMap);
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagOpenbrace);
            if (length > 0) {
                write(writer, obj);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
}
#else
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Hprose.IO.Serializers {
    using static Tags;

    internal class AnonymousTypeSerializer<T> : ReferenceSerializer<T> {
        public override void Write(Writer writer, T obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            var properties = obj.GetType().GetProperties();
            int length = properties.Length;
            stream.WriteByte(TagMap);
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagOpenbrace);
            if (length > 0) {
                var stringSerializer = Serializer<String>.Instance;
                foreach (var property in properties) {
                    stringSerializer.Serialize(writer, property.Name);
                    writer.Serialize(property.GetValue(obj, null));
                }
            }
            stream.WriteByte(TagClosebrace);
        }
    }
}
#endif