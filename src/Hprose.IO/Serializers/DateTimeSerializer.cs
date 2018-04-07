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
 * DateTimeSerializer.cs                                  *
 *                                                        *
 * DateTimeSerializer class for C#.                       *
 *                                                        *
 * LastModified: Apr 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;

namespace Hprose.IO.Serializers {
    class DateTimeSerializer : ReferenceSerializer<DateTime> {
        public override void Write(Writer writer, DateTime obj) {
            base.Write(writer, obj);
            ValueWriter.Write(writer.Stream, obj);
        }
    }
}
