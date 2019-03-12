/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  JsonRpcClientCodec.cs                                   |
|                                                          |
|  JsonRpcClientCodec class for C#.                        |
|                                                          |
|  LastModified: Mar 12, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC.Codec.JSONRPC {
    public class JsonRpcClientCodec : IClientCodec {
        public static JsonRpcClientCodec Instance { get; } = new JsonRpcClientCodec();
        private volatile int counter = 0;
        public Stream Encode(string name, object[] args, ClientContext context) {
            var id = Interlocked.Increment(ref counter) & 0x7FFFFFFF;
            var request = new Dictionary<string, object> {
                { "jsonrpc", "2.0" },
                { "id", id },
                { "method", name }
            };
            if (context.RequestHeaders.Count > 0) {
                request.Add("headers", context.RequestHeaders);
            }
            if (args != null && args.Length > 0) {
                request.Add("params", args);
            }
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
            return new MemoryStream(data, 0, data.Length, false, true);
        }
        public async Task<object> Decode(Stream response, ClientContext context) {
            MemoryStream stream = await response.ToMemoryStream().ConfigureAwait(false);
            var data = stream.GetArraySegment();
            var result = JsonConvert.DeserializeObject<JObject>(Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            if ((result as IDictionary<string, JToken>).ContainsKey("headers")) {
                var responseHeaders = context.ResponseHeaders;
                var headers = result["headers"].ToObject<IDictionary<string, object>>();
                foreach (var pair in headers) {
                    responseHeaders[pair.Key] = pair.Value;
                }
            }
            if ((result as IDictionary<string, JToken>).ContainsKey("result")) {
                return result["result"].ToObject(context.ReturnType ?? typeof(object));
            }
            if ((result as IDictionary<string, JToken>).ContainsKey("error")) {
                var error = result["error"] as JObject;
                if ((error as IDictionary<string, JToken>).ContainsKey("code") && error["code"].ToObject<int>() != 0) {
                    throw new Exception(error["code"].ToObject<int>() + ":" + error["message"].ToObject<string>());
                }
                throw new Exception(error["message"].ToObject<string>());
            }
            return null;
        }
    }
}