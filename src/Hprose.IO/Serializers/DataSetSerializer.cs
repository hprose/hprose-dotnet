/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  DataSetSerializer.cs                                    |
|                                                          |
|  DataSetSerializer class for C#.                         |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Data;

namespace Hprose.IO.Serializers {
    using static Tags;

    class DataSetSerializer<T> : ReferenceSerializer<T> where T : DataSet {
        public override void Write(Writer writer, T obj) {
            base.Write(writer, obj);
            var tables = obj.Tables;
            var length = tables.Count;
            var stream = writer.Stream;
            stream.WriteByte(TagList);
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagOpenbrace);
            for (int i = 0; i < length; ++i) {
                Serializer<DataTable>.Instance.Serialize(writer, tables[i]);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
}
