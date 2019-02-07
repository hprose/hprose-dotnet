/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  DecimalSerializer.cs                                    |
|                                                          |
|  DecimalSerializer class for C#.                         |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Serializers {
    internal class DecimalSerializer : Serializer<decimal> {
        public override void Write(Writer writer, decimal obj) => ValueWriter.Write(writer.Stream, obj);
    }
}