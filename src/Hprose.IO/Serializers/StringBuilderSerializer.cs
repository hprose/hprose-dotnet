/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  StringBuilderSerializer.cs                              |
|                                                          |
|  StringBuilderSerializer class for C#.                   |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Text;

namespace Hprose.IO.Serializers {
    using static Tags;

    class StringBuilderSerializer : ReferenceSerializer<StringBuilder> {
        public override void Write(Writer writer, StringBuilder obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagString);
            ValueWriter.Write(stream, obj.ToString());
        }
        public override void Serialize(Writer writer, StringBuilder obj) {
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