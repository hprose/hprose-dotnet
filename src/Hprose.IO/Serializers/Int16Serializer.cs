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
 * Int16Serializer.cs                                     *
 *                                                        *
 * Int16Serializer class for C#.                          *
 *                                                        *
 * LastModified: Mar 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class Int16Serializer : Serializer<short> {
        private static readonly Int16Serializer _instance = new Int16Serializer();
        public static Int16Serializer Instance => _instance;
        public override void Write(Writer writer, short obj) => ValueWriter.Write(writer.Stream, obj);
    }
}
