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
 * LastModified: Mar 30, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Serializers {
    class StringSerializer : ReferenceSerializer<string> {
        private static readonly StringSerializer _instance = new StringSerializer();
        public static StringSerializer Instance => _instance;
        public override void Serialize(Writer writer, string obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagString);
            ValueWriter.Write(stream, obj);
        }
        public override void Write(Writer writer, string obj) {
            var stream = writer.Stream;
            switch (obj?.Length) {
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
