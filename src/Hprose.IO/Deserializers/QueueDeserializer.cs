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
 * QueueDeserializer.cs                                   *
 *                                                        *
 * QueueDeserializer class for C#.                        *
 *                                                        *
 * LastModified: Dec 13, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System.IO;
using System.Collections.Generic;

using static Hprose.IO.Tags;

namespace Hprose.IO.Deserializers {
    class QueueDeserializer<T> : Deserializer<Queue<T>> {
        public static Queue<T> Read(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            Queue<T> queue = new Queue<T>();
            reader.AddReference(queue);
            var deserializer = Deserializer<T>.Instance;
            for (int i = 0; i < count; ++i) {
                queue.Enqueue(deserializer.Deserialize(reader));
            }
            stream.ReadByte();
            return queue;
        }
        public override Queue<T> Read(Reader reader, int tag) {
            switch (tag) {
                case TagList:
                    return Read(reader);
                case TagEmpty:
                    return new Queue<T>();
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
