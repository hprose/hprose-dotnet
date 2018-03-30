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
 * LastModified: Mar 30, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

namespace Hprose.IO.Serializers {
    class Int64Serializer : Serializer<long> {
        public override void Write(Writer writer, long obj) => ValueWriter.Write(writer.Stream, obj);
    }
}