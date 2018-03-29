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
 * NullableKeySerializer.cs                               *
 *                                                        *
 * NullableKeySerializer class for C#.                    *
 *                                                        *
 * LastModified: Mar 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using Hprose.Collections.Generic;

namespace Hprose.IO.Serializers {
    class NullableKeySerializer<T> : Serializer<NullableKey<T>> {
        private static readonly NullableKeySerializer<T> _instance = new NullableKeySerializer<T>();
        private readonly Serializer serializer;
        private readonly Serializer<T> serializerT;
        public static NullableKeySerializer<T> Instance => _instance;
        public NullableKeySerializer() {
            serializer = SerializerFactory.Get(typeof(T));
            serializerT = serializer as Serializer<T>;
        }
        public override void Write(Writer writer, NullableKey<T> obj) {
            T value = obj.Value;
            if (value != null) {
                if (serializerT != null) {
                    serializerT.Write(writer, value);
                }
                else {
                    serializer.Write(writer, value);
                }
            }
            else {
                writer.Stream.WriteByte(HproseTags.TagNull);
            }
        }
    }
}
