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
 * LastModified: Apr 17, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using static Hprose.IO.Tags;

namespace Hprose.IO.Deserializers {
    class NullableDeserializer<T> : Deserializer<T?> where T : struct {
        public override T? Read(Reader reader, int tag) {
            if (tag == TagNull) {
                return null;
            }
            return Deserializer<T>.Instance.Read(reader, tag);
        }
    }
}
