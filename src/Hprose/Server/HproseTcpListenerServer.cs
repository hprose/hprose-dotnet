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
 * LastModified: Apr 7, 2014                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !(dotNET10 || dotNET11 || ClientOnly)
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

        protected override object[] FixArguments(Type[] argumentTypes, object[] arguments, int count, object context) {
            TcpClient client = (TcpClient)context;
            if (argumentTypes.Length != count) {
                object[] args = new object[argumentTypes.Length];
                System.Array.Copy(arguments, args, count);
                Type argType = argumentTypes[count];
                if (argType == typeof(TcpClient)) {
                    args[count] = client;
                }
                return args;
            }
            return arguments;
        }

        public override HproseMethods GlobalMethods {
            get {
                if (gMethods == null) {
                    gMethods = new HproseTcpMethods();
                }
                return gMethods;
            }
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

        private LingerOption m_lingerState = new LingerOption(false, 0);
        public LingerOption LingerState {
            get {
                return m_lingerState;
            }
            set {
                m_lingerState = value;
            }
        }
        private bool m_noDelay = false;
        public bool NoDelay {
            get {
                return m_noDelay;
            }
            set {
                m_noDelay = value;
            }
        }
        private int m_receiveBufferSize = 8192;
        public int ReceiveBufferSize {
            get {
                return m_receiveBufferSize;
            }
            set {
                m_receiveBufferSize = value;
            }
        }
        private int m_sendBufferSize = 8192;
        public int SendBufferSize {
            get {
                return m_sendBufferSize;
            }
            set {
                m_sendBufferSize = value;
            }
        }
        private int m_receiveTimeout = 0;
        public int ReceiveTimeout {
            get {
                return m_receiveTimeout;
            }
            set {
                m_receiveTimeout = value;
            }
        }
        private int m_sendTimeout = 0;
        public int SendTimeout {
            get {
                return m_sendTimeout;
            }
            set {
                m_sendTimeout = value;
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
                for (int i = 0; i < localAddrs.Length; ++i) {
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
                for (int i = 0; i < tCount; ++i) {
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
                client.LingerState = m_lingerState;
                client.NoDelay = m_noDelay;
                client.ReceiveBufferSize = m_receiveBufferSize;
                client.SendBufferSize = m_sendBufferSize;
                client.ReceiveTimeout = m_receiveTimeout;
                client.SendTimeout = m_sendTimeout;
                stream = client.GetStream();
                for (; ; ) {
                    HproseHelper.SendDataOverTcp(stream, Handle(HproseHelper.ReceiveDataOverTcp(stream), null, client));
                }
            }
            catch (Exception e) {
                FireErrorEvent(e, client);
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