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
 * DataSetDeserializer.cs                                 *
 *                                                        *
 * DataSetDeserializer class for C#.                      *
 *                                                        *
 * LastModified: Apr 26, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.Data;
using System.IO;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class DataSetDeserializer : Deserializer<DataSet> {
        private static DataSet Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            DataSet dataset = new DataSet();
            reader.SetRef(dataset);
            var deserializer = Deserializer<DataTable>.Instance;
            for (int i = 0; i < count; ++i) {
                dataset.Tables.Add(deserializer.Deserialize(reader));
            }
            stream.ReadByte();
            return dataset;
        }
        public override DataSet Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagList:
                    return Read(reader);
                case TagEmpty:
                    return new DataSet();
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
