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
 * LastModified: Apr 21, 2014                             *
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

        private delegate void ReadCallback(ReadContext context);

        private class ReadContext {
            public ReadCallback callback;
            public TcpClient client;
            public NetworkStream stream;
            public byte[] buf;
            public int n;
            public int size;
        }

        private void CloseConnection(TcpClient client, NetworkStream stream) {
            try {
                if (stream != null) {
                    stream.Close();
                }
                if (client != null) {
                    client.Close();
                }
            }
            catch (Exception) { }
        }

        private void AsyncReadCallback(IAsyncResult asyncResult) {
            ReadContext context = (ReadContext)asyncResult.AsyncState;
            TcpClient client = context.client;
            NetworkStream stream = context.stream;
            try {
                int n = stream.EndRead(asyncResult);
                if (n > 0) {
                    context.n += n;
                    context.size -= n;
                    if (context.n < context.buf.Length) {
                        stream.BeginRead(context.buf, context.n, context.size, new AsyncCallback(AsyncReadCallback), context);
                        return;
                    }
                }
                if (context.n < context.buf.Length) {
                    FireErrorEvent(new HproseException("Unexpected EOF"), client);
                    CloseConnection(client, stream);
                    return;
                }
                context.callback(context);
            }
            catch (IOException) {
                CloseConnection(client, stream);
            }
            catch (Exception e) {
                FireErrorEvent(e, client);
                CloseConnection(client, stream);
            }
        }

        private void AsyncRead(TcpClient client, NetworkStream stream, byte[] buf, ReadCallback callback) {
            ReadContext context = new ReadContext();
            context.callback = callback;
            context.client = client;
            context.stream = stream;
            context.buf = buf;
            context.n = 0;
            context.size = buf.Length;
            stream.BeginRead(context.buf, context.n, context.size, new AsyncCallback(AsyncReadCallback), context);
        }

        private void ReadHeadCallback(ReadContext context) {
            byte[] buf = context.buf;
            int len = (int)buf[0] << 24 | (int)buf[1] << 16 | (int)buf[2] << 8 | (int)buf[3];
            AsyncRead(context.client, context.stream, new byte[len], new ReadCallback(ReadBodyCallback));
        }

        private void ReadBodyCallback(ReadContext context) {
            TcpClient client = context.client;
            NetworkStream stream = context.stream;
            byte[] buf = context.buf;
            try {
                AsyncWrite(client, stream, Handle(new MemoryStream(buf, 0, buf.Length), null, client));
            }
            catch (Exception e) {
                FireErrorEvent(e, client);
                CloseConnection(client, stream);
            }
        }

        private class WriteFullContext {
            public TcpClient client;
            public NetworkStream stream;
        }

        private void WriteFullCallback(IAsyncResult asyncResult) {
            WriteFullContext context = (WriteFullContext)asyncResult.AsyncState;
            TcpClient client = context.client;
            NetworkStream stream = context.stream;
            try {
                stream.EndWrite(asyncResult);
                NonBlockingHandle(client, stream);
            }
            catch (Exception e) {
                FireErrorEvent(e, client);
                CloseConnection(client, stream);
            }
        }

        private class WritePartContext : WriteFullContext {
            public byte[] buf;
            public int offset;
            public int length;
        }

        private void WritePartCallback(IAsyncResult asyncResult) {
            WritePartContext context = (WritePartContext)asyncResult.AsyncState;
            TcpClient client = context.client;
            NetworkStream stream = context.stream;
            try {
                stream.EndWrite(asyncResult);
                stream.BeginWrite(context.buf, context.offset, context.length, new AsyncCallback(WriteFullCallback), context);
            }
            catch (Exception e) {
                FireErrorEvent(e, client);
                CloseConnection(client, stream);
            }
        }

        private void AsyncWrite(TcpClient client, NetworkStream stream, MemoryStream data) {
            try {
                int n = (int)data.Length;
                int len = n > 1020 ? 2048 : n > 508 ? 1024 : 512;
                byte[] buf = new byte[len];
                buf[0] = (byte)((n >> 24) & 0xff);
                buf[1] = (byte)((n >> 16) & 0xff);
                buf[2] = (byte)((n >> 8) & 0xff);
                buf[3] = (byte)(n & 0xff);
                int p = len - 4;
                if (n <= p) {
                    data.Read(buf, 4, n);
                    WriteFullContext context = new WriteFullContext();
                    context.client = client;
                    context.stream = stream;
                    stream.BeginWrite(buf, 0, n + 4, new AsyncCallback(WriteFullCallback), context);
                }
                else {
                    data.Read(buf, 4, p);
                    WritePartContext context = new WritePartContext();
                    context.client = client;
                    context.stream = stream;
                    context.buf = data.ToArray();
                    context.offset = p;
                    context.length = n - p;
                    stream.BeginWrite(buf, 0, len, new AsyncCallback(WritePartCallback), context);
                }
            }
            catch (Exception e) {
                FireErrorEvent(e, client);
                CloseConnection(client, stream);
            }
        }

        private void NonBlockingHandle(TcpClient client, NetworkStream stream) {
            AsyncRead(client, stream, new byte[4], new ReadCallback(ReadHeadCallback));
        }

        private void AcceptTcpCallback(IAsyncResult asyncResult) {
            TcpListener listener = asyncResult.AsyncState as TcpListener;
            TcpClient client = null;
            NetworkStream stream = null;
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
                NonBlockingHandle(client, stream);
            }
            catch (ObjectDisposedException) {
                CloseConnection(client, stream);
            }
            catch (Exception e) {
                FireErrorEvent(e, client);
                CloseConnection(client, stream);
            }
        }
    }
}
#endif