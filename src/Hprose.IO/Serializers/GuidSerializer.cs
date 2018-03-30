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
 * GuidSerializer.cs                                      *
 *                                                        *
 * GuidSerializer class for C#.                           *
 *                                                        *
 * LastModified: Mar 30, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;
using System.IO;
using static Hprose.IO.HproseTags;

namespace Hprose.IO.Serializers {
    class GuidSerializer : ReferenceSerializer<Guid> {
        public override void Serialize(Writer writer, Guid obj) {
            base.Serialize(writer, obj);
            Stream stream = writer.Stream;
            stream.WriteByte(TagGuid);
            stream.WriteByte(TagOpenbrace);
            byte[] buf = ValueWriter.GetASCII(obj.ToString());
            stream.Write(buf, 0, buf.Length);
            stream.WriteByte(TagClosebrace);
        }
    }
}