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
 * HproseUnityHttpClient.cs                               *
 *                                                        *
 * hprose http client class for C#.                       *
 *                                                        *
 * LastModified: Jan 31, 2015                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections;
using System.IO;
using System.Threading;
using Hprose.IO;
using Hprose.Common;
using UnityEngine;

namespace Hprose.Client {
    public class HproseUnityHttpClient : HproseClient {

        private class AsyncResult: IAsyncResult {
            private WWW www;
            internal AsyncResult(WWW www) {
                this.www = www;
            }
            public object AsyncState { get { return www; } }
            public WaitHandle AsyncWaitHandle { get { return null; } }
            public bool CompletedSynchronously { get { return true; } }
            public bool IsCompleted { get { return true; } }
        }

        private MonoBehaviour mb;

        public HproseUnityHttpClient()
            : base() {
            mb = new MonoBehaviour();
        }

        public HproseUnityHttpClient(string uri)
            : base(uri) {
            mb = new MonoBehaviour();
        }

        public HproseUnityHttpClient(HproseMode mode)
            : base(mode) {
            mb = new MonoBehaviour();
        }

        public HproseUnityHttpClient(string uri, HproseMode mode)
            : base(uri, mode) {
            mb = new MonoBehaviour();
        }

        public static new HproseClient Create(string uri, HproseMode mode) {
            Uri u = new Uri(uri);
            if (u.Scheme != "http" &&
                u.Scheme != "https") {
                throw new HproseException("This client doesn't support " + u.Scheme + " scheme.");
            }
            return new HproseUnityHttpClient(uri, mode);
        }

        private IEnumerator Start(MemoryStream data, AsyncCallback callback) {
            WWW www = new WWW(uri, data.ToArray());
            yield return www;
            callback(new AsyncResult(www));
        }

        protected override MemoryStream SendAndReceive(MemoryStream data) {
            MemoryStream result = null;
            Exception error = null;
            AutoResetEvent done = new AutoResetEvent(false);
            mb.StartCoroutine(Start(data, delegate(IAsyncResult asyncResult) {
                WWW www = (WWW)asyncResult.AsyncState;
                if (!string.IsNullOrEmpty(www.error)) {
                    error = new Exception(www.error);
                }
                else {
                    result = new MemoryStream(www.bytes);
                }
                done.Set();
            }));
            done.WaitOne();
            if (error != null) throw error;
            return result;
        }

        // AsyncInvoke
        protected override IAsyncResult BeginSendAndReceive(MemoryStream data, AsyncCallback callback) {
            mb.StartCoroutine(Start(data, callback));
            return null;
        }

        protected override MemoryStream EndSendAndReceive(IAsyncResult asyncResult) {
            WWW www = (WWW)asyncResult.AsyncState;
            if (!string.IsNullOrEmpty(www.error)) {
                throw new Exception(www.error);
            }
            MemoryStream result = new MemoryStream(www.bytes);
            return result;
        }
    }
}
