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
 * DecimalSerializer.cs                                   *
 *                                                        *
 * DecimalSerializer class for C#.                        *
 *                                                        *
 * LastModified: Mar 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class DecimalSerializer : Serializer<decimal> {
        private static readonly DecimalSerializer _instance = new DecimalSerializer();
        public static DecimalSerializer Instance => _instance;
        public override void Write(Writer writer, decimal obj) => ValueWriter.Write(writer.Stream, obj);
    }
}
