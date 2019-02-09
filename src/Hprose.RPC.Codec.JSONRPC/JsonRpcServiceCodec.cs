/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  JsonRpcServiceCodec.cs                                  |
|                                                          |
|  JsonRpcServiceCodec class for C#.                       |
|                                                          |
|  LastModified: Feb 5, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Hprose.RPC.Codec.JSONRPC {
    public class JsonRpcServiceCodec : IServiceCodec {
        public static JsonRpcServiceCodec Instance { get; } = new JsonRpcServiceCodec();
        public Stream Encode(object result, ServiceContext context) {
            JObject response = new JObject {
                { "jsonrpc", "2.0" },
                { "id", context["jsonrpc.id"] }
            };
            if (result is Exception) {
                var error = result as Exception;
                switch (error.Message) {
                    case "Parse error":
                        response["error"] = new JObject {
                            { "code", -32700 },
                            { "message", "Parse error" }
                        };
                        break;
                    case "Invalid Request":
                        response["error"] = new JObject {
                            { "code", -32600 },
                            { "message", "Invalid Request" }
                        };
                        break;
                    case "Method not found":
                        response["error"] = new JObject {
                            { "code", -32601 },
                            { "message", "Method not found" }
                        };
                        break;
                    case "Invalid params":
                        response["error"] = new JObject {
                            { "code", -32602 },
                            { "message", "Invalid params" }
                        };
                        break;
                    default:
                        response["error"] = new JObject {
                            { "code", 0 },
                            { "message",  error.Message },
                            { "data", error.StackTrace }
                        };
                        break;
                }
            }
            else {
                response["result"] = new JValue(result);
            }
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            return new MemoryStream(data, 0, data.Length, false, true);
        }
        public async Task<(string, object[])> Decode(Stream request, ServiceContext context) {
            MemoryStream stream = await request.ToMemoryStream().ConfigureAwait(false);
            JObject call = null;
            try {
                var data = stream.GetArraySegment();
                call = JsonConvert.DeserializeObject<JObject>(Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
            catch {
                throw new Exception("Parse error");
            }
            if (call == null) {
                throw new Exception("Parse error");
            }
            if (call.TryGetValue("jsonrpc", out var jsonrpc)) {
                if (jsonrpc.ToObject<string>() != "2.0") {
                    throw new Exception("Invalid Request");
                }
            }
            if (!call.TryGetValue("method", out var name)) {
                throw new Exception("Invalid Request");
            }
            if (!call.TryGetValue("id", out var id)) {
                throw new Exception("Invalid Request");
            }
            context["jsonrpc.id"] = id;
            var fullname = name.ToObject<string>();
            var args = new JArray();
            if (call.ContainsKey("params")) {
                args = call["params"] as JArray;
            }
            var method = context.Service.Get(fullname, args.Count);
            context.Method = method ?? throw new Exception("Method not found");
            if (method.Missing) {
                return (fullname, args.ToObject<object[]>());
            }
            var count = args.Count;
            var parameters = method.Parameters;
            var n = parameters.Length;
            var arguments = new object[n];
            var autoParams = 0;
            try {
                for (int i = 0; i < n; ++i) {
                    var paramType = parameters[i].ParameterType;
                    if (typeof(Context).IsAssignableFrom(paramType)) {
                        autoParams = 1;
                        arguments[i] = context;
                    }
                    else if (i - autoParams < count) {
                        arguments[i] = args[i - autoParams].ToObject(paramType);
                    }
                    else {
                        arguments[i] = parameters[i].RawDefaultValue;
                    }
                }
            }
            catch {
                throw new Exception("Invalid params");
            }
            return (fullname, arguments);
        }
    }
}
