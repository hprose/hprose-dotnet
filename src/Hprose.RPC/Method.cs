/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  Method.cs                                               |
|                                                          |
|  Method class for C#.                                    |
|                                                          |
|  LastModified: Mar 7, 2020                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hprose.RPC {
    public class Method {
        public bool Missing { get; set; } = false;
        public string Fullname { get; private set; }
        public MethodInfo MethodInfo { get; private set; }
        public ParameterInfo[] Parameters { get; private set; }
        public object Target { get; private set; }
        public IDictionary<string, object> Options { get; private set; } = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        public object this[string name] {
            get => Options[name];
            set => Options[name] = value;
        }
        public Method(MethodInfo methodInfo, string fullname, object target = null) {
            MethodInfo = methodInfo;
            Fullname = fullname ?? methodInfo.Name;
            Target = target;
            Parameters = methodInfo.GetParameters();
        }
        public Method(MethodInfo methodInfo, object target = null) : this(methodInfo, methodInfo.Name, target) { }
    }
}