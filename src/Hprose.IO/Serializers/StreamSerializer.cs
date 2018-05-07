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
 * LastModified: Apr 26, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.IO;

using static Hprose.IO.Tags;

namespace Hprose.IO.Serializers {
    class StreamSerializer<T> : ReferenceSerializer<T> where T : Stream {
        public override void Write(Writer writer, T obj) {
            if (!obj.CanRead) {
                throw new IOException("This stream can't support serialize.");
            }
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
            stream.WriteByte(TagQuote);
            obj.CopyTo(stream);
            stream.WriteByte(TagQuote);
            if (obj.CanSeek) {
                obj.Position = oldPos;
            }
        }
    }
}