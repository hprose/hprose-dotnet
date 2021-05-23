/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ArraySegmentDeserializer.cs                             |
|                                                          |
|  ArraySegmentDeserializer class for C#.                  |
|                                                          |
|  LastModified: Jun 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class ArraySegmentDeserializer<T> : Deserializer<ArraySegment<T>> {
        private static readonly T[] empty = new T[0];
        public override ArraySegment<T> Read(Reader reader, int tag) => tag switch {
            TagList => new ArraySegment<T>(ReferenceReader.ReadArray<T>(reader)),
            TagEmpty => new ArraySegment<T>(empty),
            TagRef => ReadReference(reader),
            _ => base.Read(reader, tag),
        };

        private static ArraySegment<T> ReadReference(Reader reader) {
            object obj = reader.ReadReference();
            if (obj.GetType().IsArray) {
                return new ArraySegment<T>((T[])obj);
            }
            throw new InvalidCastException("Cannot convert " + obj.GetType().ToString() + " to " + typeof(T).ToString() + ".");
        }
    }
}
