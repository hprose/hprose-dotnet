/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  RealTypeAttribute.cs                                    |
|                                                          |
|  RealType Attribute for C#.                              |
|                                                          |
|  LastModified: Jan 25, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
using System;

namespace Hprose.RPC {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RealTypeAttribute : Attribute {
        public RealTypeAttribute(RealType value) => Value = value;
        public RealType Value { get; set; }
    }
}
