/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  SingleSerializer.cs                                     |
|                                                          |
|  SingleSerializer class for C#.                          |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Serializers {
    class SingleSerializer : Serializer<float> {
        public override void Write(Writer writer, float obj) => ValueWriter.Write(writer.Stream, obj);
    }
}