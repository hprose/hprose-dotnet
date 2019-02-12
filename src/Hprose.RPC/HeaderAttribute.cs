/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  HeaderAttribute.cs                                      |
|                                                          |
|  Header Attribute for C#.                                |
|                                                          |
|  LastModified: Jan 25, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.RPC {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class HeaderAttribute : Attribute {
        public HeaderAttribute(string key, object value) => Value = (key, value);
        public (string, object) Value { get; set; }
    }
}
