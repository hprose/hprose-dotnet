/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  NullableSerializer.cs                                   |
|                                                          |
|  NullableSerializer class for C#.                        |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Serializers {
    internal class NullableSerializer<T> : Serializer<T?> where T : struct {
        public override void Write(Writer writer, T? obj) {
            if (obj.HasValue) {
                Serializer<T>.Instance.Serialize(writer, obj.Value);
            }
            else {
                writer.Stream.WriteByte(Tags.TagNull);
            }
        }
    }
}