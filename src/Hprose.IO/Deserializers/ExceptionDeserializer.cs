/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ExceptionDeserializer.cs                                |
|                                                          |
|  ExceptionDeserializer class for C#.                     |
|                                                          |
|  LastModified: Jun 30, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Deserializers {
    using System.Reflection;
    using static Tags;

    internal class ExceptionDeserializer<T> : Deserializer<T> where T : Exception {
        public override T Read(Reader reader, int tag) => tag switch {
#if !NET35_CF
            TagError => (T)Activator.CreateInstance(typeof(T), new object[] { reader.Deserialize<string>() }),
#else
            TagError => (T)typeof(T).GetConstructor(new Type[] { typeof(string) }).Invoke(new object[] { reader.Deserialize<string>() }),
#endif

            _ => base.Read(reader, tag),
        };
    }
}
