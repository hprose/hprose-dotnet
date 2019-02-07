/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  IntPtrSerializer.cs                                     |
|                                                          |
|  IntPtrSerializer class for C#.                          |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Serializers {
    internal class IntPtrSerializer : Serializer<IntPtr> {
        public override void Write(Writer writer, IntPtr obj) => ValueWriter.Write(writer.Stream, obj);
    }
}