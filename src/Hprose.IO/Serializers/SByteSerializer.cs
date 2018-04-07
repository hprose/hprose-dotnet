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
 * SByteSerializer.cs                                     *
 *                                                        *
 * SByteSerializer class for C#.                          *
 *                                                        *
 * LastModified: Apr 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class SByteSerializer : Serializer<sbyte> {
        public override void Serialize(Writer writer, sbyte obj) => ValueWriter.Write(writer.Stream, obj);
    }
}