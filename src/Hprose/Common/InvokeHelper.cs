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
 * LastModified: Feb 22, 2014                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !(dotNET10 || dotNET11 || dotNETCF10)
using System;

namespace Hprose.Common {
    interface IInvokeHelper {
        void Invoke(HproseInvoker invoker, string functionName, object[] args, Delegate callback, HproseErrorEvent errorEvent, bool byRef, HproseResultMode resultMode, bool simple);
    }

    class InvokeHelper<T> : IInvokeHelper {
        public void Invoke(HproseInvoker invoker, string functionName, object[] args, Delegate callback, HproseErrorEvent errorEvent, bool byRef, HproseResultMode resultMode, bool simple) {
            invoker.Invoke<T>(functionName, args, (HproseCallback<T>)callback, errorEvent, byRef, resultMode, simple);
        }
    }
    interface IInvokeHelper1 {
        void Invoke(HproseInvoker invoker, string functionName, object[] args, Delegate callback, HproseErrorEvent errorEvent, HproseResultMode resultMode, bool simple);
    }

    class InvokeHelper1<T> : IInvokeHelper1 {
        public void Invoke(HproseInvoker invoker, string functionName, object[] args, Delegate callback, HproseErrorEvent errorEvent, HproseResultMode resultMode, bool simple) {
            invoker.Invoke<T>(functionName, args, (HproseCallback1<T>)callback, errorEvent, resultMode, simple);
        }
    }
}
#endif
