using Hprose.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.IO;
using System.Text;

namespace Hprose.UnitTests.IO {
    [TestClass]
    public class DataTableSerializerTests {
        [TestMethod]
        public void TestSerializeDataTable() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var table = MakeTable();
                writer.Serialize(table);
                writer.Serialize(table);
                var data = stream.GetArraySegment();
                Assert.AreEqual("a3{c9\"TestTable\"3{s2\"id\"s4\"name\"s3\"age\"}o0{0s5\"Mario\"i45;}o0{1s5\"Luigi\"i42;}o0{2s5\"Peach\"i28;}}r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
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
