/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ListTypeAttribute.cs                                    |
|                                                          |
|  ListType Attribute for C#.                              |
|                                                          |
|  LastModified: Jan 25, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
using System;

namespace Hprose.RPC {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ListTypeAttribute : Attribute {
        public ListTypeAttribute(ListType value) => Value = value;
        public ListType Value { get; set; }
    }
}
