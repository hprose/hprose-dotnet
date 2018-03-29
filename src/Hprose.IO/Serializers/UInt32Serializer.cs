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
 * UInt32Serializer.cs                                    *
 *                                                        *
 * UInt32Serializer class for C#.                         *
 *                                                        *
 * LastModified: Mar 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class UInt32Serializer : Serializer<uint> {
        private static readonly UInt32Serializer _instance = new UInt32Serializer();
        public static UInt32Serializer Instance => _instance;
        public override void Write(Writer writer, uint obj) => ValueWriter.Write(writer.Stream, obj);
    }
}
