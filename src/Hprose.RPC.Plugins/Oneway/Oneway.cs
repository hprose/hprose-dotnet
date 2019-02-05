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
|  LastModified: Feb 2, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.Oneway {
    public class Oneway {
        public static async Task<object> Handler(string name, object[] args, Context context, NextInvokeHandler next) {
            var result = next(name, args, context);
            if (context.Contains("Oneway")) {
                return null;
            }
            return await result;
        }
    }
}
