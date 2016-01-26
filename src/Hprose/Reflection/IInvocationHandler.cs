/**********************************************************\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: http://www.hprose.com/                 |
|                   http://www.hprose.org/                 |
|                                                          |
\**********************************************************/
/**********************************************************\
 *                                                        *
 * IInvocationHandler.cs                                  *
 *                                                        *
 * IInvocationHandler interface for C#.                   *
 *                                                        *
 * LastModified: Jan 23, 2016                             *
 * Authors: Ma Bingyao <andot@hprose.com>                 *
 *                                                        *
\**********************************************************/

using System;
using System.Reflection;

namespace Hprose.Reflection {
    public interface IInvocationHandler {
#if dotNETMF
        object Invoke(object proxy, string methodName, Type[] paramTypes, Type returnType, object[] attrs, object[] args);
#else 
        object Invoke(object proxy, MethodInfo method, object[] args);
#endif
    }
}
