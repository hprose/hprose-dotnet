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
 * NullableDeserializer.cs                                *
 *                                                        *
 * NullableDeserializer class for C#.                     *
 *                                                        *
 * LastModified: Jan 11, 2019                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Deserializers {
    using static Tags;

    class NullableDeserializer<T> : Deserializer<T?> where T : struct {
        public override T? Read(Reader reader, int tag) {
            if (tag == TagNull) {
                return null;
            }
            return Deserializer<T>.Instance.Read(reader, tag);
        }
    }
}
