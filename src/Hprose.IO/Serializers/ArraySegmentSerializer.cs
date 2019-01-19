/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ArraySerializer.cs                                      |
|                                                          |
|  ArraySerializer class for C#.                           |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Serializers {
    using System;
    using static Tags;

    class ArraySegmentSerializer<T> : ReferenceSerializer<ArraySegment<T>> {
        public override void Write(Writer writer, ArraySegment<T> obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            int length = obj.Count;
            stream.WriteByte(TagList);
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagOpenbrace);
            var serializer = Serializer<T>.Instance;
            for (int i = obj.Offset, n = obj.Offset + length; i < n; ++i) {
                serializer.Serialize(writer, obj.Array[i]);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
}
