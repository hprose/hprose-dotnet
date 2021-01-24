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
|  LastModified: Jan 24, 2021                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Hprose.RPC {
    public class Method {
        public bool Missing { get; set; } = false;
        public bool PassContext { get; private set; } = false;
        public string Name { get; private set; }
        public MethodInfo MethodInfo { get; private set; }
        public ParameterInfo[] Parameters { get; private set; }
        public object Target { get; private set; }
        public IDictionary<string, object> Options { get; private set; } = new ConcurrentDictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        public object this[string name] {
            get => Options[name];
            set => Options[name] = value;
        }
        public Method(MethodInfo methodInfo, string name, object target = null) {
            MethodInfo = methodInfo;
            Name = name ?? methodInfo.Name;
            Target = target;
            Parameters = methodInfo.GetParameters();
            if (Parameters.Length > 0) {
                PassContext = typeof(Context).IsAssignableFrom(Parameters[Parameters.Length - 1].ParameterType);
            }
        }
        public Method(MethodInfo methodInfo, object target = null) : this(methodInfo, methodInfo.Name, target) { }
    }
}