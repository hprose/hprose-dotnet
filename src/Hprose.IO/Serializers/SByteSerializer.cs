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
 * LastModified: Mar 30, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class SByteSerializer : Serializer<sbyte> {
        public override void Write(Writer writer, sbyte obj) => ValueWriter.Write(writer.Stream, obj);
    }
}