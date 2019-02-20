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
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.IO.Deserializers {
    using System.Reflection;
    using static Tags;

    internal class ExceptionDeserializer<T> : Deserializer<T> where T : Exception {
        public override T Read(Reader reader, int tag) {
            var stream = reader.Stream;
            switch (tag) {
                case TagError:
#if !NET35_CF
                    return (T)Activator.CreateInstance(typeof(T), new object[] { reader.Deserialize<string>() });
#else
                    return (T)typeof(T).GetConstructor(new Type[] { typeof(string) }).Invoke(new object[] { reader.Deserialize<string>() });
#endif
                default:
                    return base.Read(reader, tag);
            }
        }
    }
}
