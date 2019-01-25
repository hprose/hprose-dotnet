/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  LongTypeAttribute.cs                                    |
|                                                          |
|  LongType Attribute for C#.                              |
|                                                          |
|  LastModified: Jan 25, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
using System;

namespace Hprose.RPC {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class LongTypeAttribute : Attribute {
        public LongTypeAttribute(LongType value) => Value = value;
        public LongType Value { get; set; }
    }
}
