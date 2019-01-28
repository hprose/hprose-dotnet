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
|  LastModified: Jan 19, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Hprose.RPC {
    public class Settings {
        public Type Type { get; set; } = null;
        public ExpandoObject RequestHeaders { get; set; } = null;
        public IDictionary<string, object> Context { get; set; } = null;
    }
}
