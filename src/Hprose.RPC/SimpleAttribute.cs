/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  SimpleAttribute.cs                                      |
|                                                          |
|  Simple Attribute for C#.                                |
|                                                          |
|  LastModified: Jan 25, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.RPC {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SimpleAttribute : Attribute {
        public SimpleAttribute(bool value) => Value = value;
        public bool Value { get; set; }
    }
}
