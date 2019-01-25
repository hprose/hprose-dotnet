/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ModeAttribute.cs                                        |
|                                                          |
|  Mode Attribute for C#.                                  |
|                                                          |
|  LastModified: Jan 25, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
using System;

namespace Hprose.RPC {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ModeAttribute : Attribute {
        public ModeAttribute(Mode value) => Value = value;
        public Mode Value { get; set; }
    }
}
