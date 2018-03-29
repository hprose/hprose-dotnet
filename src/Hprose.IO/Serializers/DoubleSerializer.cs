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
 * DoubleSerializer.cs                                    *
 *                                                        *
 * DoubleSerializer class for C#.                         *
 *                                                        *
 * LastModified: Mar 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class DoubleSerializer : Serializer<double> {
        private static readonly DoubleSerializer _instance = new DoubleSerializer();
        public static DoubleSerializer Instance => _instance;
        public override void Write(Writer writer, double obj) => ValueWriter.Write(writer.Stream, obj);
    }
}
