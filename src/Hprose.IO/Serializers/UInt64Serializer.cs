/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  UInt64Serializer.cs                                     |
|                                                          |
|  UInt64Serializer class for C#.                          |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Serializers {
    internal class UInt64Serializer : Serializer<ulong> {
        public override void Write(Writer writer, ulong obj) => ValueWriter.Write(writer.Stream, obj);
    }
}