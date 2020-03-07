/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
| IClientCodec.cs                                          |
|                                                          |
| IClientCodec interface for C#.                           |
|                                                          |
| LastModified: Jan 19, 2019                               |
| Author: Ma Bingyao <andot@hprose.com>                    |
|                                                          |
\*________________________________________________________*/

using System.IO;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public interface IClientCodec {
        MemoryStream Encode(string name, object[] args, ClientContext context);
        object Decode(MemoryStream response, ClientContext context);
    }
}