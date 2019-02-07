/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  UInt16Serializer.cs                                     |
|                                                          |
|  UInt16Serializer class for C#.                          |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Serializers {
    internal class UInt16Serializer : Serializer<ushort> {
        public override void Write(Writer writer, ushort obj) => ValueWriter.Write(writer.Stream, obj);
    }
}