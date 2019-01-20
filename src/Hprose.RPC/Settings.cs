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
        public bool? Simple { get; set; } = null;
        public Mode? Mode { get; set; } = null;
        public LongType? LongType { get; set; } = null;
        public RealType? RealType { get; set; } = null;
        public CharType? CharType { get; set; } = null;
        public ListType? ListType { get; set; } = null;
        public DictType? DictType { get; set; } = null;
        public Type Type { get; set; } = null;
        public ExpandoObject RequestHeaders { get; set; } = null;
        public IDictionary<string, object> Context { get; set; } = null;
    }
}
