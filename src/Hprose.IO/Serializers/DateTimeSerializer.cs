/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  DateTimeSerializer.cs                                   |
|                                                          |
|  DateTimeSerializer class for C#.                        |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Serializers {
    internal class DateTimeSerializer : ReferenceSerializer<DateTime> {
        public override void Write(Writer writer, DateTime obj) {
            base.Write(writer, obj);
            ValueWriter.Write(writer.Stream, obj);
        }
    }
}
