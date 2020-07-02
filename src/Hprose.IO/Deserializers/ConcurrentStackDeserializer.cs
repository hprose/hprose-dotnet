/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ConcurrentStackDeserializer.cs                          |
|                                                          |
|  ConcurrentStackDeserializer class for C#.               |
|                                                          |
|  LastModified: Jun 29, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections.Concurrent;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class ConcurrentStackDeserializer<T> : Deserializer<ConcurrentStack<T>> {
        public static ConcurrentStack<T> Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            ConcurrentStack<T> stack = new ConcurrentStack<T>();
            reader.AddReference(stack);
            T[] array = new T[count];
            var deserializer = Deserializer<T>.Instance;
            for (int i = 0; i < count; ++i) {
                array[i] = deserializer.Deserialize(reader);
            }
            for (int i = count - 1; i >= 0; --i) {
                stack.Push(array[i]);
            }
            stream.ReadByte();
            return stack;
        }
        public override ConcurrentStack<T> Read(Reader reader, int tag) => tag switch
        {
            TagList => Read(reader),
            TagEmpty => new ConcurrentStack<T>(),
            _ => base.Read(reader, tag),
        };
    }
}
