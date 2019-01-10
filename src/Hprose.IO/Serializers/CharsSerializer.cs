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
 * CharsSerializer.cs                                     *
 *                                                        *
 * CharsSerializer class for C#.                          *
 *                                                        *
 * LastModified: Jan 11, 2019                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    using static Tags;

    class CharsSerializer : ReferenceSerializer<char[]> {
        public override void Write(Writer writer, char[] obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagString);
            ValueWriter.Write(stream, obj);
        }
        public override void Serialize(Writer writer, char[] obj) {
            var stream = writer.Stream;
            switch (obj?.Length) {
                case 0:
                    stream.WriteByte(TagEmpty);
                    break;
                case 1:
                    ValueWriter.Write(stream, obj[0]);
                    break;
                default:
                    base.Serialize(writer, obj);
                    break;
            }
        }
    }
}
