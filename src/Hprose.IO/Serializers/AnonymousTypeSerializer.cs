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
 * AnonymousTypeSerializer.cs                             *
 *                                                        *
 * AnonymousTypeSerializer class for C#.                  *
 *                                                        *
 * LastModified: Apr 2, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.Reflection;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Serializers {
    class AnonymousTypeSerializer<T> : ReferenceSerializer<T> {
        static readonly PropertyInfo[] Properties;
        static readonly int Length;
        static AnonymousTypeSerializer() {
            Properties = typeof(T).GetProperties();
            Length = Properties.Length;
        }
        public override void Serialize(Writer writer, T obj) {
            base.Serialize(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagMap);
            if (Length > 0) ValueWriter.WriteInt(stream, Length);
            stream.WriteByte(TagOpenbrace);
            var strSerializer = Serializer<string>.Instance;
            var objSerializer = Serializer.Instance;
            foreach (PropertyInfo property in Properties) {
                strSerializer.Write(writer, property.Name);
                objSerializer.Write(writer, property.GetValue(obj, null));
            }
            stream.WriteByte(TagClosebrace);
        }
    }
}
