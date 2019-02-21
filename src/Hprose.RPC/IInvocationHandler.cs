/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  IInvocationHandler.cs                                   |
|                                                          |
|  IInvocationHandler interface for C#.                    |
|                                                          |
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/
#if !NET35_CF
using System.Reflection;

namespace Hprose.RPC {
    public interface IInvocationHandler {
        object Invoke(object proxy, MethodInfo method, object[] args);
    }
}
#endif