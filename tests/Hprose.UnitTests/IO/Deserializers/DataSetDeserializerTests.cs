using System.Data;
using System.IO;

using Hprose.IO.Serializers;
using Hprose.IO.Deserializers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hprose.UnitTests.IO.Deserializers {
    [TestClass]
    public class DataSetDeserializerTests {
        [TestMethod]
        public void TestDeserializeDataSet() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var dataSet = MakeDataSet();
                writer.Serialize(dataSet);
                writer.Serialize(dataSet);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                var dataSet2 = reader.Deserialize<DataSet>();
                var dataSet3 = reader.Deserialize<DataSet>();
                Assert.AreEqual(dataSet2, dataSet3);
                Assert.AreEqual(dataSet.Tables[0].TableName, dataSet2.Tables[0].TableName);
                Assert.AreEqual(dataSet.Tables[0].Rows[0][0], dataSet2.Tables[0].Rows[0][0]);
                Assert.AreEqual(dataSet.Tables[0].Rows[0][1], dataSet2.Tables[0].Rows[0][1]);
                Assert.AreEqual(dataSet.Tables[1].TableName, dataSet2.Tables[1].TableName);
                Assert.AreEqual(dataSet.Tables[1].Rows[0][0], dataSet2.Tables[1].Rows[0][0]);
                Assert.AreEqual(dataSet.Tables[1].Rows[0][1], dataSet2.Tables[1].Rows[0][1]);
                Assert.AreEqual(dataSet.Tables[1].Rows[0][2], dataSet2.Tables[1].Rows[0][2]);
                Assert.AreEqual(dataSet.Tables[1].Rows[10][0], dataSet2.Tables[1].Rows[10][0]);
                Assert.AreEqual(dataSet.Tables[1].Rows[10][1], dataSet2.Tables[1].Rows[10][1]);
                Assert.AreEqual(dataSet.Tables[1].Rows[10][2], dataSet2.Tables[1].Rows[10][2]);
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
