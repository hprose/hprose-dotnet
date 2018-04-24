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
 * DataSetSerializer.cs                                   *
 *                                                        *
 * DataSetSerializer class for C#.                        *
 *                                                        *
 * LastModified: Apr 24, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.Data;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Serializers {
    class DataSetSerializer<T> : ReferenceSerializer<T> where T : DataSet {
        public override void Write(Writer writer, T obj) {
            base.Write(writer, obj);
            var tables = obj.Tables;
            var length = tables.Count;
            var stream = writer.Stream;
            stream.WriteByte(TagMap);
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagOpenbrace);
            for (int i = 0; i < length; ++i) {
                Serializer<string>.Instance.Serialize(writer, tables[i].TableName);
                Serializer<DataTable>.Instance.Serialize(writer, tables[i]);
            }
            stream.WriteByte(TagClosebrace);
        }
    }
}
