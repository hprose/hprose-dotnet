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
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Deserializers {
    internal class IntPtrDeserializer : Deserializer<IntPtr> {
        public override IntPtr Read(Reader reader, int tag) => IntPtr.Size == 4
                ? (IntPtr)Deserializer<int>.Instance.Read(reader, tag)
                : (IntPtr)Deserializer<long>.Instance.Read(reader, tag);
    }
}
