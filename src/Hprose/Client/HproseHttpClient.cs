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
 * HproseHttpClient.cs                                    *
 *                                                        *
 * hprose http client class for C#.                       *
 *                                                        *
 * LastModified: Feb 8, 2014                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
using System;
using System.Collections;
#if !(dotNET10 || dotNET11 || dotNETCF10)
using System.Collections.Generic;
#endif
using System.IO;
#if !(SILVERLIGHT || WINDOWS_PHONE || Core)
using System.IO.Compression;
#elif !(SL2 || Core)
using System.Net.Browser;
#endif
using System.Net;
using Hprose.IO;
using Hprose.Common;
#if !(dotNET10 || dotNET11 || dotNETCF10 || dotNETCF20 || SILVERLIGHT || WINDOWS_PHONE || Core)
using System.Security.Cryptography.X509Certificates;
#endif
using System.Threading;

namespace Hprose.Client {
    public class HproseHttpClient : HproseClient {
#if !SL2
        private static bool disableGlobalCookie = false;
        public bool DisableGlobalCookie {
            get {
                return disableGlobalCookie;
            }
            set {
                disableGlobalCookie = value;
            }
        }
#endif
#if (PocketPC || Smartphone || WindowsCE)
        private static CookieManager globalCookieManager = new CookieManager();
        private CookieManager cookieManager = disableGlobalCookie ? new CookieManager() : globalCookieManager;
#elif !SL2
        private static CookieContainer globalCookieContainer = new CookieContainer();
        private CookieContainer cookieContainer = disableGlobalCookie ? new CookieContainer() : globalCookieContainer;
#endif
        private class AsyncContext {
            internal HttpWebRequest request;
            internal HttpWebResponse response = null;
            internal MemoryStream data;
            internal AsyncCallback callback;
#if !Core
            internal Timer timer;
#endif
            internal AsyncContext(HttpWebRequest request) {
                this.request = request;
            }
        }


#if !(dotNET10 || dotNET11 || dotNETCF10)
        private Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
#elif MONO
        private Hashtable headers = new Hashtable(StringComparer.OrdinalIgnoreCase);
#else
        private Hashtable headers = new Hashtable(new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());
#endif
#if !Core
		private int timeout = 30000;
#endif
#if !(SILVERLIGHT || WINDOWS_PHONE)
        private ICredentials credentials = null;
#if !Core
        private bool keepAlive = true;
        private int keepAliveTimeout = 300;
        private IWebProxy proxy = null;
        private string encoding = null;
#if !dotNETCF10
        private string connectionGroupName = null;
#if !(dotNET10 || dotNET11 || dotNETCF20 || UNITY_WEBPLAYER)
        private X509CertificateCollection clientCertificates = null;
#endif
#endif
#endif
#endif
        public HproseHttpClient()
            : base() {
        }

        public HproseHttpClient(string uri)
            : base(uri) {
        }

        public HproseHttpClient(HproseMode mode)
            : base(mode) {
        }

        public HproseHttpClient(string uri, HproseMode mode)
            : base(uri, mode) {
        }

        public static new HproseClient Create(string uri, HproseMode mode) {
            Uri u = new Uri(uri);
            if (u.Scheme != "http" &&
                u.Scheme != "https") {
                throw new HproseException("This client doesn't support " + u.Scheme + " scheme.");
            }
            return new HproseHttpClient(uri, mode);
        }

        public void SetHeader(string name, string value) {
            string nl = name.ToLower();
            if (nl != "content-type" &&
                nl != "content-length" &&
                nl != "host") {
                if (value == null) {
                    headers.Remove(name);
                }
                else {
                    headers[name] = value;
                }
            }
        }

        public string GetHeader(string name) {
#if (dotNET10 || dotNET11 || dotNETCF10)
            return (string)headers[name];
#else
            return headers[name];
#endif
        }

#if !Core
        public int Timeout {
            get {
                return timeout;
            }
            set {
                timeout = value;
            }
        }
#endif

#if !(SILVERLIGHT || WINDOWS_PHONE)
        public ICredentials Credentials {
            get {
                return credentials;
            }
            set {
                credentials = value;
            }
        }

#if !Core
        public bool KeepAlive {
            get {
                return keepAlive;
            }
            set {
                keepAlive = value;
            }
        }

        public int KeepAliveTimeout {
            get {
                return keepAliveTimeout;
            }
            set {
                keepAliveTimeout = value;
            }
        }

        public IWebProxy Proxy {
            get {
                return proxy;
            }
            set {
                proxy = value;
            }
        }

        public string AcceptEncoding {
            get {
                return encoding;
            }
            set {
                encoding = value;
            }
        }

#if !dotNETCF10
        public string ConnectionGroupName {
            get {
                return connectionGroupName;
            }
            set {
                connectionGroupName = value;
            }
        }
#if !(dotNET10 || dotNET11 || dotNETCF20 || UNITY_WEBPLAYER)
        public X509CertificateCollection ClientCertificates {
            get {
                return clientCertificates;
            }
            set {
                clientCertificates = value;
            }
        }
#endif
#endif
#endif
#endif

