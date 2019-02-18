/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Settings.cs                                             |
|                                                          |
|  Settings class for C#.                                  |
|                                                          |
|  LastModified: Feb 18, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;

namespace Hprose.RPC {
    public class Settings {
        public Type Type { get; set; } = null;
        public IDictionary<string, object> RequestHeaders { get; set; } = null;
        public IDictionary<string, object> Context { get; set; } = null;
    }
}
