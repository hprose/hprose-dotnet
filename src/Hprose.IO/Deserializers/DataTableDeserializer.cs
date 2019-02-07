/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  DataTableDeserializer.cs                                |
|                                                          |
|  DataTableDeserializer class for C#.                     |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Data;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class DataTableDeserializer : Deserializer<DataTable> {
        private static IDeserializer[] ReadMapAsFirstRow(Reader reader, DataTable table, Deserializer<string> strDeserializer) {
            var columns = table.Columns;
            var row = table.NewRow();
            reader.AddReference(row);
            var stream = reader.Stream;
            var deserializer = Deserializer.Instance;
            int count = ValueReader.ReadCount(stream);
            IDeserializer[] deserializers = new IDeserializer[count];
            for (int i = 0; i < count; ++i) {
                var name = strDeserializer.Deserialize(reader);
                var value = deserializer.Deserialize(reader);
                var type = value?.GetType() ?? typeof(string);
                deserializers[i] = Deserializer.GetInstance(type);
                var column = new DataColumn(name, type);
                columns.Add(column);
                row[column] = value ?? DBNull.Value;
            }
            table.Rows.Add(row);
            stream.ReadByte();
            return deserializers;
        }

        private static IDeserializer[] ReadObjectAsFirstRow(Reader reader, DataTable table) {
            var columns = table.Columns;
            var row = table.NewRow();
            reader.AddReference(row);
            var stream = reader.Stream;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            TypeInfo typeInfo = reader.GetTypeInfo(index);
            table.TableName = typeInfo.name;
            var names = typeInfo.names;
            var deserializer = Deserializer.Instance;
            var count = names.Length;
            IDeserializer[] deserializers = new IDeserializer[count];
            if (typeInfo.type != null) {
                var members = Accessor.GetMembers(typeInfo.type, reader.Mode);
                for (int i = 0; i < count; ++i) {
                    var name = names[i];
                    var member = members[name];
                    if (member != null) {
                        var type = Accessor.GetMemberType(member);
                        deserializers[i] = Deserializer.GetInstance(type);
                        var value = deserializers[i].Deserialize(reader);
                        var column = new DataColumn(member.Name, type);
                        columns.Add(column);
                        row[column] = value ?? DBNull.Value;
                    }
                    else {
                        var value = deserializer.Deserialize(reader);
                        var type = value?.GetType() ?? typeof(string);
                        deserializers[i] = Deserializer.GetInstance(type);
                        var column = new DataColumn(name, type);
                        columns.Add(column);
                        row[column] = value ?? DBNull.Value;
                    }
                }
            }
            else {
                for (int i = 0; i < count; ++i) {
                    var name = names[i];
                    var value = deserializer.Deserialize(reader);
                    var type = value?.GetType() ?? typeof(string);
                    deserializers[i] = Deserializer.GetInstance(type);
                    var column = new DataColumn(name, type);
                    columns.Add(column);
                    row[column] = value ?? DBNull.Value;
                }
            }
            table.Rows.Add(row);
            stream.ReadByte();
            return deserializers;
        }

        private static void ReadMapAsRow(Reader reader, DataTable table, Deserializer<string> strDeserializer, IDeserializer[] deserializers) {
            var row = table.NewRow();
            reader.AddReference(row);
            var stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            for (int i = 0; i < count; ++i) {
                var name = strDeserializer.Deserialize(reader);
                var value = deserializers[i].Deserialize(reader);
                row[name] = value ?? DBNull.Value;
            }
            table.Rows.Add(row);
            stream.ReadByte();
        }

        private static void ReadObjectAsRow(Reader reader, DataTable table, IDeserializer[] deserializers) {
            var row = table.NewRow();
            reader.AddReference(row);
            var stream = reader.Stream;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            var columns = table.Columns;
            var count = columns.Count;
            for (int i = 0; i < count; ++i) {
                var column = columns[i];
                var value = deserializers[i].Deserialize(reader);
                row[column] = value ?? DBNull.Value;
            }
            table.Rows.Add(row);
            stream.ReadByte();
        }

        private static DataTable Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            DataTable table = new DataTable();
            reader.AddReference(table);
            int tag = stream.ReadByte();
            if (count == 0) {
                return table;
            }
            if (tag == TagClass) {
                reader.ReadClass();
                tag = stream.ReadByte();
            }
            var deserializer = Deserializer.Instance;
            IDeserializer[] deserializers;
            switch (tag) {
                case TagObject:
                    deserializers = ReadObjectAsFirstRow(reader, table);
                    tag = stream.ReadByte();
                    for (int i = 1; i < count; ++i) {
                        ReadObjectAsRow(reader, table, deserializers);
                        tag = stream.ReadByte();
                    }
                    break;
                case TagMap:
                    var strDeserializer = Deserializer<string>.Instance;
                    deserializers = ReadMapAsFirstRow(reader, table, strDeserializer);
                    tag = stream.ReadByte();
                    for (int i = 1; i < count; ++i) {
                        ReadMapAsRow(reader, table, strDeserializer, deserializers);
                        tag = stream.ReadByte();
                    }
                    break;
                default:
                    throw new InvalidCastException("Cannot convert " + Tags.ToString(tag) + " to DataRow.");
            }
            return table;
        }

        public override DataTable Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagList:
                    return Read(reader);
                case TagEmpty:
                    return new DataTable();
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
