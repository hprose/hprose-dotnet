/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Int32Serializer.cs                                      |
|                                                          |
|  Int32Serializer class for C#.                           |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Serializers {
    class Int32Serializer : Serializer<int> {
        public override void Write(Writer writer, int obj) => ValueWriter.Write(writer.Stream, obj);
    }
}