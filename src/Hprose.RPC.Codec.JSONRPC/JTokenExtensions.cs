/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  JTokenExtensions.cs                                     |
|                                                          |
|  JTokenExtensions class for .NET CF                      |
|                                                          |
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

#if NET35_CF
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Hprose.RPC.Codec.JSONRPC {
    static class JTokenExtensions {
        public static T ToObject<T>(this JToken token) => (T)token.ToObject(typeof(T));

        public static object ToObject(this JToken token, Type objectType) => token.ToObject(objectType, new JsonSerializer());

        public static T ToObject<T>(this JToken token, JsonSerializer jsonSerializer) => (T)token.ToObject(typeof(T), jsonSerializer);

        public static object ToObject(this JToken token, Type objectType, JsonSerializer jsonSerializer) {
            using JTokenReader jsonReader = new JTokenReader(token);
            return jsonSerializer.Deserialize(jsonReader, objectType);
        }
    }
}
#endif