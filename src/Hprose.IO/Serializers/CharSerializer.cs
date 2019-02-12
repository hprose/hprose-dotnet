/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  CharSerializer.cs                                       |
|                                                          |
|  CharSerializer class for C#.                            |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Serializers {
    internal class CharSerializer : Serializer<char> {
        public override void Write(Writer writer, char obj) => ValueWriter.Write(writer.Stream, obj);
    }
}