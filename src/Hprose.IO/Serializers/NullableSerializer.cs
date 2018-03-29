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
 * NullableSerializer.cs                                  *
 *                                                        *
 * NullableSerializer class for C#.                       *
 *                                                        *
 * LastModified: Mar 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;
namespace Hprose.IO.Serializers {
    class NullableSerializer<T> : Serializer<T?> where T : struct {
        private static readonly NullableSerializer<T> _instance = new NullableSerializer<T>();
        private readonly Serializer serializer;
        private readonly Serializer<T> serializerT;
        public static NullableSerializer<T> Instance => _instance;
        public NullableSerializer() {
            serializer = SerializerFactory.Get(typeof(T));
            serializerT = serializer as Serializer<T>;
        }
        public override void Write(Writer writer, T? obj) {
            if (obj.HasValue) {
                if (serializerT != null) {
                    serializerT.Write(writer, obj.Value);
                }
                else {
                    serializer.Write(writer, obj.Value);
                }
            }
            else {
                writer.Stream.WriteByte(HproseTags.TagNull);
            }
        }
    }
}
