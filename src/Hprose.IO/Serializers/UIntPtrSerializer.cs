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
 * UIntPtrSerializer.cs                                   *
 *                                                        *
 * UIntPtrSerializer class for C#.                        *
 *                                                        *
 * LastModified: Apr 1, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;

namespace Hprose.IO.Serializers {
    class UIntPtrSerializer : Serializer<UIntPtr> {
        public override void Write(Writer writer, UIntPtr obj) => ValueWriter.Write(writer.Stream, obj);
    }
}