/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  BigIntegerSerializer.cs                                 |
|                                                          |
|  BigIntegerSerializer class for C#.                      |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Numerics;

namespace Hprose.IO.Serializers {
    class BigIntegerSerializer : Serializer<BigInteger> {
        public override void Write(Writer writer, BigInteger obj) => ValueWriter.Write(writer.Stream, obj);
    }
}
