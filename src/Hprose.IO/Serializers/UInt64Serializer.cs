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
 * UInt64Serializer.cs                                    *
 *                                                        *
 * UInt64Serializer class for C#.                         *
 *                                                        *
 * LastModified: Mar 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class UInt64Serializer : Serializer<ulong> {
        private static readonly UInt64Serializer _instance = new UInt64Serializer();
        public static UInt64Serializer Instance => _instance;
        public override void Write(Writer writer, ulong obj) => ValueWriter.Write(writer.Stream, obj);
    }
}
