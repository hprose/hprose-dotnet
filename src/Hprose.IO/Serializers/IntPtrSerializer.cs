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
 * IntPtrSerializer.cs                                    *
 *                                                        *
 * IntPtrSerializer class for C#.                         *
 *                                                        *
 * LastModified: Apr 7, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;

namespace Hprose.IO.Serializers {
    class IntPtrSerializer : Serializer<IntPtr> {
        public override void Serialize(Writer writer, IntPtr obj) => ValueWriter.Write(writer.Stream, obj);
    }
}