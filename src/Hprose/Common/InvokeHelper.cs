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
 * InvokeHelper.cs                                        *
 *                                                        *
 * hprose Invoke Helper class for C#.                     *
 *                                                        *
 * LastModified: Feb 18, 2014                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !(dotNET10 || dotNET11 || dotNETCF10)
using System;

namespace Hprose.Common {
    interface IInvokeHelper {
        void Invoke(HproseInvoker invoker, string functionName, object[] args, Delegate callback, bool byRef);
    }

    class InvokeHelper<T> : IInvokeHelper {
        public void Invoke(HproseInvoker invoker, string functionName, object[] args, Delegate callback, bool byRef) {
            invoker.Invoke<T>(functionName, args, (HproseCallback<T>)callback, byRef);
        }
    }
    interface IInvokeHelper1 {
        void Invoke(HproseInvoker invoker, string functionName, object[] args, Delegate callback);
    }

    class InvokeHelper1<T> : IInvokeHelper1 {
        public void Invoke(HproseInvoker invoker, string functionName, object[] args, Delegate callback) {
            invoker.Invoke<T>(functionName, args, (HproseCallback1<T>)callback);
        }
    }
}
#endif
