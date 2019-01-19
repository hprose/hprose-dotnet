/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  BooleanSerializer.cs                                    |
|                                                          |
|  BooleanSerializer class for C#.                         |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Serializers {
    class BooleanSerializer : Serializer<bool> {
        public override void Write(Writer writer, bool obj) => ValueWriter.Write(writer.Stream, obj);
    }
}
