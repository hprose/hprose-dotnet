/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
| MockAgent.cs                                             |
|                                                          |
| MockAgent for C#.                                        |
|                                                          |
| LastModified: Feb 27, 2019                               |
| Author: Ma Bingyao <andot@hprose.com>                    |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace Hprose.RPC {
    class MockAgent {
        private static readonly ConcurrentDictionary<string, Func<string, Stream, Task<Stream>>> handlers = new();
        public static void Register(string address, Func<string, Stream, Task<Stream>> handler) {
            handlers[address] = handler;
        }
        public static void Cancel(string address) {
            handlers.TryRemove(address, out var _);
        }
        public static async Task<Stream> Handler(string address, Stream request) {
            if (handlers.TryGetValue(address, out var handler)) {
                return await handler(address, request).ConfigureAwait(false);
            }
            throw new Exception("Server is stopped");
        }
    }
}
