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
 * CharSerializer.cs                                      *
 *                                                        *
 * CharSerializer class for C#.                           *
 *                                                        *
 * LastModified: Mar 28, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class CharSerializer : Serializer<char> {
        private static readonly CharSerializer instance = new CharSerializer();
        public static CharSerializer Instance => instance;
        public override void Write(Writer writer, char obj) => ValueWriter.Write(writer.Stream, obj);
    }
}
