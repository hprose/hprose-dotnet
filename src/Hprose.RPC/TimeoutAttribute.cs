/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  TimeoutAttribute.cs                                     |
|                                                          |
|  Timeout Attribute for C#.                               |
|                                                          |
|  LastModified: Feb 27, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.RPC {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TimeoutAttribute : Attribute {
        public TimeoutAttribute(TimeSpan value) => Value = value;
        public TimeSpan Value { get; set; }
    }
}
