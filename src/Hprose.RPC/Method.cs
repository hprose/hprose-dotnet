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
|  LastModified: Jan 26, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Reflection;

namespace Hprose.RPC {
    public class Method {
        public bool Missing { get; set; } = false;
        public string Fullname { get; private set; }
        public MethodInfo MethodInfo { get; private set; }
        public ParameterInfo[] Parameters { get; private set; }
        public object Target { get; private set; }
        public Method(MethodInfo methodInfo, string fullname, object target = null) {
            MethodInfo = methodInfo;
            Fullname = fullname ?? methodInfo.Name;
            Target = target;
            Parameters = methodInfo.GetParameters();
        }
        public Method(MethodInfo methodInfo, object target = null) : this(methodInfo, methodInfo.Name, target) { }
    }
}