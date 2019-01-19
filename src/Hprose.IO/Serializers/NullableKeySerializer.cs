/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  NullableKeySerializer.cs                                |
|                                                          |
|  NullableKeySerializer class for C#.                     |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.Collections.Generic;

namespace Hprose.IO.Serializers {
    class NullableKeySerializer<T> : Serializer<NullableKey<T>> {
        public override void Write(Writer writer, NullableKey<T> obj) => Serializer<T>.Instance.Serialize(writer, obj.Value);
    }
}