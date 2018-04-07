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
 * LastModified: Apr 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class UInt32Serializer : Serializer<uint> {
        public override void Write(Writer writer, uint obj) => ValueWriter.Write(writer.Stream, obj);
    }
}