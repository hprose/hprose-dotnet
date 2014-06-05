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
 * HproseInvoker.cs                                       *
 *                                                        *
 * hprose invoker class for C#.                           *
 *                                                        *
 * LastModified: Feb 22, 2014                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;

namespace Hprose.Common {
    public interface HproseInvoker {
#if !(dotNET10 || dotNET11 || dotNETCF10)
        T Invoke<T>(string functionName);
        T Invoke<T>(string functionName, HproseResultMode resultMode);
        T Invoke<T>(string functionName, object[] arguments);
        T Invoke<T>(string functionName, object[] arguments, HproseResultMode resultMode);
        T Invoke<T>(string functionName, object[] arguments, bool byRef);
        T Invoke<T>(string functionName, object[] arguments, bool byRef, HproseResultMode resultMode);
        T Invoke<T>(string functionName, object[] arguments, bool byRef, bool simple);
        T Invoke<T>(string functionName, object[] arguments, bool byRef, HproseResultMode resultMode, bool simple);
#endif
        object Invoke(string functionName);
        object Invoke(string functionName, object[] arguments);
        object Invoke(string functionName, object[] arguments, bool byRef);
        object Invoke(string functionName, object[] arguments, bool byRef, bool simple);

        object Invoke(string functionName, Type returnType);
        object Invoke(string functionName, object[] arguments, Type returnType);
        object Invoke(string functionName, object[] arguments, Type returnType, bool byRef);
        object Invoke(string functionName, object[] arguments, Type returnType, bool byRef, bool simple);

        object Invoke(string functionName, HproseResultMode resultMode);
        object Invoke(string functionName, object[] arguments, HproseResultMode resultMode);
        object Invoke(string functionName, object[] arguments, bool byRef, HproseResultMode resultMode);
        object Invoke(string functionName, object[] arguments, bool byRef, HproseResultMode resultMode, bool simple);

        object Invoke(string functionName, Type returnType, HproseResultMode resultMode);
        object Invoke(string functionName, object[] arguments, Type returnType, HproseResultMode resultMode);
        object Invoke(string functionName, object[] arguments, Type returnType, bool byRef, HproseResultMode resultMode);
        object Invoke(string functionName, object[] arguments, Type returnType, bool byRef, HproseResultMode resultMode, bool simple);

#if !(dotNET10 || dotNET11 || dotNETCF10)
        void Invoke<T>(string functionName, HproseCallback<T> callback);
        void Invoke<T>(string functionName, HproseCallback<T> callback, HproseResultMode resultMode);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, HproseResultMode resultMode);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, bool byRef);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, bool byRef, HproseResultMode resultMode);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, bool byRef, bool simple);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, bool byRef, HproseResultMode resultMode, bool simple);

        void Invoke<T>(string functionName, HproseCallback1<T> callback);
        void Invoke<T>(string functionName, HproseCallback1<T> callback, HproseResultMode resultMode);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback1<T> callback);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback1<T> callback, HproseResultMode resultMode);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback1<T> callback, bool simple);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback1<T> callback, HproseResultMode resultMode, bool simple);

        void Invoke<T>(string functionName, HproseCallback<T> callback, HproseErrorEvent errorEvent);
        void Invoke<T>(string functionName, HproseCallback<T> callback, HproseErrorEvent errorEvent, HproseResultMode resultMode);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, HproseErrorEvent errorEvent);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, HproseErrorEvent errorEvent, HproseResultMode resultMode);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, HproseErrorEvent errorEvent, bool byRef);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, HproseErrorEvent errorEvent, bool byRef, HproseResultMode resultMode);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, HproseErrorEvent errorEvent, bool byRef, bool simple);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback<T> callback, HproseErrorEvent errorEvent, bool byRef, HproseResultMode resultMode, bool simple);

        void Invoke<T>(string functionName, HproseCallback1<T> callback, HproseErrorEvent errorEvent);
        void Invoke<T>(string functionName, HproseCallback1<T> callback, HproseErrorEvent errorEvent, HproseResultMode resultMode);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback1<T> callback, HproseErrorEvent errorEvent);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback1<T> callback, HproseErrorEvent errorEvent, HproseResultMode resultMode);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback1<T> callback, HproseErrorEvent errorEvent, bool simple);
        void Invoke<T>(string functionName, object[] arguments, HproseCallback1<T> callback, HproseErrorEvent errorEvent, HproseResultMode resultMode, bool simple);
#endif
        void Invoke(string functionName, HproseCallback callback);
        void Invoke(string functionName, object[] arguments, HproseCallback callback);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, bool byRef);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, bool byRef, bool simple);

        void Invoke(string functionName, HproseCallback1 callback);
        void Invoke(string functionName, object[] arguments, HproseCallback1 callback);
        void Invoke(string functionName, object[] arguments, HproseCallback1 callback, bool simple);

        void Invoke(string functionName, HproseCallback callback, HproseErrorEvent errorEvent);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, bool byRef);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, bool byRef, bool simple);

        void Invoke(string functionName, HproseCallback1 callback, HproseErrorEvent errorEvent);
        void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorEvent);
        void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorEvent, bool simple);

        void Invoke(string functionName, HproseCallback callback, Type returnType);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, Type returnType);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, Type returnType, bool byRef);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, Type returnType, bool byRef, bool simple);

        void Invoke(string functionName, HproseCallback1 callback, Type returnType);
        void Invoke(string functionName, object[] arguments, HproseCallback1 callback, Type returnType);
        void Invoke(string functionName, object[] arguments, HproseCallback1 callback, Type returnType, bool simple);

        void Invoke(string functionName, HproseCallback callback, HproseErrorEvent errorEvent, Type returnType);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, Type returnType);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, Type returnType, bool byRef);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, Type returnType, bool byRef, bool simple);

        void Invoke(string functionName, HproseCallback1 callback, HproseErrorEvent errorEvent, Type returnType);
        void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorEvent, Type returnType);
        void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorEvent, Type returnType, bool simple);

        void Invoke(string functionName, HproseCallback callback, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, bool byRef, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, bool byRef, HproseResultMode resultMode, bool simple);

        void Invoke(string functionName, HproseCallback1 callback, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseResultMode resultMode, bool simple);

        void Invoke(string functionName, HproseCallback callback, Type returnType, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, Type returnType, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, Type returnType, bool byRef, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, Type returnType, bool byRef, HproseResultMode resultMode, bool simple);

        void Invoke(string functionName, HproseCallback1 callback, Type returnType, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback1 callback, Type returnType, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback1 callback, Type returnType, HproseResultMode resultMode, bool simple);

        void Invoke(string functionName, HproseCallback callback, HproseErrorEvent errorEvent, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, bool byRef, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, bool byRef, HproseResultMode resultMode, bool simple);

        void Invoke(string functionName, HproseCallback1 callback, HproseErrorEvent errorEvent, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorEvent, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorEvent, HproseResultMode resultMode, bool simple);

        void Invoke(string functionName, HproseCallback callback, HproseErrorEvent errorEvent, Type returnType, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, Type returnType, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, Type returnType, bool byRef, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback callback, HproseErrorEvent errorEvent, Type returnType, bool byRef, HproseResultMode resultMode, bool simple);

        void Invoke(string functionName, HproseCallback1 callback, HproseErrorEvent errorEvent, Type returnType, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorEvent, Type returnType, HproseResultMode resultMode);
        void Invoke(string functionName, object[] arguments, HproseCallback1 callback, HproseErrorEvent errorEvent, Type returnType, HproseResultMode resultMode, bool simple);

    }
}