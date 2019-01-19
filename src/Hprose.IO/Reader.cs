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
 * Reader.cs                                              *
 *                                                        *
 * hprose Reader class for C#.                            *
 *                                                        *
 * LastModified: Jan 19, 2019                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Generic;
using System.IO;

namespace Hprose.IO {
    sealed class ReaderRefer {
        private readonly List<object> @ref = new List<object>();
        public int LastIndex => @ref.Count - 1;
        public void Add(object obj) => @ref.Add(obj);
        public void Set(int index, object obj) => @ref[index] = obj;
        public object Read(int index) => @ref[index];
        public void Reset() => @ref.Clear();
    }

    public class Reader {
        private readonly ReaderRefer refer;
        private readonly List<TypeInfo> @ref = new List<TypeInfo>();
        private volatile LongType longType = LongType.BigInteger;
        private volatile RealType realType = RealType.Double;
        private volatile CharType charType = CharType.String;
        private volatile ListType listType = ListType.List;
        private volatile DictType dictType = DictType.NullableKeyDictionary;

        public LongType LongType { get => longType; set => longType = value; }
        public RealType RealType { get => realType; set => realType = value; }
        public CharType CharType { get => charType; set => charType = value; }
        public ListType ListType { get => listType; set => listType = value; }
        public DictType DictType { get => dictType; set => dictType = value; }

        public Stream Stream { get; private set; }
        public Mode Mode { get; private set; }

        public TypeInfo GetTypeInfo(int index) => @ref[index];

        public Reader(Stream stream, Mode mode = Mode.MemberMode) {
            Stream = stream;
            Mode = mode;
            refer = new ReaderRefer();
        }

        public Reader(Stream stream, bool simple, Mode mode = Mode.MemberMode) {
            Stream = stream;
            Mode = mode;
            refer = simple ? null : new ReaderRefer();
        }

        public object Deserialize() => Deserializer.Instance.Deserialize(this);

        public object Deserialize(Type type) => Deserializer.GetInstance(type).Deserialize(this);

        public T Deserialize<T>() => Deserializer<T>.Instance.Deserialize(this);

        public object Read(int tag) => Deserializer.Instance.Read(this, tag);

        public T Read<T>(int tag) => Deserializer<T>.Instance.Read(this, tag);

        public void ReadClass() {
            string name = ValueReader.ReadString(Stream);
            int count = ValueReader.ReadCount(Stream);
            string[] names = new string[count];
            var strDeserialize = Deserializer<string>.Instance;
            for (int i = 0; i < count; ++i) {
                names[i] = strDeserialize.Deserialize(this);
            }
            Stream.ReadByte();
            @ref.Add(new TypeInfo(name, names));
        }

        public object ReadReference() => refer?.Read(ValueReader.ReadInt(Stream));

        public T ReadReference<T>() {
            object obj = ReadReference();
            if (obj != null) {
                return Converter<T>.Convert(obj);
            }
            throw new InvalidCastException("Cannot convert " + obj.GetType().ToString() + " to " + typeof(T).ToString() + ".");
        }

        public void AddReference(object obj) => refer?.Add(obj);

        public void SetReference(int index, object obj) => refer?.Set(index, obj);

        public int LastReferenceIndex => refer?.LastIndex ?? -1;

        public void Reset() {
            refer?.Reset();
            @ref.Clear();
        }
    }
}
