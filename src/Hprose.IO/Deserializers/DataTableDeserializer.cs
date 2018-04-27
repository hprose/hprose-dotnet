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
 * DataTableDeserializer.cs                               *
 *                                                        *
 * DataTableDeserializer class for C#.                    *
 *                                                        *
 * LastModified: Apr 26, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Data;
using System.IO;

using Hprose.IO.Accessors;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class DataTableDeserializer<T> : Deserializer<T> where T : DataTable, new() {
        private static void ReadMapAsFirstRow(Reader reader, DataTable table) {
            var columns = table.Columns;
            var row = table.NewRow();
            reader.SetRef(row);
            var stream = reader.Stream;
            var strDeserializer = Deserializer<string>.Instance;
            int count = ValueReader.ReadCount(stream);
            for (int i = 0; i < count; ++i) {
                var name = strDeserializer.Deserialize(reader);
                var value = reader.Deserialize();
                var column = new DataColumn(name, value?.GetType() ?? typeof(string));
                columns.Add(column);
                row[column] = value ?? DBNull.Value;
            }
            table.Rows.Add(row);
            stream.ReadByte();
        }

        private static void ReadObjectAsFirstRow(Reader reader, DataTable table) {
            var columns = table.Columns;
            var row = table.NewRow();
            reader.SetRef(row);
            var stream = reader.Stream;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            ClassInfo classInfo = reader[index];
            table.TableName = classInfo.name;
            var names = classInfo.names;
            var count = names.Length;
            if (classInfo.type != null) {
                var members = Accessor.GetMembers(classInfo.type, reader.Mode);
                for (int i = 0; i < count; ++i) {
                    var name = names[i];
                    var member = members[name];
                    if (member != null) {
                        var type = Accessor.GetMemberType(member);
                        var value = reader.Deserialize(type);
                        var column = new DataColumn(member.Name, type);
                        columns.Add(column);
                        row[column] = value ?? DBNull.Value;
                    }
                    else {
                        var value = reader.Deserialize();
                        var column = new DataColumn(name, value?.GetType() ?? typeof(string));
                        columns.Add(column);
                        row[column] = value ?? DBNull.Value;
                    }
                }
            }
            else {
                for (int i = 0; i < count; ++i) {
                    var name = names[i];
                    var value = reader.Deserialize();
                    var column = new DataColumn(name, value?.GetType() ?? typeof(string));
                    columns.Add(column);
                    row[column] = value ?? DBNull.Value;
                }
            }
            table.Rows.Add(row);
            stream.ReadByte();
        }

        private static void ReadMapAsRow(Reader reader, DataTable table) {
            var row = table.NewRow();
            reader.SetRef(row);
            var stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            var strDeserializer = Deserializer<string>.Instance;
            for (int i = 0; i < count; ++i) {
                var name = strDeserializer.Deserialize(reader);
                var value = reader.Deserialize();
                row[name] = value ?? DBNull.Value;
            }
            table.Rows.Add(row);
            stream.ReadByte();
        }

        private static void ReadObjectAsRow(Reader reader, DataTable table) {
            var row = table.NewRow();
            reader.SetRef(row);
            var stream = reader.Stream;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            ClassInfo classInfo = reader[index];
            table.TableName = classInfo.name;
            var names = classInfo.names;
            var count = names.Length;
            if (classInfo.type != null) {
                var members = Accessor.GetMembers(classInfo.type, reader.Mode);
                for (int i = 0; i < count; ++i) {
                    var name = names[i];
                    var member = members[name];
                    if (member != null) {
                        var type = Accessor.GetMemberType(member);
                        var value = reader.Deserialize(type);
                        row[member.Name] = value ?? DBNull.Value;
                    }
                    else {
                        var value = reader.Deserialize();
                        row[name] = value ?? DBNull.Value;
                    }
                }
            }
            else {
                for (int i = 0; i < count; ++i) {
                    var name = names[i];
                    var value = reader.Deserialize();
                    row[name] = value ?? DBNull.Value;
                }
            }
            table.Rows.Add(row);
            stream.ReadByte();
        }

        private static T Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T table = new T();
            reader.SetRef(table);
            int tag = stream.ReadByte();
            if (count == 0) {
                return table;
            }
            if (tag == TagClass) {
                reader.ReadClass();
                tag = stream.ReadByte();
            }
            switch (tag) {
                case TagObject:
                    ReadObjectAsFirstRow(reader, table);
                    tag = stream.ReadByte();
                    for (int i = 1; i < count; ++i) {
                        ReadObjectAsRow(reader, table);
                        tag = stream.ReadByte();
                    }
                    break;
                case TagMap:
                    ReadMapAsFirstRow(reader, table);
                    tag = stream.ReadByte();
                    for (int i = 1; i < count; ++i) {
                        ReadMapAsRow(reader, table);
                        tag = stream.ReadByte();
                    }
                    break;
                default:
                    throw new InvalidCastException("Cannot convert " + HproseTags.ToString(tag) + " to DataRow.");
            }
            return table;
        }

        public override T Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagList:
                    return Read(reader);
                case TagEmpty:
                    return new T();
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
