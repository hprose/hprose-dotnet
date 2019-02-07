/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  EnumSerializer.cs                                       |
|                                                          |
|  EnumSerializer class for C#.                            |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Serializers {
    internal class EnumSerializer<T> : Serializer<T> where T : struct, IComparable, IConvertible, IFormattable {
        public override void Write(Writer writer, T obj) {
            switch (obj.GetTypeCode()) {
                case TypeCode.Int32:
                    ValueWriter.Write(writer.Stream, obj.ToInt32(null));
                    break;
                case TypeCode.SByte:
                    ValueWriter.Write(writer.Stream, obj.ToSByte(null));
                    break;
                case TypeCode.Int16:
                    ValueWriter.Write(writer.Stream, obj.ToInt16(null));
                    break;
                case TypeCode.Int64:
                    ValueWriter.Write(writer.Stream, obj.ToInt64(null));
                    break;
                case TypeCode.UInt32:
                    ValueWriter.Write(writer.Stream, obj.ToUInt32(null));
                    break;
                case TypeCode.Byte:
                    ValueWriter.Write(writer.Stream, obj.ToByte(null));
                    break;
                case TypeCode.UInt16:
                    ValueWriter.Write(writer.Stream, obj.ToUInt16(null));
                    break;
                case TypeCode.UInt64:
                    ValueWriter.Write(writer.Stream, obj.ToUInt64(null));
                    break;
                case TypeCode.Boolean:
                    ValueWriter.Write(writer.Stream, obj.ToBoolean(null));
                    break;
                case TypeCode.Char:
                    ValueWriter.Write(writer.Stream, obj.ToChar(null));
                    break;
            }
        }
    }
}
