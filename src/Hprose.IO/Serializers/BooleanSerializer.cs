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
 * BooleanSerializer.cs                                   *
 *                                                        *
 * BooleanSerializer class for C#.                        *
 *                                                        *
 * LastModified: Mar 28, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class BooleanSerializer : Serializer<bool> {
        private static readonly BooleanSerializer _instance = new BooleanSerializer();
        public static BooleanSerializer Instance => _instance;
        public override void Write(Writer writer, bool obj) => ValueWriter.Write(writer.Stream, obj);
    }
}
