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
|  LastModified: Mar 28, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hprose.RPC.Codec.JSONRPC {
    public class JsonRpcServiceCodec : IServiceCodec {
        public static JsonRpcServiceCodec Instance { get; } = new JsonRpcServiceCodec();
        public MemoryStream Encode(object result, ServiceContext context) {
            if (!context.Contains("jsonrpc") || !(bool)context["jsonrpc"]) {
                return ServiceCodec.Instance.Encode(result, context);
            }
            var response = new Dictionary<string, object> {
                { "jsonrpc", "2.0" }
            };
            if (context.Contains("jsonrpc.id")) {
                response["id"] = context["jsonrpc.id"];
            }
            if (context.ResponseHeaders.Count > 0) {
                response["headers"] = context.ResponseHeaders;
            }
            if (result is Exception) {
                var error = result as Exception;
                switch (error.Message) {
                    case "Parse error":
                        response["error"] = new Dictionary<string, object> {
                            { "code", -32700 },
                            { "message", "Parse error" }
                        };
                        break;
                    case "Invalid Request":
                        response["error"] = new Dictionary<string, object> {
                            { "code", -32600 },
                            { "message", "Invalid Request" }
                        };
                        break;
                    case "Method not found":
                        response["error"] = new Dictionary<string, object> {
                            { "code", -32601 },
                            { "message", "Method not found" }
                        };
                        break;
                    case "Invalid params":
                        response["error"] = new Dictionary<string, object> {
                            { "code", -32602 },
                            { "message", "Invalid params" }
                        };
                        break;
                    default:
                        response["error"] = new Dictionary<string, object> {
                            { "code", 0 },
                            { "message",  error.Message },
                            { "data", error.StackTrace }
                        };
                        break;
                }
            }
            else {
                response["result"] = result;
            }
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            return new MemoryStream(data, 0, data.Length, false, true);
        }
        public (string, object[]) Decode(MemoryStream request, ServiceContext context) {
            var tag = request.ReadByte();
            request.Position = 0;
            context["jsonrpc"] = (tag == '{');
            if (!(bool)context["jsonrpc"]) {
                return ServiceCodec.Instance.Decode(request, context);
            }
            JObject call = null;
            try {
                var data = request.GetArraySegment();
                call = JsonConvert.DeserializeObject<JObject>(Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
            catch {
                throw new Exception("Parse error");
            }
            if (call == null) {
                throw new Exception("Parse error");
            }
            if (call.TryGetValue("jsonrpc", out var jsonrpc)) {
                if ((string)jsonrpc != "2.0") {
                    throw new Exception("Invalid Request");
                }
            }
            if (!call.TryGetValue("method", out var methodName)) {
                throw new Exception("Invalid Request");
            }
            if (!call.TryGetValue("id", out var id)) {
                throw new Exception("Invalid Request");
            }
            context["jsonrpc.id"] = id;
            if ((call as IDictionary<string, JToken>).ContainsKey("headers")) {
                var requestHeaders = context.RequestHeaders;
                var headers = call["headers"].ToObject<IDictionary<string, object>>();
                foreach (var pair in headers) {
                    requestHeaders[pair.Key] = pair.Value;
                }
            }
            var name = methodName.ToString();
            var args = new JArray();
            if ((call as IDictionary<string, JToken>).ContainsKey("params")) {
                args = call["params"] as JArray;
            }
            var method = context.Service.Get(name, args.Count);
            context.Method = method ?? throw new Exception("Method not found");
            if (method.Missing) {
                return (name, args.ToObject<object[]>());
            }
            var count = args.Count;
            var parameters = method.Parameters;

            var n = parameters.Length;
            if (method.PassContext) {
                n--;
            }
            var arguments = new object[n];
            try {
                for (int i = 0; i < n; ++i) {
                    if (i < count) {
                        arguments[i] = args[i].ToObject(parameters[i].ParameterType);
                    }
                    else {
                        arguments[i] = parameters[i].DefaultValue;
                    }
                }
            }
            catch {
                throw new Exception("Invalid params");
            }
            return (name, arguments);
        }
    }
}
