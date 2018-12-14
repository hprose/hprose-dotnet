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
 * Deserializer.cs                                        *
 *                                                        *
 * hprose Deserializer class for C#.                      *
 *                                                        *
 * LastModified: Dec 13, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization;

using Hprose.Collections.Generic;

using static Hprose.IO.Tags;

namespace Hprose.IO.Deserializers {
    internal interface IDeserializer {
        object Read(Reader reader, int tag);
        object Deserialize(Reader reader);
    }

    public abstract class Deserializer<T> : IDeserializer {
        static Deserializer() => Deserializers.Initialize();
        private static volatile Deserializer<T> instance;
        public static Deserializer<T> Instance {
            get {
                if (instance == null) {
                    instance = Deserializers.GetInstance(typeof(T)) as Deserializer<T>;
                }
                return instance;
            }
        }
        public virtual T Read(Reader reader, int tag) {
            switch (tag) {
                case TagNull:
                    return default;
                case TagRef:
                    return reader.ReadReference<T>();
                case TagClass:
                    reader.ReadClass();
                    return Deserialize(reader);
                case TagError:
                    throw new SerializationException(reader.Deserialize<string>());
            }
            throw new InvalidCastException("Cannot convert " + Tags.ToString(tag) + " to " + typeof(T).ToString() + ".");
        }
        public virtual T Deserialize(Reader reader) => Read(reader, reader.Stream.ReadByte());
        object IDeserializer.Read(Reader reader, int tag) => Read(reader, tag);
        object IDeserializer.Deserialize(Reader reader) => Deserialize(reader);
    }
    public class Deserializer : Deserializer<object> {
        private static readonly object trueObject = true;
        private static readonly object falseObject = false;
        private static readonly object[] digitObject = new object[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public override object Read(Reader reader, int tag) {
            if (tag >= '0' && tag <= '9') {
                return digitObject[tag - '0'];
            }
            var stream = reader.Stream;
            switch (tag) {
                case TagInteger:
                    return ValueReader.ReadInt(stream);
                case TagString:
                    return ReferenceReader.ReadString(reader);
                case TagBytes:
                    return ReferenceReader.ReadBytes(reader);
                case TagTrue:
                    return trueObject;
                case TagFalse:
                    return falseObject;
                case TagEmpty:
                    return "";
                case TagObject:
                    return Read(reader);
                case TagRef:
                    return reader.ReadReference();
                case TagDate:
                    return ReferenceReader.ReadDateTime(reader);
                case TagTime:
                    return ReferenceReader.ReadTime(reader);
                case TagGuid:
                    return ReferenceReader.ReadGuid(reader);
                case TagLong:
                    switch (reader.DefaultLongType) {
                        case LongType.Int64:
                            return ValueReader.ReadLong(stream);
                        case LongType.UInt64:
                            return (ulong)ValueReader.ReadLong(stream);
                        default:
                            return ValueReader.ReadBigInteger(stream);
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
                case TagList:
                    switch (reader.DefaultListType) {
                        case ListType.Array:
                            return ReferenceReader.ReadArray<object>(reader);
                        case ListType.ArrayList:
                            return ListDeserializer<ArrayList>.Read(reader);
                        default:
                            return CollectionDeserializer<List<object>, object>.Read(reader);
                    }
                case TagMap:
                    switch (reader.DefaultDictType) {
                        case DictType.Dictionary:
                            return DictionaryDeserializer<Dictionary<object, object>, object, object>.Read(reader);
                        case DictType.ExpandoObject:
                            return StringObjectDictionaryDeserializer<ExpandoObject>.Read(reader);
                        case DictType.Hashtable:
                            return DictionaryDeserializer<Hashtable>.Read(reader);
                        default:
                            return DictionaryDeserializer<NullableKeyDictionary<object, object>, object, object>.Read(reader);
                    }
                default:
                    return base.Read(reader, tag);
            }
        }

        private object Read(Reader reader) {
            Stream stream = reader.Stream;
            int index = ValueReader.ReadInt(stream, TagOpenbrace);
            var classInfo = reader.GetClassInfo(index);
            var type = classInfo.type;
            if (type != null) {
                return ((IObjectDeserializer)Deserializers.GetInstance(type)).ReadObject(reader, classInfo.key);
            }
            var obj = new ExpandoObject();
            reader.AddReference(obj);
            var dict = (IDictionary<string, object>)obj;
            string[] names = classInfo.names;
            int count = names.Length;
            for (int i = 0; i < count; ++i) {
                dict.Add(names[i], Deserialize(reader));
            }
            stream.ReadByte();
            return obj;
        }
    }
}
