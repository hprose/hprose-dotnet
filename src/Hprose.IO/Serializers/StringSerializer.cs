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
 * StringSerializer.cs                                    *
 *                                                        *
 * StringSerializer class for C#.                         *
 *                                                        *
 * LastModified: Apr 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using static Hprose.IO.Tags;

namespace Hprose.IO.Serializers {
    class StringSerializer : ReferenceSerializer<string> {
        public override void Write(Writer writer, string obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagString);
            ValueWriter.Write(stream, obj);
        }
        public override void Serialize(Writer writer, string obj) {
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