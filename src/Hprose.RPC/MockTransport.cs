/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
| MockTransport.cs                                         |
|                                                          |
| MockTransport for C#.                                    |
|                                                          |
| LastModified: Feb 27, 2019                               |
| Author: Ma Bingyao <andot@hprose.com>                    |
|                                                          |
\*________________________________________________________*/

using System.IO;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public class MockTransport : ITransport {
        public static string[] Schemes { get; } = new string[] { "mock" };
        public Task<Stream> Transport(Stream request, Context context) {
            return MockAgent.Handler((context as ClientContext).Uri.Host, request);
        }
        public Task Abort() {
#if NET40
            return TaskEx.FromResult<object>(null);
#elif NET45 || NET451 || NET452
            return Task.FromResult<object>(null);
#else
            return Task.CompletedTask;
#endif
        }
    }
}
