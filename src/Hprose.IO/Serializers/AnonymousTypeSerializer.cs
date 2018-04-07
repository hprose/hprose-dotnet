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
 * AnonymousTypeSerializer.cs                             *
 *                                                        *
 * AnonymousTypeSerializer class for C#.                  *
 *                                                        *
 * LastModified: Apr 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Serializers {
    class AnonymousTypeSerializer<T> : ReferenceSerializer<T> {
        static readonly Action<Writer, T> WriteProperties;
        static readonly int Length;
        static AnonymousTypeSerializer() {
            var properties = typeof(T).GetProperties();
            Length = properties.Length;
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
                        Expression.Constant(property.Name)
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
            WriteProperties = (Action<Writer, T>)Expression.Lambda(Expression.Block(expressions), writer, obj).Compile();
        }
        public override void Write(Writer writer, T obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagMap);
            if (Length > 0) {
                ValueWriter.WriteInt(stream, Length);
            }
            stream.WriteByte(TagOpenbrace);
            if (Length > 0) {
                WriteProperties(writer, obj);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
}
