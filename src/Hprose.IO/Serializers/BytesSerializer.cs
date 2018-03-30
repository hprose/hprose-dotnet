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
 * BytesSerializer.cs                                     *
 *                                                        *
 * BytesSerializer class for C#.                          *
 *                                                        *
 * LastModified: Mar 30, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Serializers {
    class BytesSerializer : ReferenceSerializer<byte[]> {
        public override void Serialize(Writer writer, byte[] obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagBytes);
            int length = obj.Length;
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagQuote);
            stream.Write(obj, 0, length);
            stream.WriteByte(TagQuote);
        }
    }
}
