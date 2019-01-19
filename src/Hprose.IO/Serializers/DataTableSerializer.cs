/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  DataTableSerializer.cs                                  |
|                                                          |
|  DataTableSerializer class for C#.                       |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Data;

namespace Hprose.IO.Serializers {
    using static Tags;

    class DataTableSerializer<T> : ReferenceSerializer<T> where T : DataTable {
        public override void Write(Writer writer, T obj) {
            base.Write(writer, obj);
            var rows = obj.Rows;
            var length = rows.Count;
            var stream = writer.Stream;
            stream.WriteByte(TagList);
            if (length == 0) {
                stream.WriteByte(TagOpenbrace);
                stream.WriteByte(TagClosebrace);
                return;
            }
            ValueWriter.WriteInt(stream, length);
            stream.WriteByte(TagOpenbrace);
            var columns = obj.Columns;
            int count = columns.Count;
            int r = writer.WriteClass(columns, () => {
                stream.WriteByte(TagClass);
                ValueWriter.Write(stream, obj.TableName);
                ValueWriter.Write(stream, count);
                stream.WriteByte(TagOpenbrace);
                var strSerializer = Serializer<string>.Instance;
                for (int i = 0; i < count; ++i) {
                    strSerializer.Serialize(writer, Accessor.UnifiedName(columns[i].ColumnName));
                }
                stream.WriteByte(TagClosebrace);
            });
            var serializer = Serializer.Instance;
            for (int i = 0; i < length; ++i) {
                writer.AddReferenceCount(1);
                stream.WriteByte(TagObject);
                ValueWriter.WriteInt(stream, r);
                stream.WriteByte(TagOpenbrace);
                if (count > 0) {
                    var row = rows[i];
                    for (int j = 0; j < count; ++j) {
                        serializer.Serialize(writer, row[j]);
                    }
                }
                stream.WriteByte(TagClosebrace);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
}
