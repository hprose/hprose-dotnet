/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  MethodNameAttribute.cs                                  |
|                                                          |
|  MethodName Attribute for C#.                            |
|                                                          |
|  LastModified: May 7, 2018                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.RPC.Common {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MethodNameAttribute : Attribute {
        public MethodNameAttribute(string value) => Value = value;
        public string Value { get; set; }
    }
}
