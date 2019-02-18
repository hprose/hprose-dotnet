/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ClientCodec.cs                                          |
|                                                          |
|  ClientCodec class for C#.                               |
|                                                          |
|  LastModified: Feb 18, 2019                              |
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
    public class ClientCodec : IClientCodec {
        public static ClientCodec Instance { get; } = new ClientCodec();
        public bool Simple { get; set; } = false;
        public Mode Mode { get; set; } = Mode.MemberMode;
        public LongType LongType { get; set; } = LongType.Int64;
        public RealType RealType { get; set; } = RealType.Double;
        public CharType CharType { get; set; } = CharType.String;
        public ListType ListType { get; set; } = ListType.List;
        public DictType DictType { get; set; } = DictType.NullableKeyDictionary;
        public Stream Encode(string name, object[] args, ClientContext context) {
            var stream = new MemoryStream();
            var writer = new Writer(stream, Simple, Mode);
            if (Simple) {
                context.RequestHeaders["simple"] = true;
            }
            if (context.RequestHeaders.Count > 0) {
                stream.WriteByte(Tags.TagHeader);
                writer.Serialize(context.RequestHeaders);
                writer.Reset();
            }
            stream.WriteByte(Tags.TagCall);
            writer.Serialize(name);
            if (args != null && args.Length > 0) {
                writer.Reset();
                writer.Serialize(args);
            }
            stream.WriteByte(Tags.TagEnd);
            stream.Position = 0;
            return stream;
        }
        public async Task<object> Decode(Stream response, ClientContext context) {
            var stream = await response.ToMemoryStream().ConfigureAwait(false);
            var reader = new Reader(stream, false, Mode) {
                LongType = LongType,
                RealType = RealType,
                CharType = CharType,
                ListType = ListType,
                DictType = DictType
            };
            var tag = stream.ReadByte();
            var responseHeaders = context.ResponseHeaders;
            if (tag == Tags.TagHeader) {
                var headers = reader.Deserialize<Dictionary<string, object>>();
                foreach (var pair in headers) {
                    responseHeaders[pair.Key] = pair.Value;
                }
                reader.Reset();
                tag = stream.ReadByte();
            }
            switch (tag) {
                case Tags.TagResult:
                    if (responseHeaders.ContainsKey("simple") && (bool)responseHeaders["simple"]) {
                        reader.Simple = true;
                    }
                    return reader.Deserialize(context.Type);
                case Tags.TagError:
                    throw new Exception(reader.Deserialize<string>());
                case Tags.TagEnd:
                    return null;
                default:
                    var data = stream.GetArraySegment();
                    throw new Exception("Invalid response\r\n" + Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
        }
    }
}
