/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  StackDeserializer.cs                                    |
|                                                          |
|  StackDeserializer class for C#.                         |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections.Generic;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class StackDeserializer<T> : Deserializer<Stack<T>> {
        public static Stack<T> Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            Stack<T> stack = new Stack<T>();
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
        public override Stack<T> Read(Reader reader, int tag) => tag switch
        {
            TagList => Read(reader),
            TagEmpty => new Stack<T>(),
            _ => base.Read(reader, tag),
        };
    }
}
