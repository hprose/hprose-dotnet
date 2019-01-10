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
 * NullableKeyDeserializer.cs                             *
 *                                                        *
 * NullableKeyDeserializer class for C#.                  *
 *                                                        *
 * LastModified: Jan 11, 2019                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using Hprose.Collections.Generic;

namespace Hprose.IO.Deserializers {
    using static Tags;

    class NullableKeyDeserializer<T> : Deserializer<NullableKey<T>> {
        public override NullableKey<T> Read(Reader reader, int tag) {
            if (tag == TagNull) {
                return default;
            }
            return Deserializer<T>.Instance.Read(reader, tag);
        }
    }
}
