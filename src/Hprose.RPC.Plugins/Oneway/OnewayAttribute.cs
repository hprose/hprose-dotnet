/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  OnewayAttribute.cs                                      |
|                                                          |
|  Oneway Attribute for C#.                                |
|                                                          |
|  LastModified: Feb 2, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;

namespace Hprose.RPC.Plugins.Oneway {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class OnewayAttribute : ContextAttribute {
        public OnewayAttribute() : base("Oneway", true) { }
    }
}