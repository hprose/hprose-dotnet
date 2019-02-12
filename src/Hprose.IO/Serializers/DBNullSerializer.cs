/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  DBNullSerializer.cs                                     |
|                                                          |
|  DBNullSerializer class for C#.                          |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Serializers {
    internal class DBNullSerializer : Serializer<DBNull> {
        public override void Write(Writer writer, DBNull obj) => writer.Stream.WriteByte(Tags.TagNull);
    }
}