        private HttpWebRequest GetRequest() {
            Uri uri = new Uri(this.uri);
#if !(SILVERLIGHT || WINDOWS_PHONE) || SL2
            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
#else
            HttpWebRequest request = WebRequestCreator.ClientHttp.Create(uri) as HttpWebRequest;
#endif
            request.Method = "POST";
            request.ContentType = "application/hprose";
#if !(SILVERLIGHT || WINDOWS_PHONE)
            request.Credentials = credentials;
#if !Core
#if !(PocketPC || Smartphone || WindowsCE)
            request.ServicePoint.ConnectionLimit = Int32.MaxValue;
#endif
            request.Timeout = timeout;
            request.SendChunked = false;
            if (encoding != null) {
                request.Headers.Set("Accept-Encoding", encoding);
            }
#if !(dotNET10 || dotNETCF10)
            request.ReadWriteTimeout = timeout;
#endif
            request.ProtocolVersion = HttpVersion.Version11;
            if (proxy != null) {
                request.Proxy = proxy;
            }
            request.KeepAlive = keepAlive;
            if (keepAlive) {
                request.Headers.Set("Keep-Alive", KeepAliveTimeout.ToString());
            }
#if !dotNETCF10
            request.ConnectionGroupName = connectionGroupName;
#if !(dotNET10 || dotNET11 || dotNETCF20 || UNITY_WEBPLAYER)
            if (clientCertificates != null) {
                request.ClientCertificates = clientCertificates;
            }
#endif
#endif
#endif
#endif
#if (dotNET10 || dotNET11 || dotNETCF10)
            foreach (DictionaryEntry header in headers) {
                request.Headers[(string)header.Key] = (string)header.Value;
            }
#else
            foreach (KeyValuePair<string, string> header in headers) {
                request.Headers[header.Key] = header.Value;
            }
#endif
#if (PocketPC || Smartphone || WindowsCE)
            request.AllowWriteStreamBuffering = true;
            request.Headers["Cookie"] = cookieManager.GetCookie(uri.Host,
                                                                uri.AbsolutePath,
                                                                uri.Scheme == "https");
#elif !SL2
            request.CookieContainer = cookieContainer;
#endif
            return request;
        }

        private void Send(MemoryStream data, Stream ostream) {
            data.WriteTo(ostream);
            ostream.Flush();
#if (dotNET10 || dotNET11 || dotNETCF10 || dotNETCF20)
            ostream.Close();
#else
            ostream.Dispose();
#endif
        }

        private MemoryStream Receive(HttpWebRequest request, HttpWebResponse response) {
#if (PocketPC || Smartphone || WindowsCE)
            cookieManager.SetCookie(response.Headers.GetValues("Set-Cookie"), request.RequestUri.Host);
            cookieManager.SetCookie(response.Headers.GetValues("Set-Cookie2"), request.RequestUri.Host);
#endif
            Stream istream = response.GetResponseStream();
#if !(SILVERLIGHT || WINDOWS_PHONE || Core)
            string contentEncoding = response.ContentEncoding.ToLower();
            if (contentEncoding.IndexOf("deflate") > -1) {
                istream = new DeflateStream(istream, CompressionMode.Decompress);
            }
            else if (contentEncoding.IndexOf("gzip") > -1) {
                istream = new GZipStream(istream, CompressionMode.Decompress);
            }
#endif
            int len = (int)response.ContentLength;
            MemoryStream data = (len > 0) ? new MemoryStream(len) : new MemoryStream();
            len = (len > 81920 || len < 0) ? 81920 : len;
            byte[] buffer = new byte[len];
            for (;;) {
                int size = istream.Read(buffer, 0, len);
                if (size == 0) break;
                data.Write(buffer, 0, size);
            }
#if (dotNET10 || dotNET11 || dotNETCF10 || dotNETCF20)
            istream.Close();
#else
            istream.Dispose();
#endif
#if dotNET45
            response.Dispose();
#else
            response.Close();
#endif
            return data;
        }

        // SyncInvoke
#if !(SILVERLIGHT || WINDOWS_PHONE || Core)
        protected override MemoryStream SendAndReceive(MemoryStream data) {
            HttpWebRequest request = GetRequest();
            Send(data, request.GetRequestStream());
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return Receive(request, response);
        }
#endif

#if !Core
        protected void TimeoutHandler(object state) {
            AsyncContext context = (AsyncContext)state;
            try {
                if (context.response == null) {
                    if (context.request != null) {
                        context.request.Abort();
                    }
                }
                else {
                    context.response.Close();
                }
                if (context.timer != null) {
                    context.timer.Dispose();
                    context.timer = null;
                }
            }
            catch (Exception) { }
        }
#endif
        // AsyncInvoke
        protected override IAsyncResult BeginSendAndReceive(MemoryStream data, AsyncCallback callback) {
            HttpWebRequest request = GetRequest();
            AsyncContext context = new AsyncContext(request);
#if !Core
            context.timer = new Timer(new TimerCallback(TimeoutHandler),
                                      context,
                                      timeout,
                                     0);
#endif
            context.data = data;
            context.callback = callback;
            return request.BeginGetRequestStream(new AsyncCallback(EndSend), context);
        }

        private void EndSend(IAsyncResult asyncResult) {
            AsyncContext context = (AsyncContext)asyncResult.AsyncState;
            Send(context.data, context.request.EndGetRequestStream(asyncResult));
            context.request.BeginGetResponse(context.callback, context);
        }

        protected override MemoryStream EndSendAndReceive(IAsyncResult asyncResult) {
            AsyncContext context = (AsyncContext)asyncResult.AsyncState;
            HttpWebRequest request = context.request;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult);
            context.response = response;
            MemoryStream data = Receive(request, response);
#if !Core
            if (context.timer != null) {
                context.timer.Dispose();
                context.timer = null;
            }
#endif
            return data;
        }
    }
}
