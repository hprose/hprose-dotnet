/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  UIntPtrSerializer.cs                                    |
|                                                          |
|  UIntPtrSerializer class for C#.                         |
|                                                          |
|  LastModified: Apr 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Serializers {
    class UIntPtrSerializer : Serializer<UIntPtr> {
        public override void Write(Writer writer, UIntPtr obj) => ValueWriter.Write(writer.Stream, obj);
    }
}