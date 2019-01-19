/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  StringSerializer.cs                                     |
|                                                          |
|  StringSerializer class for C#.                          |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Serializers {
    using static Tags;

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