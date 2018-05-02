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
 * ConcurrentStackDeserializer.cs                         *
 *                                                        *
 * ConcurrentStackDeserializer class for C#.              *
 *                                                        *
 * LastModified: May 2, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.Buffers;
using System.Collections.Concurrent;
using System.IO;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class ConcurrentStackDeserializer<T> : Deserializer<ConcurrentStack<T>> {
        public static ConcurrentStack<T> Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            ConcurrentStack<T> stack = new ConcurrentStack<T>();
            reader.SetRef(stack);
            var deserializer = Deserializer<T>.Instance;
            T[] buffer = ArrayPool<T>.Shared.Rent(count);
            try {
                for (int i = 0; i < count; ++i) {
                    buffer[i] = deserializer.Deserialize(reader);
                }
                for (int i = count - 1; i >= 0; --i) {
                    stack.Push(buffer[i]);
                }
            }
            finally {
                ArrayPool<T>.Shared.Return(buffer, false);
            }
            stream.ReadByte();
            return stack;
        }
        public override ConcurrentStack<T> Read(Reader reader, int tag) {
            switch (tag) {
                case TagList:
                    return Read(reader);
                case TagEmpty:
                    return new ConcurrentStack<T>();
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
