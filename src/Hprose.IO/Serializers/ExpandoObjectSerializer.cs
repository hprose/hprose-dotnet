/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ExpandoObjectSerializer.cs                              |
|                                                          |
|  ExpandoObjectSerializer class for C#.                   |
|                                                          |
|  LastModified: Feb 18, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/
#if !NET35
using System.Collections.Generic;
using System.Dynamic;

namespace Hprose.IO.Serializers {
    using static Tags;

    internal class ExpandoObjectSerializer : ReferenceSerializer<ExpandoObject> {
        public override void Write(Writer writer, ExpandoObject obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            var dict = (IDictionary<string, object>)obj;
            int length = dict.Count;
            stream.WriteByte(TagMap);
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagOpenbrace);
            var strSerializer = StringSerializer.Instance;
            var serializer = Serializer.Instance;
            foreach (var pair in dict) {
                strSerializer.Serialize(writer, Accessor.UnifiedName(pair.Key));
                serializer.Serialize(writer, pair.Value);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
}
#endif