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
 * LastModified: Jan 23, 2016                             *
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
#if !dotNETMF
        private HttpListener Listener = new HttpListener();
#else
        private HttpListener Listener = null;
#endif
        private string url = null;
        private string crossDomainXmlFile = null;
        private string clientAccessPolicyXmlFile = null;
#if !dotNETMF
        private string crossDomainXmlContent = null;
        private string clientAccessPolicyXmlContent = null;
#endif
        private byte[] crossDomainXml = null;
        private byte[] clientAccessPolicyXml = null;
        private string lastModified = null;
        private string etag = null;
        private int tCount = 2;

        public HproseHttpListenerServer(string url) {
            Url = url.Replace("0.0.0.0", "*");
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
#if !dotNETMF
                Listener.Prefixes.Clear();
                Listener.Prefixes.Add(url);
#else
                Listener = new HttpListener(url);
#endif
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

#if !dotNETMF
        public string CrossDomainXmlFile {
            get {
                return crossDomainXmlFile;
            }
            set {
                crossDomainXmlFile = value;
                crossDomainXmlContent = File.ReadAllText(value);
                crossDomainXml = Encoding.ASCII.GetBytes(crossDomainXmlContent);
            }
        }

        public string CrossDomainXmlContent {
            get {
                return crossDomainXmlContent;
            }
            set {
                crossDomainXmlContent = value;
                crossDomainXml = Encoding.ASCII.GetBytes(value);
                crossDomainXmlFile = null;
            }
        }

        public byte[] CrossDomainXml {
            get {
                return crossDomainXml;
            }
            set {
                crossDomainXmlContent = Encoding.ASCII.GetString(value);
                crossDomainXml = value;
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
                clientAccessPolicyXml = Encoding.ASCII.GetBytes(clientAccessPolicyXmlContent);
            }
        }

        public string ClientAccessPolicyXmlContent {
            get {
                return clientAccessPolicyXmlContent;
            }
            set {
                clientAccessPolicyXmlContent = value;
                clientAccessPolicyXml = Encoding.ASCII.GetBytes(value);
                clientAccessPolicyXmlFile = null;
            }
        }

        public byte[] ClientAccessPolicyXml {
            get {
                return clientAccessPolicyXml;
            }
            set {
                clientAccessPolicyXmlContent = Encoding.ASCII.GetString(value);
                clientAccessPolicyXml = value;
                clientAccessPolicyXmlFile = null;
            }
        }
#else
        public string CrossDomainXmlFile {
            get {
                return crossDomainXmlFile;
            }
            set {
                crossDomainXmlFile = value;
                crossDomainXml = File.ReadAllBytes(value);
            }
        }

        public byte[] CrossDomainXml {
            get {
                return crossDomainXml;
            }
            set {
                crossDomainXml = value;
                crossDomainXmlFile = null;
            }
        }

        public string ClientAccessPolicyXmlFile {
            get {
                return clientAccessPolicyXmlFile;
            }
            set {
                clientAccessPolicyXmlFile = value;
                clientAccessPolicyXml = File.ReadAllBytes(value);
            }
        }

        public byte[] ClientAccessPolicyXml {
            get {
                return clientAccessPolicyXml;
            }
            set {
                clientAccessPolicyXml = value;
                clientAccessPolicyXmlFile = null;
            }
        }
#endif

        private bool CrossDomainXmlHandler(HttpListenerContext context) {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            if (request.Url.AbsolutePath.ToLower() == "/crossdomain.xml") {
                if (request.Headers["If-Modified-Since"] == lastModified &&
                    request.Headers["If-None-Match"] == etag) {
                    response.StatusCode = 304;
                }
                else {
#if !dotNETMF
                    response.AppendHeader("Last-Modified", lastModified);
                    response.AppendHeader("Etag", etag);
#else
                    response.Headers.Add("Last-Modified", lastModified);
                    response.Headers.Add("Etag", etag);
#endif
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
#if !dotNETMF
                    response.AppendHeader("Last-Modified", lastModified);
                    response.AppendHeader("Etag", etag);
#else
                    response.Headers.Add("Last-Modified", lastModified);
                    response.Headers.Add("Etag", etag);
#endif
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

        public void Stop() {
            if (Listener.IsListening) {
                Listener.Stop();
            }
        }

        public void Close() {
            Listener.Close();
#if !dotNETMF
            Listener = new HttpListener();
            Listener.Prefixes.Add(url);
#else
            Listener = new HttpListener(url);
#endif
        }

        public void Abort() {
            Listener.Abort();
#if !dotNETMF
            Listener = new HttpListener();
            Listener.Prefixes.Add(url);
#else
            Listener = new HttpListener(url);
#endif
        }

 #if !dotNETMF
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

        private void GetContext(IAsyncResult result) {
            HttpListenerContext context = null;
            try {
                context = Listener.EndGetContext(result);
                Listener.BeginGetContext(GetContext, Listener);
                if (clientAccessPolicyXml != null && ClientAccessPolicyXmlHandler(context)) return;
                if (crossDomainXml != null && CrossDomainXmlHandler(context)) return;
                Handle(context);
            }
            catch (Exception e) {
                FireErrorEvent(e, new HproseHttpListenerContext(context));
            }
        }
#else
         public void Start() {
            if (Listener.IsListening) {
                return;
            }
            lastModified = DateTime.Now.ToString("R");
            etag = '"' + new Random().Next().ToString("x") + ":" + new Random().Next().ToString() + '"';
            Listener.Start();
            HttpListenerContext context = null;
            while (Listener.IsListening) {
                try {
                    context = Listener.GetContext();
                    if (clientAccessPolicyXml != null && ClientAccessPolicyXmlHandler(context)) continue;
                    if (crossDomainXml != null && CrossDomainXmlHandler(context)) continue;
                    Handle(context);
                }
                catch (Exception e) {
                    FireErrorEvent(e, new HproseHttpListenerContext(context));
                }
            }
        }
#endif
    }
}
#endif
