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
|  LastModified: Jan 30, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
                ((dynamic)context.RequestHeaders).Simple = true;
            }
            if ((context.RequestHeaders as IDictionary<string, object>).Count > 0) {
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
            MemoryStream stream;
            if (response is MemoryStream) {
                stream = (MemoryStream)response;
            }
            else {
                stream = new MemoryStream();
                await response.CopyToAsync(stream);
            }
            stream.Position = 0;
            var reader = new Reader(stream, false, Mode) {
                LongType = LongType,
                RealType = RealType,
                CharType = CharType,
                ListType = ListType,
                DictType = DictType
            };
            var tag = stream.ReadByte();
            if (tag == Tags.TagHeader) {
                var headers = reader.Deserialize<ExpandoObject>();
                var responseHeaders = context.ResponseHeaders as IDictionary<string, object>;
                foreach (var pair in headers) {
                    responseHeaders[pair.Key] = pair.Value;
                }
                reader.Reset();
                tag = stream.ReadByte();
            }
            switch (tag) {
                case Tags.TagResult:
                    if (((IDictionary<string, object>)context.ResponseHeaders).ContainsKey("Simple")
                        && ((dynamic)context.ResponseHeaders).Simple) {
                        reader.Simple = true;
                    }
                    return reader.Deserialize(context.Type);
                case Tags.TagError:
                    throw new Exception(reader.Deserialize<string>());
                case Tags.TagEnd:
                    return null;
                default:
                    throw new Exception("Invalid response\r\n" + Encoding.UTF8.GetString(stream.ToArray()));
            }
        }
    }
}
