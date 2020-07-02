/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  UIntPtrDeserializer.cs                                  |
|                                                          |
|  UIntPtrDeserializer class for C#.                       |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Deserializers {
    internal class UIntPtrDeserializer : Deserializer<UIntPtr> {
        public override UIntPtr Read(Reader reader, int tag) => UIntPtr.Size == 4
                ? (UIntPtr)Deserializer<uint>.Instance.Read(reader, tag)
                : (UIntPtr)Deserializer<ulong>.Instance.Read(reader, tag);
    }
}
