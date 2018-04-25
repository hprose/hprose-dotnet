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
 * StackDeserializer.cs                                   *
 *                                                        *
 * StackDeserializer class for C#.                        *
 *                                                        *
 * LastModified: Apr 16, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.IO;
using System.Collections.Generic;

using static Hprose.IO.HproseTags;

namespace Hprose.IO.Deserializers {
    class StackDeserializer<T> : Deserializer<Stack<T>> {
        public static Stack<T> Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            Stack<T> stack = new Stack<T>();
            reader.SetRef(stack);
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
        public override Stack<T> Read(Reader reader, int tag) {
            switch (tag) {
                case TagEmpty:
                    return new Stack<T>();
                case TagList:
                    return Read(reader);
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
