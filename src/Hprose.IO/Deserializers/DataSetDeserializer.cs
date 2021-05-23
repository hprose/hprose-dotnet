/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  DataSetDeserializer.cs                                  |
|                                                          |
|  DataSetDeserializer class for C#.                       |
|                                                          |
|  LastModified: Jun 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Data;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class DataSetDeserializer : Deserializer<DataSet> {
        private static DataSet Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            DataSet dataset = new DataSet();
            reader.AddReference(dataset);
            var deserializer = Deserializer<DataTable>.Instance;
            for (int i = 0; i < count; ++i) {
                dataset.Tables.Add(deserializer.Deserialize(reader));
            }
            stream.ReadByte();
            return dataset;
        }
        public override DataSet Read(Reader reader, int tag) => tag switch {
            TagList => Read(reader),
            TagEmpty => new DataSet(),
            _ => base.Read(reader, tag),
        };
    }
}
