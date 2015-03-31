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
 * HproseTcpListenerServer.cs                             *
 *                                                        *
 * hprose tcp listener server class for C#.               *
 *                                                        *
 * LastModified: Mar 31, 2015                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !(dotNET10 || dotNET11 || ClientOnly)
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
#if Smartphone
using System.Threading;
#endif
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
                System.Array.Copy(arguments, 0, args, 0, count);
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
#if !dotNETCF10
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
#endif
        public bool IsStarted {
            get {
                return (Listener != null);
            }
        }

        public void Start() {
            if (Listener == null) {
                Uri u = new Uri(uri);
#if !Smartphone
                IPAddress[] localAddrs = Dns.GetHostAddresses(u.Host);
#elif dotNETCF10
                IPAddress[] localAddrs = Dns.Resolve(u.Host).AddressList;
#else
                IPAddress[] localAddrs = Dns.GetHostEntry(u.Host).AddressList;
#endif
#if dotNETCF10
                Listener = new TcpListener(localAddrs[0], u.Port);
#else
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
#endif
                Listener.Start();
                for (int i = 0; i < tCount; ++i) {
#if !Smartphone
                    Listener.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpCallback), Listener);
#else
                    new Thread(new ThreadStart(AcceptTcp));
#endif
                }
            }
        }

        public void Stop() {
            if (Listener != null) {
                Listener.Stop();
                Listener = null;
            }
        }

        private class SendAndReceiveContext {
            public AsyncCallback callback;
            public AsyncCallback readCallback;
            public TcpClient client;
            public NetworkStream stream;
            public Exception e;
            public byte[] buf;
            public int offset;
            public int length;
        }

        private void NonBlockingHandle(SendAndReceiveContext context) {
            context.readCallback = new AsyncCallback(ReadHeadCallback);
            context.buf = new byte[4];
            context.offset = 0;
            context.length = 4;
            context.stream.BeginRead(context.buf, context.offset, context.length, new AsyncCallback(AsyncReadCallback), context);
        }

        private void AsyncReadCallback(IAsyncResult asyncResult) {
            SendAndReceiveContext context = (SendAndReceiveContext)asyncResult.AsyncState;
            try {
                NetworkStream stream = context.stream;
                int n = stream.EndRead(asyncResult);
                if (n > 0) {
                    context.offset += n;
                    context.length -= n;
                    if (context.offset < context.buf.Length) {
                        stream.BeginRead(context.buf, context.offset, context.length, new AsyncCallback(AsyncReadCallback), context);
                        return;
                    }
                }
                if (context.offset < context.buf.Length) {
                    context.e = new HproseException("Unexpected EOF");
                    context.callback(asyncResult);
                    return;
                }
                context.readCallback(asyncResult);
            }
            catch (Exception e) {
                context.e = e;
                context.callback(asyncResult);
            }
        }

        private void ReadHeadCallback(IAsyncResult asyncResult) {
            SendAndReceiveContext context = (SendAndReceiveContext)asyncResult.AsyncState;
            NetworkStream stream = context.stream;
            byte[] buf = context.buf;
            int len = (int)buf[0] << 24 | (int)buf[1] << 16 | (int)buf[2] << 8 | (int)buf[3];
            context.readCallback = new AsyncCallback(ReadBodyCallback);
            context.buf = new byte[len];
            context.offset = 0;
            context.length = len;
            stream.BeginRead(context.buf, context.offset, context.length, new AsyncCallback(AsyncReadCallback), context);
        }

        private void ReadBodyCallback(IAsyncResult asyncResult) {
            SendAndReceiveContext context = (SendAndReceiveContext)asyncResult.AsyncState;
            byte[] buf = context.buf;
            AsyncWrite(context, Handle(new MemoryStream(buf, 0, buf.Length), null, context.client));
        }

        private void WriteFullCallback(IAsyncResult asyncResult) {
            SendAndReceiveContext context = (SendAndReceiveContext)asyncResult.AsyncState;
            try {
                NetworkStream stream = context.stream;
                stream.EndWrite(asyncResult);
                NonBlockingHandle(context);
            }
            catch (Exception e) {
                context.e = e;
                context.callback(asyncResult);
            }
        }

        private void WritePartCallback(IAsyncResult asyncResult) {
            SendAndReceiveContext context = (SendAndReceiveContext)asyncResult.AsyncState;
            try {
                NetworkStream stream = context.stream;
                stream.EndWrite(asyncResult);
                stream.BeginWrite(context.buf, context.offset, context.length, new AsyncCallback(WriteFullCallback), context);
            }
            catch (Exception e) {
                context.e = e;
                context.callback(asyncResult);
            }
        }

        private IAsyncResult AsyncWrite(SendAndReceiveContext context, MemoryStream data) {
            int n = (int)data.Length;
            int len = n > 1020 ? 2048 : n > 508 ? 1024 : 512;
            byte[] buf = new byte[len];
            buf[0] = (byte)((n >> 24) & 0xff);
            buf[1] = (byte)((n >> 16) & 0xff);
            buf[2] = (byte)((n >> 8) & 0xff);
            buf[3] = (byte)(n & 0xff);
            int p = len - 4;
            NetworkStream stream = context.stream;
            if (n <= p) {
                data.Read(buf, 4, n);
                return stream.BeginWrite(buf, 0, n + 4, new AsyncCallback(WriteFullCallback), context);
            }
            else {
                data.Read(buf, 4, p);
                context.buf = data.ToArray();
                context.offset = p;
                context.length = n - p;
                return stream.BeginWrite(buf, 0, len, new AsyncCallback(WritePartCallback), context);
            }
        }

        private void CloseConnection(SendAndReceiveContext context) {
            try {
                if (context.stream != null) {
                    context.stream.Close();
                }
                if (context.client != null) {
                    context.client.Close();
                }
            }
            catch (Exception) { }
        }

        private void ErrorCallback(IAsyncResult asyncResult) {
            SendAndReceiveContext context = (SendAndReceiveContext)asyncResult.AsyncState;
            if (context.e != null) {
                FireErrorEvent(context.e, context.client);
            }
            CloseConnection(context);
        }

