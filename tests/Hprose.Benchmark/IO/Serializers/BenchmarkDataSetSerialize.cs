using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;

using Hprose.IO;

using Newtonsoft.Json;

namespace Hprose.Benchmark.IO.Serializers {
    [ClrJob(isBaseline: true), CoreJob, MonoJob]
    [RPlotExporter, RankColumn]
    public class BenchmarkDataSetSerialize {
        private static DataSet MakeDataSet() {
            var dataSet = new DataSet();
            MakeParentTable(dataSet);
            MakeChildTable(dataSet);
            MakeDataRelation(dataSet);
            return dataSet;
        }

        private static void MakeParentTable(DataSet dataSet) {
            DataTable table = new DataTable("ParentTable");
            DataColumn column;
            DataRow row;

            column = new DataColumn {
                DataType = Type.GetType("System.Int32"),
                ColumnName = "id",
                ReadOnly = true,
                Unique = true
            };
            table.Columns.Add(column);

            column = new DataColumn {
                DataType = Type.GetType("System.String"),
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

        private static void MakeChildTable(DataSet dataSet) {
            DataTable table = new DataTable("childTable");
            DataColumn column;
            DataRow row;

            column = new DataColumn {
                DataType = Type.GetType("System.Int32"),
                ColumnName = "ChildID",
                AutoIncrement = true,
                Caption = "ID",
                ReadOnly = true,
                Unique = true
            };

            table.Columns.Add(column);

            column = new DataColumn {
                DataType = Type.GetType("System.String"),
                ColumnName = "ChildItem",
                AutoIncrement = false,
                Caption = "ChildItem",
                ReadOnly = false,
                Unique = false
            };
            table.Columns.Add(column);

            column = new DataColumn {
                DataType = Type.GetType("System.Int32"),
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

        private static void MakeDataRelation(DataSet dataSet) {
            DataColumn parentColumn =
                dataSet.Tables["ParentTable"].Columns["id"];
            DataColumn childColumn =
                dataSet.Tables["ChildTable"].Columns["ParentID"];
            DataRelation relation = new
                DataRelation("parent2Child", parentColumn, childColumn);
            dataSet.Tables["ChildTable"].ParentRelations.Add(relation);
        }

        private static DataSet dataSet = MakeDataSet();

        private static byte[] hproseData;
        private static byte[] dcData;
        private static string newtonData;

        static BenchmarkDataSetSerialize() {
            hproseData = HproseFormatter.Serialize(dataSet);
            newtonData = JsonConvert.SerializeObject(dataSet);
            using (MemoryStream stream = new MemoryStream()) {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(DataSet));
                js.WriteObject(stream, dataSet);
                stream.Position = 0;
                dcData = stream.ToArray();
            }
        }

        [Benchmark]
        public void HproseSerializeDataSet() => HproseFormatter.Serialize(dataSet);
        [Benchmark]
        public void NewtonJsonSerializeDataSet() => JsonConvert.SerializeObject(dataSet);
        [Benchmark]
        public void DataContractSerializeDataSet() {
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(DataSet));
            using (MemoryStream stream = new MemoryStream()) {
                js.WriteObject(stream, dataSet);
            }
        }
        [Benchmark]
        public void HproseDeserializeDataSet() => HproseFormatter.Deserialize<DataSet>(hproseData);
        [Benchmark]
        public void NewtonJsonDeserializeDataSet() => JsonConvert.DeserializeObject<DataSet>(newtonData);
        [Benchmark]
        public void DataContractDeserializeDataSet() {
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(DataSet));
            using (MemoryStream stream = new MemoryStream(dcData)) {
                js.ReadObject(stream);
            }
        }
    }
}
