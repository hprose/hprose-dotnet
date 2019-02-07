/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  DoubleSerializer.cs                                     |
|                                                          |
|  DoubleSerializer class for C#.                          |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Serializers {
    internal class DoubleSerializer : Serializer<double> {
        public override void Write(Writer writer, double obj) => ValueWriter.Write(writer.Stream, obj);
    }
}