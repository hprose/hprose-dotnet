/**********************************************************\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: http://www.hprose.com/                 |
|                   http://www.hprose.org/                 |
|                                                          |
\**********************************************************/
/**********************************************************\
 *                                                        *
 * ObjectSerializer.cs                                    *
 *                                                        *
 * ObjectSerializer class for C#.                         *
 *                                                        *
 * LastModified: Apr 28, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;

using static Hprose.IO.Tags;

namespace Hprose.IO.Serializers {
    class ObjectSerializer<T> : ReferenceSerializer<T> {
        public override void Write(Writer writer, T obj) {
            MembersWriter membersWriter = MembersWriter.GetMembersWriter<T>(writer.Mode);
            int count = membersWriter.count;
            var stream = writer.Stream;
            var type = typeof(T);
            int r = writer.WriteClass(type, () => {
                byte[] data = membersWriter.data;
                stream.Write(data, 0, data.Length);
                writer.AddCount(count);
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
