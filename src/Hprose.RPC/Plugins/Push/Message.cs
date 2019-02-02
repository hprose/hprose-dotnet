/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Message.cs                                              |
|                                                          |
|  Message class for C#.                                   |
|                                                          |
|  LastModified: Feb 2, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;

namespace Hprose.RPC.Plugins.Push {
    public struct Message {
        public object Data;
        public string From;
    }
}