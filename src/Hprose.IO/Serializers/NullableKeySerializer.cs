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
 * NullableKeySerializer.cs                               *
 *                                                        *
 * NullableKeySerializer class for C#.                    *
 *                                                        *
 * LastModified: Mar 30, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using Hprose.Collections.Generic;

namespace Hprose.IO.Serializers {
    class NullableKeySerializer<T> : Serializer<NullableKey<T>> {
        public override void Write(Writer writer, NullableKey<T> obj) => Serializer<T>.Instance.Write(writer, obj.Value);
    }
}