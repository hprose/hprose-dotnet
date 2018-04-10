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
 * LastModified: Apr 9, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Generic;
using System.IO;

namespace Hprose.IO.Deserializers {
    public class Reader {
        private readonly Stream _stream;
        private readonly ReaderRefer _refer;
        private readonly HproseMode _mode;
        private readonly List<ClassInfo> _ref = new List<ClassInfo>();

        public Stream Stream => _stream;
        public HproseMode Mode => _mode;

        public Reader(Stream stream, bool simple = false, HproseMode mode = HproseMode.MemberMode) {
            _stream = stream;
            _refer = simple ? null : new ReaderRefer();
            _mode = mode;
        }

        public object Deserialize() => Deserializer.Instance.Deserialize(this);

        public T Deserialize<T>() => Deserializer<T>.Instance.Deserialize(this);

        public object Read(int tag) => Deserializer.Instance.Read(this, tag);

        public T Read<T>(int tag) => Deserializer<T>.Instance.Read(this, tag);

        public void ReadClass() {
            string name = ValueReader.ReadString(_stream);
            int count = ValueReader.ReadCount(_stream);
            string[] members = new string[count];
            var strDeserialize = Deserializer<string>.Instance;
            for (int i = 0; i < count; ++i) {
                members[i] = strDeserialize.Deserialize(this);
            }
            _stream.ReadByte();
            _ref.Add(new ClassInfo {
                Name = name,
                Type = ClassManager.GetType(name),
                Members = members
            });
        }

        internal T ReadRef<T>() {
            object obj = _refer?.Read(ValueReader.ReadInt(_stream));
            if (obj != null) {
                return Converters.Converter<T>.Instance.Convert(obj);
            }
            throw new InvalidCastException("Cannot convert " + obj.GetType().ToString() + " to " + typeof(T).ToString() + ".");
        }

        internal void SetRef(object obj) => _refer?.Set(obj);

        public void Reset() {
            _refer?.Reset();
            _ref.Clear();
        }
    }
}
