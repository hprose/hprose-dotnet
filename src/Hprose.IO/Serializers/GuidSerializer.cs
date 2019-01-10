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
 * LastModified: Jan 11, 2019                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.IO;

namespace Hprose.IO.Serializers {
    using static Tags;

    class GuidSerializer : ReferenceSerializer<Guid> {
        public override void Write(Writer writer, Guid obj) {
            base.Write(writer, obj);
            Stream stream = writer.Stream;
            stream.WriteByte(TagGuid);
            stream.WriteByte(TagOpenbrace);
            byte[] buf = ValueWriter.GetASCII(obj.ToString());
            stream.Write(buf, 0, buf.Length);
            stream.WriteByte(TagClosebrace);
        }
    }
}