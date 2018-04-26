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
    class DataSetDeserializer<T> : Deserializer<T> where T : DataSet, new() {
        private static T ReadListAsDataSet(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T dataset = new T();
            reader.SetRef(dataset);
            var deserializer = Deserializer<DataTable>.Instance;
            for (int i = 0; i < count; ++i) {
                dataset.Tables.Add(deserializer.Deserialize(reader));
            }
            stream.ReadByte();
            return dataset;
        }
        private static T ReadMapAsDataSet(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T dataset = new T();
            reader.SetRef(dataset);
            var strDeserializer = Deserializer<string>.Instance;
            var deserializer = Deserializer<DataTable>.Instance;
            for (int i = 0; i < count; ++i) {
                var name = strDeserializer.Deserialize(reader);
                var table = deserializer.Deserialize(reader);
                table.TableName = name;
                dataset.Tables.Add(table);
            }
            stream.ReadByte();
            return dataset;
        }
        public override T Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagList:
                    return ReadListAsDataSet(reader);
                case TagMap:
                    return ReadMapAsDataSet(reader);
                case TagEmpty:
                    return new T();
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
