/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  DictTypeAttribute.cs                                    |
|                                                          |
|  DictType Attribute for C#.                              |
|                                                          |
|  LastModified: Jan 25, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
using System;

namespace Hprose.RPC {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DictTypeAttribute : Attribute {
        public DictTypeAttribute(DictType value) => Value = value;
        public DictType Value { get; set; }
    }
}
