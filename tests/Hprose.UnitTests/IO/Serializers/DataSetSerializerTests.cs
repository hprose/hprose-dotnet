using System.Data;
using System.IO;

using Hprose.IO.Serializers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hprose.UnitTests.IO.Serializers {
    [TestClass]
    public class DataSetSerializerTests {
        [TestMethod]
        public void TestSerializeDataSet() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var dataSet = MakeDataSet();
                writer.Serialize(dataSet);
                writer.Serialize(dataSet);
                Assert.AreEqual("m2{s11\"ParentTable\"a3{c11\"ParentTable\"2{s2\"id\"s10\"parentItem\"}o0{0s12\"ParentItem 0\"}o0{1s12\"ParentItem 1\"}o0{2s12\"ParentItem 2\"}}s10\"childTable\"a15{c10\"childTable\"3{s7\"childID\"s9\"childItem\"s8\"parentID\"}o1{0s6\"Item 0\"0}o1{1s6\"Item 1\"0}o1{2s6\"Item 2\"0}o1{3s6\"Item 3\"0}o1{4s6\"Item 4\"0}o1{5r17;1}o1{6r19;1}o1{7r21;1}o1{8r23;1}o1{9r25;1}o1{i10;r17;2}o1{i11;r19;2}o1{i12;r21;2}o1{i13;r23;2}o1{i14;r25;2}}}r0;", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
        }

        private DataSet MakeDataSet() {
            var dataSet = new DataSet();
            MakeParentTable(dataSet);
            MakeChildTable(dataSet);
            MakeDataRelation(dataSet);
            return dataSet;
        }

        private void MakeParentTable(DataSet dataSet) {
            DataTable table = new DataTable("ParentTable");
            DataColumn column;
            DataRow row;

            column = new DataColumn {
                DataType = System.Type.GetType("System.Int32"),
                ColumnName = "id",
                ReadOnly = true,
                Unique = true
            };
            table.Columns.Add(column);

            column = new DataColumn {
                DataType = System.Type.GetType("System.String"),
                ColumnName = "ParentItem",
                AutoIncrement = false,
                Caption = "ParentItem",
                ReadOnly = false,
                Unique = false
            };
            table.Columns.Add(column);

            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = table.Columns["id"];
            table.PrimaryKey = PrimaryKeyColumns;

            dataSet.Tables.Add(table);

            for (int i = 0; i <= 2; i++) {
                row = table.NewRow();
                row["id"] = i;
                row["ParentItem"] = "ParentItem " + i;
                table.Rows.Add(row);
            }
        }

        private void MakeChildTable(DataSet dataSet) {
            DataTable table = new DataTable("childTable");
            DataColumn column;
            DataRow row;

            column = new DataColumn {
                DataType = System.Type.GetType("System.Int32"),
                ColumnName = "ChildID",
                AutoIncrement = true,
                Caption = "ID",
                ReadOnly = true,
                Unique = true
            };

            table.Columns.Add(column);

            column = new DataColumn {
                DataType = System.Type.GetType("System.String"),
                ColumnName = "ChildItem",
                AutoIncrement = false,
                Caption = "ChildItem",
                ReadOnly = false,
                Unique = false
            };
            table.Columns.Add(column);

            column = new DataColumn {
                DataType = System.Type.GetType("System.Int32"),
                ColumnName = "ParentID",
                AutoIncrement = false,
                Caption = "ParentID",
                ReadOnly = false,
                Unique = false
            };
            table.Columns.Add(column);

            dataSet.Tables.Add(table);

            for (int i = 0; i <= 4; i++) {
                row = table.NewRow();
                row["childID"] = i;
                row["ChildItem"] = "Item " + i;
                row["ParentID"] = 0;
                table.Rows.Add(row);
            }
            for (int i = 0; i <= 4; i++) {
                row = table.NewRow();
                row["childID"] = i + 5;
                row["ChildItem"] = "Item " + i;
                row["ParentID"] = 1;
                table.Rows.Add(row);
            }
            for (int i = 0; i <= 4; i++) {
                row = table.NewRow();
                row["childID"] = i + 10;
                row["ChildItem"] = "Item " + i;
                row["ParentID"] = 2;
                table.Rows.Add(row);
            }
        }

        private void MakeDataRelation(DataSet dataSet) {
            DataColumn parentColumn =
                dataSet.Tables["ParentTable"].Columns["id"];
            DataColumn childColumn =
                dataSet.Tables["ChildTable"].Columns["ParentID"];
            DataRelation relation = new
                DataRelation("parent2Child", parentColumn, childColumn);
            dataSet.Tables["ChildTable"].ParentRelations.Add(relation);
        }
    }
}
