/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Oneway.cs                                               |
|                                                          |
|  Oneway plugin for C#.                                   |
|                                                          |
|  LastModified: Mar 26, 2020                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.IO;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Oneway {
    public static class Oneway {
        public static async Task<Stream> Handler(Stream request, Context context, NextIOHandler next) {
            var result = next(request, context);
            if (context.Contains("Oneway")) {
                return null;
            }
            return await result.ConfigureAwait(false);
        }
    }
}
