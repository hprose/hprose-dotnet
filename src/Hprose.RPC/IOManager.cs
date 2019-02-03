/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  IOManager.cs                                            |
|                                                          |
|  IOManager class for C#.                                 |
|                                                          |
|  LastModified: Feb 4, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Hprose.RPC {
    public delegate Task<Stream> NextIOHandler(Stream request, Context context);
    public delegate Task<Stream> IOHandler(Stream request, Context context, NextIOHandler next);
    public class IOManager : HandlerManager<IOHandler, NextIOHandler> {
        public IOManager(NextIOHandler handler) : base(handler) { }
        protected override NextIOHandler GetNextHandler(IOHandler handler, NextIOHandler next) {
            return (request, context) => handler(request, context, next);
        }
    }
}
