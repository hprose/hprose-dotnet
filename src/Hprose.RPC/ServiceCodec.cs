/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ServiceCodec.cs                                         |
|                                                          |
|  ServiceCodec class for C#.                              |
|                                                          |
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class ServiceCodec : IServiceCodec {
        private static readonly object[] emptyArgs = { };
        public static ServiceCodec Instance { get; } = new ServiceCodec();
        public bool Debug { get; set; } = false;
        public bool Simple { get; set; } = false;
        public Mode Mode { get; set; } = Mode.MemberMode;
        public LongType LongType { get; set; } = LongType.Int64;
        public RealType RealType { get; set; } = RealType.Double;
        public CharType CharType { get; set; } = CharType.String;
        public ListType ListType { get; set; } = ListType.List;
        public DictType DictType { get; set; } = DictType.NullableKeyDictionary;
        public Stream Encode(object result, ServiceContext context) {
            var stream = new MemoryStream();
            var writer = new Writer(stream, Simple, Mode);
            if (Simple) {
                context.RequestHeaders["simple"] = true;
            }
            if (context.ResponseHeaders.Count > 0) {
                stream.WriteByte(Tags.TagHeader);
                writer.Serialize(context.ResponseHeaders);
                writer.Reset();
            }
            if (result is Exception) {
                stream.WriteByte(Tags.TagError);
                var error = result as Exception;
                writer.Serialize(Debug ? error.ToString() : error.Message);
            }
            else {
                stream.WriteByte(Tags.TagResult);
                writer.Serialize(result);
            }
            stream.WriteByte(Tags.TagEnd);
            stream.Position = 0;
            return stream;
        }
        private static Method DecodeMethod(string fullname, int paramCount, ServiceContext context) {
            var service = context.Service;
            var method = service.Get(fullname, paramCount);
            context.Method = method ?? throw new Exception("Can't find this method " + fullname + "().");
            return method;
        }
        private object[] DecodeArguments(string fullname, Reader reader, ServiceContext context) {
            var stream = reader.Stream;
            var tag = stream.ReadByte();
            object[] args = emptyArgs;
            var count = 0;
            if (tag == Tags.TagList) {
                reader.Reset();
                count = ValueReader.ReadCount(stream);
                reader.AddReference(null);
            }
            var method = DecodeMethod(fullname, count, context);
            if (method.Missing) {
                args = new object[count];
                for (int i = 0; i < count; i++) {
                    args[i] = reader.Deserialize();
                }
                return args;
            }
            var parameters = method.Parameters;
            var n = parameters.Length;
            args = new object[n];
            var autoParams = 0;
            for (int i = 0; i < n; ++i) {
                if (typeof(Context).IsAssignableFrom(parameters[i].ParameterType)) {
                    autoParams = 1;
                    args[i] = context;
                }
                else if (i - autoParams < count) {
                    args[i] = reader.Deserialize(parameters[i].ParameterType);
                }
                else {
                    args[i] = parameters[i].DefaultValue;
                }
            }
            return args;
        }
        public async Task<(string, object[])> Decode(Stream request, ServiceContext context) {
            var stream = await request.ToMemoryStream().ConfigureAwait(false);
            if (stream.Length == 0) {
                DecodeMethod("~", 0, context);
                return ("~", emptyArgs);
            }
            var reader = new Reader(stream, false, Mode) {
                LongType = LongType,
                RealType = RealType,
                CharType = CharType,
                ListType = ListType,
                DictType = DictType
            };
            var tag = stream.ReadByte();
            var requestHeaders = context.RequestHeaders;
            if (tag == Tags.TagHeader) {
                var headers = reader.Deserialize<Dictionary<string, object>>();
                foreach (var pair in headers) {
                    requestHeaders[pair.Key] = pair.Value;
                }
                reader.Reset();
                tag = stream.ReadByte();
            }
            switch (tag) {
                case Tags.TagCall:
                    if (requestHeaders.ContainsKey("simple") && (bool)requestHeaders["simple"]) {
                        reader.Simple = true;
                    }
                    var fullname = reader.Deserialize<string>();
                    var args = DecodeArguments(fullname, reader, context);
                    return (fullname, args);
                case Tags.TagEnd:
                    DecodeMethod("~", 0, context);
                    return ("~", emptyArgs);
                default:
                    var data = stream.GetArraySegment();
                    throw new Exception("Invalid request:\r\n" + Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
        }
    }
}