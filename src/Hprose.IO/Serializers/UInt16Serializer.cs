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
 * UInt16Serializer.cs                                    *
 *                                                        *
 * UInt16Serializer class for C#.                         *
 *                                                        *
 * LastModified: Mar 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class UInt16Serializer : Serializer<ushort> {
        private static readonly UInt16Serializer _instance = new UInt16Serializer();
        public static UInt16Serializer Instance => _instance;
        public override void Write(Writer writer, ushort obj) => ValueWriter.Write(writer.Stream, obj);
    }
}
