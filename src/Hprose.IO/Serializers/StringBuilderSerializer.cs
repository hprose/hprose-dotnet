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
 * StringBuilderSerializer.cs                             *
 *                                                        *
 * StringBuilderSerializer class for C#.                  *
 *                                                        *
 * LastModified: Mar 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System.Text;
using static Hprose.IO.HproseTags;

namespace Hprose.IO.Serializers {
    class StringBuilderSerializer : ReferenceSerializer<StringBuilder> {
        private static readonly StringBuilderSerializer _instance = new StringBuilderSerializer();
        public static StringBuilderSerializer Instance => _instance;
        public override void Serialize(Writer writer, StringBuilder obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagString);
            ValueWriter.Write(stream, obj.ToString());
        }
        public override void Write(Writer writer, StringBuilder obj) {
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
