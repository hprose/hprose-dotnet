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
 * LastModified: Apr 27, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Generic;
using System.IO;

using Hprose.IO.Converters;

namespace Hprose.IO.Deserializers {
    public class Reader {
        private readonly Stream _stream;
        private readonly ReaderRefer _refer;
        private readonly HproseMode _mode;
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

        public Stream Stream => _stream;
        public HproseMode Mode => _mode;

        public ClassInfo GetClassInfo(int index) => _ref[index];

        public Reader(Stream stream, bool simple = false, HproseMode mode = HproseMode.MemberMode) {
            _stream = stream;
            _refer = simple ? null : new ReaderRefer();
            _mode = mode;
        }

        public object Deserialize() => Deserializer.Instance.Deserialize(this);

        public object Deserialize(Type type) => Deserializer.Deserialize(this, type);

        public T Deserialize<T>() => Deserializer<T>.Instance.Deserialize(this);

        public object Read(int tag) => Deserializer.Instance.Read(this, tag);

        public T Read<T>(int tag) => Deserializer<T>.Instance.Read(this, tag);

        public void ReadClass() {
            string name = ValueReader.ReadString(_stream);
            int count = ValueReader.ReadCount(_stream);
            string[] names = new string[count];
            var strDeserialize = Deserializer<string>.Instance;
            for (int i = 0; i < count; ++i) {
                names[i] = strDeserialize.Deserialize(this);
            }
            _stream.ReadByte();
            _ref.Add(new ClassInfo(name, names));
        }

        public object ReadRef() {
            return _refer?.Read(ValueReader.ReadInt(_stream));
        }

        public T ReadRef<T>() {
            object obj = _refer?.Read(ValueReader.ReadInt(_stream));
            if (obj != null) {
                return Converter<T>.Convert(obj);
            }
            throw new InvalidCastException("Cannot convert " + obj.GetType().ToString() + " to " + typeof(T).ToString() + ".");
        }

        public void SetRef(object obj) => _refer?.Set(obj);

        public void SetRef(int index, object obj) => _refer?.Set(index, obj);

        public int LastRefIndex => _refer?.LastIndex ?? -1;

        public void Reset() {
            _refer?.Reset();
            _ref.Clear();
        }
    }
}
