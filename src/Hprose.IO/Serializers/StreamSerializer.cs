/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  StreamSerializer.cs                                     |
|                                                          |
|  StreamSerializer class for C#.                          |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.IO;

namespace Hprose.IO.Serializers {
    using static Tags;

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