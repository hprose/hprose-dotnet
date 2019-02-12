/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ObjectSerializer.cs                                     |
|                                                          |
|  ObjectSerializer class for C#.                          |
|                                                          |
|  LastModified: Feb 7, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Serializers {
    using static Tags;

    internal class ObjectSerializer<T> : ReferenceSerializer<T> {
        public override void Write(Writer writer, T obj) {
            MembersWriter membersWriter = MembersWriter.GetMembersWriter<T>(writer.Mode);
            int count = membersWriter.count;
            var stream = writer.Stream;
            var type = typeof(T);
            int r = writer.WriteClass(type, () => {
                var data = membersWriter.data;
                stream.Write(data, 0, data.Length);
                writer.AddReferenceCount(count);
            });
            base.Write(writer, obj);
            stream.WriteByte(TagObject);
            ValueWriter.WriteInt(stream, r);
            stream.WriteByte(TagOpenbrace);
            if (count > 0) {
                ((Action<Writer, T>)(membersWriter.write))(writer, obj);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
}
