/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  SByteSerializer.cs                                      |
|                                                          |
|  SByteSerializer class for C#.                           |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Serializers {
    internal class SByteSerializer : Serializer<sbyte> {
        public override void Write(Writer writer, sbyte obj) => ValueWriter.Write(writer.Stream, obj);
    }
}