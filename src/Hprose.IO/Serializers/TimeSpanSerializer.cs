/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  TimeSpanSerializer.cs                                   |
|                                                          |
|  TimeSpanSerializer class for C#.                        |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Serializers {
    class TimeSpanSerializer : Serializer<TimeSpan> {
        public override void Write(Writer writer, TimeSpan obj) => ValueWriter.Write(writer.Stream, obj.Ticks);
    }
}