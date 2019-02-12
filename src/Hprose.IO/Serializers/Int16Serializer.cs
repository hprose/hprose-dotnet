/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Int16Serializer.cs                                      |
|                                                          |
|  Int16Serializer class for C#.                           |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Serializers {
    internal class Int16Serializer : Serializer<short> {
        public override void Write(Writer writer, short obj) => ValueWriter.Write(writer.Stream, obj);
    }
}