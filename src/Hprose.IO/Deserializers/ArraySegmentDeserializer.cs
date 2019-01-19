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
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;

namespace Hprose.IO.Deserializers {
    using static Tags;

    class ArraySegmentDeserializer<T> : Deserializer<ArraySegment<T>> {
        private static readonly T[] empty = new T[0] { };
        public override ArraySegment<T> Read(Reader reader, int tag) {
            switch (tag) {
                case TagList:
                    return new ArraySegment<T>(ReferenceReader.ReadArray<T>(reader));
                case TagEmpty:
                    return new ArraySegment<T>(empty);
                case TagRef:
                    object obj = reader.ReadReference();
                    if (obj.GetType().IsArray) {
                        return new ArraySegment<T>((T[])obj);
                    }
                    throw new InvalidCastException("Cannot convert " + obj.GetType().ToString() + " to " + typeof(T).ToString() + ".");
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
