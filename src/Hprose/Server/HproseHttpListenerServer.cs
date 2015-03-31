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
 * HproseHttpListenerServer.cs                            *
 *                                                        *
 * hprose http listener server class for C#.              *
 *                                                        *
 * LastModified: Mar 31, 2015                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !(dotNET10 || dotNET11 || ClientOnly || Smartphone)
using System;
using System.IO;
using System.Net;
using System.Text;
using Hprose.IO;
using Hprose.Common;

namespace Hprose.Server {
    public class HproseHttpListenerServer : HproseHttpListenerService {
        private HttpListener Listener = new HttpListener();
        private string url = null;
        private string crossDomainXmlFile = null;
        private string crossDomainXmlContent = null;
        private string clientAccessPolicyXmlFile = null;
        private string clientAccessPolicyXmlContent = null;
        private string lastModified = null;
        private string etag = null;
        private int tCount = 2;

        public HproseHttpListenerServer(string url) {
            Url = url;
        }

        public HproseHttpListenerServer()
            : this("http://127.0.0.1/") {
        }

        public string Url {
            get {
                return url;
            }
            set {
                url = value;
                Listener.Prefixes.Clear();
                Listener.Prefixes.Add(url);
            }
        }

        public int ThreadCount {
            get {
                return tCount;
            }
            set {
                tCount = value;
            }
        }

        public bool IsStarted {
            get {
                return Listener.IsListening;
            }
        }

        public string CrossDomainXmlFile {
            get {
                return crossDomainXmlFile;
            }
            set {
                crossDomainXmlFile = value;
                crossDomainXmlContent = File.ReadAllText(value);
            }
        }

        public string CrossDomainXmlContent {
            get {
                return crossDomainXmlContent;
            }
            set {
                crossDomainXmlContent = value;
                crossDomainXmlFile = null;
            }
        }

        public string ClientAccessPolicyXmlFile {
            get {
                return clientAccessPolicyXmlFile;
            }
            set {
                clientAccessPolicyXmlFile = value;
                clientAccessPolicyXmlContent = File.ReadAllText(value);
            }
        }

        public string ClientAccessPolicyXmlContent {
            get {
                return clientAccessPolicyXmlContent;
            }
            set {
                clientAccessPolicyXmlContent = value;
                clientAccessPolicyXmlFile = null;
            }
        }

        private bool CrossDomainXmlHandler(HttpListenerContext context) {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            if (request.Url.AbsolutePath.ToLower() == "/crossdomain.xml") {
                if (request.Headers["If-Modified-Since"] == lastModified &&
                    request.Headers["If-None-Match"] == etag) {
                    response.StatusCode = 304;
                }
                else {
                    byte[] crossDomainXml = Encoding.ASCII.GetBytes(crossDomainXmlContent);
                    response.AppendHeader("Last-Modified", lastModified);
                    response.AppendHeader("Etag", etag);
                    response.ContentType = "text/xml";
                    response.ContentLength64 = crossDomainXml.Length;
                    response.SendChunked = false;
                    response.OutputStream.Write(crossDomainXml, 0, crossDomainXml.Length);
                    response.OutputStream.Flush();
                }
                response.Close();
                return true;
            }
            return false;
        }

        private bool ClientAccessPolicyXmlHandler(HttpListenerContext context) {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            if (request.Url.AbsolutePath.ToLower() == "/clientaccesspolicy.xml") {
                if (request.Headers["If-Modified-Since"] == lastModified &&
                    request.Headers["If-None-Match"] == etag) {
                    response.StatusCode = 304;
                }
                else {
                    byte[] clientAccessPolicyXml = Encoding.ASCII.GetBytes(clientAccessPolicyXmlContent);
                    response.AppendHeader("Last-Modified", lastModified);
                    response.AppendHeader("Etag", etag);
                    response.ContentType = "text/xml";
                    response.ContentLength64 = clientAccessPolicyXml.Length;
                    response.SendChunked = false;
                    response.OutputStream.Write(clientAccessPolicyXml, 0, clientAccessPolicyXml.Length);
                    response.OutputStream.Flush();
                }
                response.Close();
                return true;
            }
            return false;
        }

        public void Start() {
            if (Listener.IsListening) {
                return;
            }
            lastModified = DateTime.Now.ToString("R");
            etag = '"' + new Random().Next().ToString("x") + ":" + new Random().Next().ToString() + '"';
            Listener.Start();
            for (int i = 0; i < tCount; ++i) {
                Listener.BeginGetContext(GetContext, Listener);
            }
        }

        public void Stop() {
            if (Listener.IsListening) {
                Listener.Stop();
            }
        }

        public void Close() {
            Listener.Close();
            Listener = new HttpListener();
            Listener.Prefixes.Add(url);
        }

        public void Abort() {
            Listener.Abort();
            Listener = new HttpListener();
            Listener.Prefixes.Add(url);
        }

        private void GetContext(IAsyncResult result) {
            HttpListenerContext context = null;
            try {
                context = Listener.EndGetContext(result);
                Listener.BeginGetContext(GetContext, Listener);
                if (clientAccessPolicyXmlContent != null && ClientAccessPolicyXmlHandler(context)) return;
                if (crossDomainXmlContent != null && CrossDomainXmlHandler(context)) return;
                Handle(context);
            }
            catch (Exception e) {
                FireErrorEvent(e, context);
            }
        }
    }
}
#endif
