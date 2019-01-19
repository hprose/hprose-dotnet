/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Int64Serializer.cs                                      |
|                                                          |
|  Int64Serializer class for C#.                           |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Serializers {
    class Int64Serializer : Serializer<long> {
        public override void Write(Writer writer, long obj) => ValueWriter.Write(writer.Stream, obj);
    }
}