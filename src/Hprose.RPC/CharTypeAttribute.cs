/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  CharTypeAttribute.cs                                    |
|                                                          |
|  CharType Attribute for C#.                              |
|                                                          |
|  LastModified: Jan 25, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
using System;

namespace Hprose.RPC {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CharTypeAttribute : Attribute {
        public CharTypeAttribute(CharType value) => Value = value;
        public CharType Value { get; set; }
    }
}
