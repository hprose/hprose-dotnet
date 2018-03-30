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
 * ListSerializer.cs                                      *
 *                                                        *
 * ListSerializer class for C#.                           *
 *                                                        *
 * LastModified: Mar 30, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.Collections.Generic;
using static Hprose.IO.HproseTags;

namespace Hprose.IO.Serializers {
    class ListSerializer<T, V> : ReferenceSerializer<T> where T : IList<V> {
        public override void Serialize(Writer writer, T obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            int length = obj.Count;
            stream.WriteByte(TagList);
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagOpenbrace);
            var serializer = Serializer<V>.Instance;
            foreach (V value in obj) {
                serializer.Write(writer, value);
            }
            stream.WriteByte(TagClosebrace);
        }
    }

}
