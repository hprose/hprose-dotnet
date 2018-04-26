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
using System.Runtime.Serialization;
using Hprose.IO.Accessors;
using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class DataTableDeserializer<T> : Deserializer<T> where T : DataTable, new() {
        private static object ReadValue(Reader reader) {
            var stream = reader.Stream;
            int tag = stream.ReadByte();
            if (tag >= '0' && tag <= '9') {
                return (tag - '0');
            }
            switch (tag) {
                case TagInteger:
                    return ValueReader.ReadInt(stream);
                case TagLong:
                    switch (reader.DefaultLongType) {
                        case LongType.Int64:
                            return ValueReader.ReadLong(stream);
                        case LongType.UInt64:
                            return (ulong)ValueReader.ReadLong(stream);
                        default:
                            return new TimeSpan(ValueReader.ReadLong(stream));
                    }
                case TagDouble:
                    switch (reader.DefaultRealType) {
                        case RealType.Single:
                            return ValueReader.ReadSingle(stream);
                        case RealType.Decimal:
                            return ValueReader.ReadDecimal(stream);
                        default:
                            return ValueReader.ReadDouble(stream);
                    }
                case TagNull:
                    return DBNull.Value;
                case TagEmpty:
                    return "";
                case TagTrue:
                    return true;
                case TagFalse:
                    return false;
                case TagNaN:
                    switch (reader.DefaultRealType) {
                        case RealType.Single:
                            return float.NaN;
                        default:
                            return double.NaN;
                    }
                case TagInfinity:
                    switch (reader.DefaultRealType) {
                        case RealType.Single:
                            return ValueReader.ReadSingleInfinity(stream);
                        default:
                            return ValueReader.ReadInfinity(stream);
                    }
                case TagUTF8Char:
                    switch (reader.DefaultCharType) {
                        case CharType.Char:
                            return ValueReader.ReadChar(stream);
                        default:
                            return ValueReader.ReadUTF8Char(stream);
                    }
                case TagString:
                    return ReferenceReader.ReadString(reader);
                case TagBytes:
                    return ReferenceReader.ReadBytes(reader);
                case TagDate:
                    return ReferenceReader.ReadDateTime(reader);
                case TagTime:
                    return ReferenceReader.ReadTime(reader);
                case TagGuid:
                    return ReferenceReader.ReadGuid(reader);
                case TagList:
                    return ReferenceReader.ReadArray<byte>(reader);
                case TagRef:
                    return reader.ReadRef<object>();
                case TagClass:
                    reader.ReadClass();
                    return ReadValue(reader);
                case TagError:
                    throw new SerializationException(reader.Deserialize<string>());
                default:
                    throw new InvalidCastException("Cannot convert " + HproseTags.ToString(tag) + " to DataTable value.");
            }
        }
        private static void ReadMapAsFirstRow(Reader reader, DataTable table) {
            var columns = table.Columns;
            var row = table.NewRow();
            reader.SetRef(row);
            var stream = reader.Stream;
            var strDeserializer = Deserializer<string>.Instance;
            int count = ValueReader.ReadCount(stream);
            for (int i = 0; i < count; ++i) {
                var name = strDeserializer.Deserialize(reader);
                var value = ReadValue(reader);
                var column = new DataColumn(name, value?.GetType() ?? typeof(string));
                columns.Add(column);
                row[column] = value;
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
                        var value = ReadValue(reader);
                        var column = new DataColumn(name, value?.GetType() ?? typeof(string));
                        columns.Add(column);
                        row[column] = value;
                    }
                }
            }
            else {
                for (int i = 0; i < count; ++i) {
                    var name = names[i];
                    var value = ReadValue(reader);
                    var column = new DataColumn(name, value?.GetType() ?? typeof(string));
                    columns.Add(column);
                    row[column] = value;
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
                var value = ReadValue(reader);
                row[name] = value;
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
                        var value = ReadValue(reader);
                        row[name] = value;
                    }
                }
            }
            else {
                for (int i = 0; i < count; ++i) {
                    var name = names[i];
                    var value = ReadValue(reader);
                    row[name] = value;
                }
            }
            table.Rows.Add(row);
            stream.ReadByte();
        }

        private static void ReadFirstRow(Reader reader, DataTable table, int tag) {
            switch (tag) {
                case TagMap:
                    ReadMapAsFirstRow(reader, table);
                    break;
                case TagObject:
                    ReadObjectAsFirstRow(reader, table);
                    break;
                default:
                    throw new InvalidCastException("Cannot convert " + HproseTags.ToString(tag) + " to DataRow.");
            }
        }

        private static void ReadRow(Reader reader, DataTable table, int tag) {
            switch (tag) {
                case TagMap:
                    ReadMapAsRow(reader, table);
                    break;
                case TagObject:
                    ReadObjectAsRow(reader, table);
                    break;
                default:
                    throw new InvalidCastException("Cannot convert " + HproseTags.ToString(tag) + " to DataRow.");
            }
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
            ReadFirstRow(reader, table, tag);
            tag = stream.ReadByte();
            for (int i = 1; i < count; ++i) {
                ReadRow(reader, table, tag);
                tag = stream.ReadByte();
            }
            return table;
        }

        public override T Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagEmpty:
                    return new T();
                case TagList:
                    return Read(reader);
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
