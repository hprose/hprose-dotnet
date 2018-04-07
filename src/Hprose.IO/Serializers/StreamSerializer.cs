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
 * StreamSerializer.cs                                    *
 *                                                        *
 * StreamSerializer class for C#.                         *
 *                                                        *
 * LastModified: Apr 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.IO;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Serializers {
    class StreamSerializer<T> : ReferenceSerializer<T> where T : Stream {
        public override void Write(Writer writer, T obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            long oldPos = 0;
            if (obj.CanSeek) {
                oldPos = obj.Position;
                obj.Position = 0;
            }
            stream.WriteByte(TagBytes);
            int length = (int)obj.Length;
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(HproseTags.TagQuote);
            byte[] buffer = new byte[4096];
            while ((length = obj.Read(buffer, 0, 4096)) != 0) {
                stream.Write(buffer, 0, length);
            }
            stream.WriteByte(HproseTags.TagQuote);
            if (obj.CanSeek) {
                obj.Position = oldPos;
            }
        }
        public override void Serialize(Writer writer, T obj) {
            if (!obj.CanRead) {
                throw new IOException("This stream can't support serialize.");
            }
            base.Serialize(writer, obj);
        }
    }
}