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
 * BigIntegerSerializer.cs                                *
 *                                                        *
 * BigIntegerSerializer class for C#.                     *
 *                                                        *
 * LastModified: Mar 30, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System.Numerics;

namespace Hprose.IO.Serializers {
    class BigIntegerSerializer : Serializer<BigInteger> {
        public override void Write(Writer writer, BigInteger obj) => ValueWriter.Write(writer.Stream, obj);
    }
}
