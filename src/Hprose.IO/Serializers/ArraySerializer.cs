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
    using static Tags;

    internal class ArraySerializer<T> : ReferenceSerializer<T[]> {
        public override void Write(Writer writer, T[] obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            int length = obj.Length;
            stream.WriteByte(TagList);
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagOpenbrace);
            var serializer = Serializer<T>.Instance;
            for (int i = 0; i < length; ++i) {
                serializer.Serialize(writer, obj[i]);
            }
            stream.WriteByte(TagClosebrace);
        }
    }

    internal class Array2Serializer<T> : ReferenceSerializer<T[,]> {
        public override void Write(Writer writer, T[,] obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            int length = obj.GetLength(0);
            int length2 = obj.GetLength(1);
            stream.WriteByte(TagList);
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagOpenbrace);
            var serializer = Serializer<T>.Instance;
            for (int i = 0; i < length; ++i) {
                writer.SetReference(new object());
                stream.WriteByte(TagList);
                if (length2 > 0) {
                    ValueWriter.WriteInt(stream, length2);
                }
                stream.WriteByte(TagOpenbrace);
                for (int j = 0; j < length2; ++j) {
                    serializer.Serialize(writer, obj[i, j]);
                }
                stream.WriteByte(TagClosebrace);
            }
            stream.WriteByte(TagClosebrace);
        }
    }

}
