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
 * LastModified: Mar 29, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;

namespace Hprose.IO.Serializers {
    class TimeSpanSerializer : Serializer<TimeSpan> {
        private static readonly TimeSpanSerializer _instance = new TimeSpanSerializer();
        public static TimeSpanSerializer Instance => _instance;
        public override void Write(Writer writer, TimeSpan obj) => ValueWriter.Write(writer.Stream, obj.Ticks);
    }
}
