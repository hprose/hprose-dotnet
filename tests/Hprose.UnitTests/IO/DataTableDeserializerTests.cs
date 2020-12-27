using Hprose.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.IO;

namespace Hprose.UnitTests.IO {
    [TestClass]
    public class DataTableDeserializerTests {
        [TestMethod]
        public void TestDeserializeDataTable() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var table = MakeTable();
                writer.Serialize(table);
                writer.Serialize(table);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                var table2 = reader.Deserialize<DataTable>();
                var table3 = reader.Deserialize<DataTable>();
                var rows = table.Rows;
                Assert.AreEqual(table.TableName, table2.TableName);
                for (int i = 0; i < rows.Count; ++i) {
                    var row = rows[i];
                    var row2 = table2.Rows[i];
                    Assert.AreEqual(row["Id"], row2["Id"]);
                    Assert.AreEqual(row["Name"], row2["Name"]);
                    Assert.AreEqual(row["Age"], row2["Age"]);
                    Assert.AreEqual(row[0], row2[0]);
                    Assert.AreEqual(row[1], row2[1]);
                    Assert.AreEqual(row[2], row2[2]);
                }
                Assert.AreEqual(table2, table3);
            }
        }

        private DataTable MakeTable() {
            DataTable table = new DataTable("Test_Table");

            table.Columns.Add(new DataColumn("Id", typeof(int)));
            table.Columns.Add(new DataColumn("Name", typeof(string)));
            table.Columns.Add(new DataColumn("Age", typeof(int)));

            table.Rows.Add(0, "Mario", 45);
            table.Rows.Add(1, "Luigi", 42);
            table.Rows.Add(2, "Peach", 28);

            return table;
        }
    }
}
