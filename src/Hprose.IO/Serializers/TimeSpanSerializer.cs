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
 * TimeSpanSerializer.cs                                  *
 *                                                        *
 * TimeSpanSerializer class for C#.                       *
 *                                                        *
 * LastModified: Apr 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;

namespace Hprose.IO.Serializers {
    class TimeSpanSerializer : Serializer<TimeSpan> {
        public override void Serialize(Writer writer, TimeSpan obj) => ValueWriter.Write(writer.Stream, obj.Ticks);
    }
}