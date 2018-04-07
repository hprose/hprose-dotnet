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
 * LastModified: Apr 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class UInt16Serializer : Serializer<ushort> {
        public override void Serialize(Writer writer, ushort obj) => ValueWriter.Write(writer.Stream, obj);
    }
}