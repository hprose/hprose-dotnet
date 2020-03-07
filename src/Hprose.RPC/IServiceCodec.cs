/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  IServiceCodec.cs                                        |
|                                                          |
|  IServiceCodec interface for C#.                         |
|                                                          |
|  LastModified: Jan 27, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.IO;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public interface IServiceCodec {
        MemoryStream Encode(object result, ServiceContext context);
        (string, object[]) Decode(MemoryStream request, ServiceContext context);
    }
}