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
 * Int64Serializer.cs                                     *
 *                                                        *
 * Int64Serializer class for C#.                          *
 *                                                        *
 * LastModified: Mar 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class Int64Serializer : Serializer<long> {
        private static readonly Int64Serializer _instance = new Int64Serializer();
        public static Int64Serializer Instance => _instance;
        public override void Write(Writer writer, long obj) => ValueWriter.Write(writer.Stream, obj);
    }
}
