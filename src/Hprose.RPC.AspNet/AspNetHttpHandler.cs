/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  AspNetHttpHandler.cs                                    |
|                                                          |
|  AspNetHttpHandler class for C#.                         |
|                                                          |
|  LastModified: Feb 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Threading.Tasks;
using System.Web;
using Hprose.RPC;

namespace Hprose.RPC.AspNet {
    public class AspNetHttpHandler : IHandler<HttpContext> {
        public Task Bind(HttpContext server) {
            throw new NotImplementedException();
        }
    }
}
