/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  GuidSerializer.cs                                       |
|                                                          |
|  GuidSerializer class for C#.                            |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;

namespace Hprose.IO.Serializers {
    using static Tags;

    internal class GuidSerializer : ReferenceSerializer<Guid> {
        public override void Write(Writer writer, Guid obj) {
            base.Write(writer, obj);
            Stream stream = writer.Stream;
            stream.WriteByte(TagGuid);
            stream.WriteByte(TagOpenbrace);
            ValueWriter.WriteASCII(stream, obj.ToString());
            stream.WriteByte(TagClosebrace);
        }
    }
}