#if !Smartphone
        private void AcceptTcpCallback(IAsyncResult asyncResult) {
            TcpListener listener = asyncResult.AsyncState as TcpListener;
            TcpClient client = null;
            NetworkStream stream = null;
            SendAndReceiveContext context = new SendAndReceiveContext();
            try {
                client = listener.EndAcceptTcpClient(asyncResult);
                listener.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpCallback), listener);
                client.LingerState = m_lingerState;
                client.NoDelay = m_noDelay;
                client.ReceiveBufferSize = m_receiveBufferSize;
                client.SendBufferSize = m_sendBufferSize;
                client.ReceiveTimeout = m_receiveTimeout;
                client.SendTimeout = m_sendTimeout;
                stream = client.GetStream();
                context.callback = new AsyncCallback(ErrorCallback);
                context.client = client;
                context.stream = stream;
                NonBlockingHandle(context);
            }
            catch (Exception e) {
                FireErrorEvent(e, client);
                CloseConnection(context);
            }
        }
#else
        private void AcceptTcp() {
            while (true) {
                TcpClient client = null;
                SendAndReceiveContext context = new SendAndReceiveContext();
                try {
                    client = Listener.AcceptTcpClient();
                    client.LingerState = m_lingerState;
                    client.NoDelay = m_noDelay;
                    client.ReceiveBufferSize = m_receiveBufferSize;
                    client.SendBufferSize = m_sendBufferSize;
#if !dotNETCF10
                    client.ReceiveTimeout = m_receiveTimeout;
                    client.SendTimeout = m_sendTimeout;
#endif
                    NetworkStream stream = client.GetStream();
                    context.callback = new AsyncCallback(ErrorCallback);
                    context.client = client;
                    context.stream = stream;
                    NonBlockingHandle(context);
                }
                catch (Exception e) {
                    FireErrorEvent(e, client);
                    CloseConnection(context);
                }
            }
        }
#endif
    }
}
#endif
