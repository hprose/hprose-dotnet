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
 * LastModified: Jan 11, 2016                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !ClientOnly
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
#if (dotNET10 || dotNET11 || Smartphone)
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

        protected override object[] FixArguments(Type[] argumentTypes, object[] arguments, int count, HproseContext context) {
            HproseTcpListenerContext currentContext = (HproseTcpListenerContext)context;
            if (argumentTypes.Length != count) {
                object[] args = new object[argumentTypes.Length];
                System.Array.Copy(arguments, 0, args, 0, count);
                Type argType = argumentTypes[count];
                if (argType == typeof(HproseContext) ||
                    argType == typeof(HproseTcpListenerContext)) {
                    args[count] = currentContext;
                }
                else if (argType == typeof(TcpClient)) {
                    args[count] = currentContext.Client;
                }
                return args;
            }
            return arguments;
        }

        public override HproseMethods GlobalMethods {
            get {
                if (gMethods == null) {
                    gMethods = new HproseTcpListenerMethods();
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
#if !Smartphone
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

        private IPAddress GetIPAddress(string host, AddressFamily addressFamily) {
#if (dotNET10 || dotNET11 || Smartphone)
            try {
                return IPAddress.Parse(host);
            }
            catch (Exception) {}
#else
            IPAddress addr;
            if (IPAddress.TryParse(host, out addr)) {
                return addr;
            }
#endif
#if (dotNET10 || dotNET11 || dotNETCF10) && !MONO
            IPAddress[] localAddrs = Dns.Resolve(host).AddressList;
#elif Smartphone
            IPAddress[] localAddrs = Dns.GetHostEntry(host).AddressList;
#else
            IPAddress[] localAddrs = Dns.GetHostAddresses(host);
#endif
#if dotNETCF10
            return localAddrs[0];
#else
            for (int i = 0; i < localAddrs.Length; ++i) {
                if (localAddrs[i].AddressFamily == addressFamily) {
                    return localAddrs[i];
                }
            }
            return localAddrs[0];
#endif
        }

        public void Start() {
            if (Listener == null) {
                Uri u = new Uri(uri);
                IPAddress addr = GetIPAddress(u.Host, (u.Scheme == "tcp6" ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork));
                Listener = new TcpListener(addr, u.Port);
                Listener.Start();
                for (int i = 0; i < tCount; ++i) {
#if !(dotNET10 || dotNET11 || Smartphone)
                    Listener.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpCallback), Listener);
#else
                    Thread t = new Thread(new ThreadStart(AcceptTcp));
#if !dotNETCF10
                    t.IsBackground = true;
#endif
                    t.Start();
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
            public HproseTcpListenerContext context;
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
            AsyncWrite(context, Handle(new MemoryStream(buf, 0, buf.Length), null, context.context));
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
                if (context.context.Client != null) {
                    context.context.Client.Close();
                }
            }
            catch (Exception) { }
        }

        private void ErrorCallback(IAsyncResult asyncResult) {
            SendAndReceiveContext context = (SendAndReceiveContext)asyncResult.AsyncState;
            if (context.e != null) {
                FireErrorEvent(context.e, context.context);
            }
            CloseConnection(context);
        }

#if !(dotNET10 || dotNET11 || Smartphone)
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
                context.context = new HproseTcpListenerContext(client);
                context.stream = stream;
                NonBlockingHandle(context);
            }
            catch (Exception e) {
                if (context.context == null) {
                    context.context = new HproseTcpListenerContext(null);
                }
                FireErrorEvent(e, context.context);
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
#if (dotNET10 || dotNET11)
                    client.ReceiveTimeout = m_receiveTimeout;
                    client.SendTimeout = m_sendTimeout;
#endif
                    NetworkStream stream = client.GetStream();
                    context.callback = new AsyncCallback(ErrorCallback);
                    context.context = new HproseTcpListenerContext(client);
                    context.stream = stream;
                    NonBlockingHandle(context);
                }
                catch (Exception e) {
                    if (context.context == null) {
                        context.context = new HproseTcpListenerContext(null);
                    }
                    FireErrorEvent(e, context.context);
                    CloseConnection(context);
                }
            }
        }
#endif
    }
}
#endif
