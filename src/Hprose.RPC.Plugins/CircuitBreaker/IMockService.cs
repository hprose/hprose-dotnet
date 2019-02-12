/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  IMockService.cs                                         |
|                                                          |
|  IMockService interface for C#.                          |
|                                                          |
|  LastModified: Feb 1, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.CircuitBreaker {
    public interface IMockService {
        Task<object> Invoke(string name, object[] args, Context context);
    }
}
