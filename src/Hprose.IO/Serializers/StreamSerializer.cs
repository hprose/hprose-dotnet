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

    internal class StreamSerializer<T> : ReferenceSerializer<T> where T : Stream {
        public override void Write(Writer writer, T obj) {
            if (!obj.CanRead || !obj.CanSeek) {
                throw new IOException("This stream can't support serialize.");
            }
            base.Write(writer, obj);
            var stream = writer.Stream;
            long oldPos = obj.Position;
            obj.Position = 0;
            stream.WriteByte(TagBytes);
            int length = (int)obj.Length;
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagQuote);
#if !NET35
            obj.CopyTo(stream);
#else
        	byte[] array = new byte[length > 81920 ? 81920 : length];
            int count;
	        while ((count = obj.Read(array, 0, array.Length)) != 0) {
		        stream.Write(array, 0, count);
	        }
#endif
            stream.WriteByte(TagQuote);
            obj.Position = oldPos;
        }
    }
}