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
 * LastModified: Dec 13, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Generic;
using System.IO;

using Hprose.IO.Converters;

namespace Hprose.IO.Deserializers {
    public class Reader {
        private readonly ReaderRefer _refer;
        private readonly List<ClassInfo> _ref = new List<ClassInfo>();
        private volatile LongType _defaultLongType = LongType.BigInteger;
        private volatile RealType _defaultRealType = RealType.Double;
        private volatile CharType _defaultCharType = CharType.String;
        private volatile ListType _defaultListType = ListType.List;
        private volatile DictType _defaultDictType = DictType.NullableKeyDictionary;

        public LongType DefaultLongType { get => _defaultLongType; set => _defaultLongType = value; }
        public RealType DefaultRealType { get => _defaultRealType; set => _defaultRealType = value; }
        public CharType DefaultCharType { get => _defaultCharType; set => _defaultCharType = value; }
        public ListType DefaultListType { get => _defaultListType; set => _defaultListType = value; }
        public DictType DefaultDictType { get => _defaultDictType; set => _defaultDictType = value; }

        public Stream Stream { get; private set; }
        public Mode Mode { get; private set; }

        public ClassInfo GetClassInfo(int index) => _ref[index];

        public Reader(Stream stream, Mode mode = Mode.MemberMode) {
            Stream = stream;
            Mode = mode;
            _refer = new ReaderRefer();
        }

        public Reader(Stream stream, bool simple, Mode mode = Mode.MemberMode) {
            Stream = stream;
            Mode = mode;
            _refer = simple ? null : new ReaderRefer();
        }

        public object Deserialize() => Deserializer.Instance.Deserialize(this);

        public object Deserialize(Type type) => Deserializer.Deserialize(this, type);

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
            _ref.Add(new ClassInfo(name, names));
        }

        public object ReadReference() {
            return _refer?.Read(ValueReader.ReadInt(Stream));
        }

        public T ReadReference<T>() {
            object obj = _refer?.Read(ValueReader.ReadInt(Stream));
            if (obj != null) {
                return Converter<T>.Convert(obj);
            }
            throw new InvalidCastException("Cannot convert " + obj.GetType().ToString() + " to " + typeof(T).ToString() + ".");
        }

        public void AddReference(object obj) => _refer?.Add(obj);

        public void SetReference(int index, object obj) => _refer?.Set(index, obj);

        public int LastRefIndex => _refer?.LastIndex ?? -1;

        public MemoryStream ReadRaw() {
            MemoryStream ostream = new MemoryStream();
            ReadRaw(ostream);
            ostream.Position = 0;
            return ostream;
        }

        public void ReadRaw(Stream ostream) {
            RawReader.ReadRaw(Stream, ostream, Stream.ReadByte());
        }

        public void Reset() {
            _refer?.Reset();
            _ref.Clear();
        }
    }
}
