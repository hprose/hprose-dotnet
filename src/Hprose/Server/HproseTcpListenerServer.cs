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
 * HproseTcpListenerServer.cs                             *
 *                                                        *
 * hprose tcp listener server class for C#.               *
 *                                                        *
 * LastModified: Feb 27, 2014                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !ClientOnly
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Hprose.IO;
using Hprose.Common;

namespace Hprose.Server {
    public class HproseTcpListenerServer : HproseService {
        private TcpListener Listener;
        private string uri = null;
        private int tCount = 1;

        public HproseTcpListenerServer(string uri) {
            this.uri = uri;
        }

        public string Uri {
            get {
                return uri;
            }
            set {
                uri = value;
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
                return (Listener != null);
            }
        }

        public void Start() {
            if (Listener == null) {
                Uri u = new Uri(uri);
                IPAddress[] localAddrs = Dns.GetHostAddresses(u.Host);
                for (int i = 0; i < localAddrs.Length; i++) {
                    if (u.Scheme == "tcp6") {
                        if (localAddrs[i].AddressFamily == AddressFamily.InterNetworkV6) {
                            Listener = new TcpListener(localAddrs[i], u.Port);
                            break;
                        }
                    }
                    else {
                        if (localAddrs[i].AddressFamily == AddressFamily.InterNetwork) {
                            Listener = new TcpListener(localAddrs[i], u.Port);
                            break;
                        }
                    }
                }
                Listener.Start();
                for (int i = 0; i < tCount; i++) {
                    Listener.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpCallback), Listener);
                }
            }
        }

        public void Stop() {
            if (Listener != null) {
                Listener.Stop();
                Listener = null;
            }
        }

        private void AcceptTcpCallback(IAsyncResult asyncResult) {
            TcpListener listener = asyncResult.AsyncState as TcpListener;
            TcpClient client = null;
            NetworkStream stream = null;
            try {
                listener.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpCallback), listener);
                client = listener.EndAcceptTcpClient(asyncResult);
                stream = client.GetStream();
                for (; ; ) {
                    HproseHelper.SendDataOverTcp(stream, Handle(HproseHelper.ReceiveDataOverTcp(stream), null, null));
                }
            }
            catch (Exception e) {
                FireErrorEvent(e);
            }
            finally {
                if (stream != null) {
                    stream.Close();
                }
                if (client != null) {
                    client.Close();
                }
            }
        }
    }
}
#endif