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
 * EnumerableSerializer.cs                                *
 *                                                        *
 * EnumerableSerializer class for C#.                     *
 *                                                        *
 * LastModified: Apr 1, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.Collections;
using System.Collections.Generic;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Serializers {
    class EnumerableSerializer<T, V> : ReferenceSerializer<T> where T : IEnumerable<V>, ICollection {
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
    class EnumerableSerializer<T> : ReferenceSerializer<T> where T : ICollection, IEnumerable {
        public override void Serialize(Writer writer, T obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            int length = obj.Count;
            stream.WriteByte(TagList);
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagOpenbrace);
            var serializer = Serializer.Instance;
            foreach (object value in obj) {
                serializer.Write(writer, value);
            }
            stream.WriteByte(TagClosebrace);
        }
    }

}
