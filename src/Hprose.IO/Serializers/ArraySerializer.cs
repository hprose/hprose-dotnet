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
 * ArraySerializer.cs                                     *
 *                                                        *
 * ArraySerializer class for C#.                          *
 *                                                        *
 * LastModified: Apr 2, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Serializers {
    class ArraySerializer<T> : ReferenceSerializer<T[]> {
        public override void Serialize(Writer writer, T[] obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            int length = obj.Length;
            stream.WriteByte(TagList);
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagOpenbrace);
            var serializer = Serializer<T>.Instance;
            for (int i = 0; i < length; ++i) {
                serializer.Write(writer, obj[i]);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
    class Array2Serializer<T> : ReferenceSerializer<T[,]> {
        public override void Serialize(Writer writer, T[,] obj) {
            base.Serialize(writer, obj);
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
                writer.SetRef(new object());
                stream.WriteByte(TagList);
                if (length2 > 0) {
                    ValueWriter.WriteInt(stream, length2);
                }
                stream.WriteByte(TagOpenbrace);
                for (int j = 0; j < length2; ++j) {
                    serializer.Write(writer, obj[i, j]);
                }
                stream.WriteByte(TagClosebrace);
            }
            stream.WriteByte(TagClosebrace);
        }
    }

}
