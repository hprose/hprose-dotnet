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
|  LastModified: Apr 13, 2018                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Deserializers {
    class UIntPtrDeserializer : Deserializer<UIntPtr> {
        public override UIntPtr Read(Reader reader, int tag) {
            if (UIntPtr.Size == 4) {
                return (UIntPtr)Deserializer<uint>.Instance.Read(reader, tag);
            }
            else {
                return (UIntPtr)Deserializer<ulong>.Instance.Read(reader, tag);
            }
        }
    }
}
