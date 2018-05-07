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
 * LastModified: Apr 17, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using Hprose.Collections.Generic;

using static Hprose.IO.Tags;

namespace Hprose.IO.Deserializers {
    class NullableKeyDeserializer<T> : Deserializer<NullableKey<T>> {
        public override NullableKey<T> Read(Reader reader, int tag) {
            if (tag == TagNull) {
                return default;
            }
            return Deserializer<T>.Instance.Read(reader, tag);
        }
    }
}
