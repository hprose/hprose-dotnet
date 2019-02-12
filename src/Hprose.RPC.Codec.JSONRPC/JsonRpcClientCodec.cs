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
|  LastModified: Feb 6, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC.Codec.JSONRPC {
    public class JsonRpcClientCodec : IClientCodec {
        public static JsonRpcClientCodec Instance { get; } = new JsonRpcClientCodec();
        private volatile int counter = 0;
        public Stream Encode(string name, object[] args, ClientContext context) {
            var id = Interlocked.Increment(ref counter);
            while (id < 0) {
                Interlocked.Add(ref counter, Int32.MinValue);
                id = Interlocked.Increment(ref counter);
            }
            JObject request = new JObject {
                { "jsonrpc", "2.0" },
                { "id", id },
                { "method", name }
            };
            if (args != null && args.Length > 0) {
                request.Add("params", new JArray(args));
            }
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
            return new MemoryStream(data, 0, data.Length, false, true);
        }
        public async Task<object> Decode(Stream response, ClientContext context) {
            MemoryStream stream = await response.ToMemoryStream().ConfigureAwait(false);
            var data = stream.GetArraySegment();
            var result = JsonConvert.DeserializeObject<JObject>(Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            if (result.ContainsKey("result")) {
                return result["result"].ToObject(context.Type ?? typeof(object));
            }
            if (result.ContainsKey("error")) {
                var error = result["error"] as JObject;
                if (error.ContainsKey("code") && error["code"].ToObject<int>() != 0) {
                    throw new Exception(error["code"].ToObject<int>() + ":" + error["message"].ToObject<string>());
                }
                throw new Exception(error["message"].ToObject<string>());
            }
            return null;
        }
    }
}