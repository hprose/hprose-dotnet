using System.Data;
using System.IO;

using Hprose.IO.Serializers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hprose.UnitTests.IO.Serializers {
    [TestClass]
    public class DataTableSerializerTests {
        [TestMethod]
        public void TestSerializeDataTable() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var table = MakeTable();
                writer.Serialize(table);
                writer.Serialize(table);
                Assert.AreEqual("a3{c9\"TestTable\"3{s2\"id\"s4\"name\"s3\"age\"}o0{0s5\"Mario\"i45;}o0{1s5\"Luigi\"i42;}o0{2s5\"Peach\"i28;}}r0;", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
        }

        private DataTable MakeTable() {
            DataTable table = new DataTable("TestTable");

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
