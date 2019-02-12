/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  UInt32Serializer.cs                                     |
|                                                          |
|  UInt32Serializer class for C#.                          |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Serializers {
    internal class UInt32Serializer : Serializer<uint> {
        public override void Write(Writer writer, uint obj) => ValueWriter.Write(writer.Stream, obj);
    }
}