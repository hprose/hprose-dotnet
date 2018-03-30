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
 * NullSerializer.cs                                      *
 *                                                        *
 * NullSerializer class for C#.                           *
 *                                                        *
 * LastModified: Mar 30, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class NullSerializer : Serializer {
        private static NullSerializer _instance = new NullSerializer();
        public static NullSerializer Instance => _instance;
        public override void Write(Writer writer, object obj) => writer.Stream.WriteByte(HproseTags.TagNull);
    }
}