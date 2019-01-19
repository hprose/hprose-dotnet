/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ByteSerializer.cs                                       |
|                                                          |
|  ByteSerializer class for C#.                            |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Serializers {
    class ByteSerializer : Serializer<byte> {
        public override void Write(Writer writer, byte obj) => ValueWriter.Write(writer.Stream, obj);
    }
}
