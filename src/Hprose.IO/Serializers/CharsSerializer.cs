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
 * LastModified: Mar 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Serializers {
    class CharsSerializer : ReferenceSerializer<char[]> {
        private static readonly CharsSerializer _instance = new CharsSerializer();
        public static CharsSerializer Instance => _instance;
        public override void Serialize(Writer writer, char[] obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagString);
            ValueWriter.Write(stream, obj);
        }
        public override void Write(Writer writer, char[] obj) {
            var stream = writer.Stream;
            switch (obj.Length) {
                case 0:
                    stream.WriteByte(TagEmpty);
                    break;
                case 1:
                    ValueWriter.Write(stream, obj[0]);
                    break;
                default:
                    base.Write(writer, obj);
                    break;
            }
        }
    }
}
