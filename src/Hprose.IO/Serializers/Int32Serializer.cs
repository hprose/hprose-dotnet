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
 * Int32Serializer.cs                                     *
 *                                                        *
 * Int32Serializer class for C#.                          *
 *                                                        *
 * LastModified: Mar 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class Int32Serializer : Serializer<int> {
        private static readonly Int32Serializer _instance = new Int32Serializer();
        public static Int32Serializer Instance => _instance;
        public override void Write(Writer writer, int obj) => ValueWriter.Write(writer.Stream, obj);
    }
}
