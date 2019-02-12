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
|  LastModified: Jan 20, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System.Reflection;

namespace Hprose.RPC {
    public interface IInvocationHandler {
        object Invoke(object proxy, MethodInfo method, object[] args);
    }
}
