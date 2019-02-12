/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  IntPtrDeserializer.cs                                   |
|                                                          |
|  IntPtrDeserializer class for C#.                        |
|                                                          |
|  LastModified: Apr 13, 2018                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Deserializers {
    internal class IntPtrDeserializer : Deserializer<IntPtr> {
        public override IntPtr Read(Reader reader, int tag) {
            if (IntPtr.Size == 4) {
                return (IntPtr)Deserializer<int>.Instance.Read(reader, tag);
            }
            else {
                return (IntPtr)Deserializer<long>.Instance.Read(reader, tag);
            }
        }
    }
}
