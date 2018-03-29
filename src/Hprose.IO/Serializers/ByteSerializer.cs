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
 * ByteSerializer.cs                                      *
 *                                                        *
 * ByteSerializer class for C#.                           *
 *                                                        *
 * LastModified: Mar 28, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class ByteSerializer : Serializer<byte> {
        private static readonly ByteSerializer _instance = new ByteSerializer();
        public static ByteSerializer Instance => _instance;
        public override void Write(Writer writer, byte obj) => ValueWriter.Write(writer.Stream, obj);
    }
}
