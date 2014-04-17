/**********************************************************\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: http://www.hprose.com/                 |
|                   http://www.hprose.net/                 |
|                   http://www.hprose.org/                 |
|                                                          |
\**********************************************************/
/**********************************************************\
 *                                                        *
 * IInvocationHandler.cs                                  *
 *                                                        *
 * IInvocationHandler interface for C#.                   *
 *                                                        *
 * LastModified: Apr 17, 2014                             *
 * Authors: Ma Bingyao <andot@hprose.com>                 *
 *                                                        *
\**********************************************************/

using System.Reflection;

namespace Hprose.Reflection {
    public interface IInvocationHandler {
        object Invoke(object proxy, MethodInfo method, object[] args);
    }
}
