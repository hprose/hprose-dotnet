/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  NullableKeyDeserializer.cs                              |
|                                                          |
|  NullableKeyDeserializer class for C#.                   |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.Collections.Generic;

namespace Hprose.IO.Deserializers {
    using static Tags;

    internal class NullableKeyDeserializer<T> : Deserializer<NullableKey<T>> {
        public override NullableKey<T> Read(Reader reader, int tag) {
            if (tag == TagNull) {
                return default;
            }
            return Deserializer<T>.Instance.Read(reader, tag);
        }
    }
}
