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
 * HproseTcpClient.cs                                     *
 *                                                        *
 * hprose tcp client class for C#.                        *
 *                                                        *
 * LastModified: Jan 28, 2016                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !(SILVERLIGHT || WINDOWS_PHONE || Core || PORTABLE || dotNETMF)
using System;
#if (dotNET10 || dotNET11 || dotNETCF10)
using System.Collections;
#else
using System.Collections.Generic;
#endif
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Hprose.IO;
using Hprose.Common;

namespace Hprose.Client {
    public class HproseTcpClient : HproseClient, IDisposable {

        public HproseTcpClient()
            : base() {
            InitHproseTcpClient();
        }

        public HproseTcpClient(string uri)
            : base(uri) {
            InitHproseTcpClient();
        }

        public HproseTcpClient(HproseMode mode)
            : base(mode) {
            InitHproseTcpClient();
        }

        public HproseTcpClient(string uri, HproseMode mode)
            : base(uri, mode) {
            InitHproseTcpClient();
        }

        public static new HproseClient Create(string uri, HproseMode mode) {
            Uri u = new Uri(uri);
            if (u.Scheme != "tcp" &&
                u.Scheme != "tcp4" &&
                u.Scheme != "tcp6") {
                throw new HproseException("This client doesn't support " + u.Scheme + " scheme.");
            }
            return new HproseTcpClient(uri, mode);
        }

        public override void UseService(string uri) {
            if (this.uri != null && this.uri != uri) {
                pool.Close(this.uri);
            }
            base.UseService(uri);
        }

        public void Dispose() {
            if (this.uri != null) {
                pool.Close(this.uri);
                this.uri = null;
            }
        }

        public void Close() {
            Dispose();
        }

        private LingerOption m_lingerState;
        public LingerOption LingerState {
            get {
                return m_lingerState;
            }
            set {
                m_lingerState = value;
            }
        }
        private bool m_noDelay;
        public bool NoDelay {
            get {
                return m_noDelay;
            }
            set {
                m_noDelay = value;
            }
        }
        private int m_receiveBufferSize;
        public int ReceiveBufferSize {
            get {
                return m_receiveBufferSize;
            }
            set {
                m_receiveBufferSize = value;
            }
        }
        private int m_sendBufferSize;
        public int SendBufferSize {
            get {
                return m_sendBufferSize;
            }
            set {
                m_sendBufferSize = value;
            }
        }
#if !Smartphone
        private int m_receiveTimeout;
        public int ReceiveTimeout {
            get {
                return m_receiveTimeout;
            }
            set {
                m_receiveTimeout = value;
            }
        }
        private int m_sendTimeout;
        public int SendTimeout {
            get {
                return m_sendTimeout;
            }
            set {
                m_sendTimeout = value;
            }
        }
#endif

        internal struct TcpConn {
            public TcpClient client;
            public NetworkStream stream;
        }

        internal enum TcpConnStatus {
            Free, Using, Closing
        }

        internal class TcpConnEntry {
            public string uri;
            public TcpConn conn;
            public TcpConnStatus status;
            public long lastUsedTime;
            public TcpConnEntry(string uri) {
                this.uri = uri;
                this.conn.client = null;
                this.conn.stream = null;
                this.status = TcpConnStatus.Using;
            }
            public void Set(TcpClient client) {
                if (client != null) {
                    this.conn.client = client;
                    this.conn.stream = client.GetStream();
                }
            }
            public void Close() {
                this.status = TcpConnStatus.Closing;
            }
        }
        internal class TcpConnPool {
#if (dotNET10 || dotNET11 || dotNETCF10)
            private readonly ArrayList pool = new ArrayList();
#else
            private readonly List<TcpConnEntry> pool = new List<TcpConnEntry>();
#endif
            private readonly object syncRoot = new object();

            private Timer timer;

            private long m_timeout = 0;

            public long Timeout {
                get {
                    return m_timeout;
                }
                set {
                    m_timeout = value;
                    if (m_timeout > 0) {
                        if (timer == null) {
                            timer = new Timer(new TimerCallback(CloseTimeoutConns),
                                    null,
                                    m_timeout,
                                    m_timeout);
                        }
                        else {
                            timer.Change(m_timeout, m_timeout);
                        }
                    }
                    else {
                        timer.Dispose();
                        timer = null;
                    }
                }
            }

#if (dotNET10 || dotNET11 || dotNETCF10)
            private void CloseConn(TcpConn conn) {
                if (conn.stream != null) {
                    conn.stream.Close();
                }
                if (conn.client != null) {
                    conn.client.Close();
                }
            }

            private void FreeConns(ArrayList conns) {
                foreach (TcpConn conn in conns) {
                    if (conn.stream != null) {
                        conn.stream.Close();
                    }
                    if (conn.client != null) {
                        conn.client.Close();
                    }
                }
            }
#else
            private void CloseConn(TcpConn conn) {
                new Thread(delegate() {
                    try {
                        if (conn.stream != null) {
                            conn.stream.Close();
                        }
                        if (conn.client != null) {
                            conn.client.Close();
                        }
                    }
                    catch (Exception) { }
                }).Start();
            }

            private void FreeConns(List<TcpConn> conns) {
                new Thread(delegate() {
                    foreach (TcpConn conn in conns) {
                        try {
                            if (conn.stream != null) {
                                conn.stream.Close();
                            }
                            if (conn.client != null) {
                                conn.client.Close();
                            }
                        }
                        catch (Exception) { }
                    }
                }).Start();
            }
#endif

            private void CloseTimeoutConns(object state) {
#if (dotNET10 || dotNET11 || dotNETCF10)
                ArrayList conns = new ArrayList();
#else
                List<TcpConn> conns = new List<TcpConn>(pool.Count);
#endif
                lock(syncRoot) {
                    foreach (TcpConnEntry entry in pool) {
                        if (entry.status == TcpConnStatus.Free && entry.uri != null) {
                            if ((DateTime.Now.Ticks - entry.lastUsedTime) > m_timeout * 10000) {
                                conns.Add(entry.conn);
                                entry.conn.stream = null;
                                entry.conn.client = null;
                                entry.uri = null;
                            }
                            else if (entry.conn.stream != null) {
                                try {
                                    if (entry.conn.stream.DataAvailable) {}
                                }
                                catch {
                                    conns.Add(entry.conn);
                                    entry.conn.stream = null;
                                    entry.conn.client = null;
                                    entry.uri = null;
                                }
                            }
                        }
                    }
                }
                FreeConns(conns);
            }

            public TcpConnEntry Get(string uri) {
                lock(syncRoot) {
                    foreach (TcpConnEntry entry in pool) {
                        if (entry.status == TcpConnStatus.Free) {
                            if (entry.uri == uri) {
                                if (entry.conn.stream != null) {
                                    try {
                                        if (entry.conn.stream.DataAvailable) {}
                                    }
                                    catch {
                                        CloseConn(entry.conn);
                                        entry.conn.stream = null;
                                        entry.conn.client = null;
                                    }
                                }
                                entry.status = TcpConnStatus.Using;
                                return entry;
                            }
                            else if (entry.uri == null) {
                                entry.status = TcpConnStatus.Using;
                                entry.uri = uri;
                                return entry;
                            }
                        }
                    }
                    TcpConnEntry newEntry = new TcpConnEntry(uri);
                    pool.Add(newEntry);
                    return newEntry;
                }
            }

            public void Close(string uri) {
#if (dotNET10 || dotNET11 || dotNETCF10)
                ArrayList conns = new ArrayList();
#else
                List<TcpConn> conns = new List<TcpConn>(pool.Count);
#endif
                lock(syncRoot) {
                    foreach (TcpConnEntry entry in pool) {
                        if (entry.uri == uri) {
                            if (entry.status == TcpConnStatus.Free) {
                                conns.Add(entry.conn);
                                entry.conn.stream = null;
                                entry.conn.client = null;
                                entry.uri = null;
                            }
                            else {
                                entry.Close();
                            }
                        }
                    }
                }
                FreeConns(conns);
            }
            public void Free(TcpConnEntry entry) {
                if (entry.status == TcpConnStatus.Closing) {
                    if (entry.conn.client != null) {
                        CloseConn(entry.conn);
                        entry.conn.stream = null;
                        entry.conn.client = null;
                    }
                    entry.uri = null;
                }
                lock(syncRoot) {
                    entry.lastUsedTime = DateTime.Now.Ticks;
                    entry.status = TcpConnStatus.Free;
                }
            }
        }

        private readonly TcpConnPool pool = new TcpConnPool();

        public long Timeout {
            get {
                return pool.Timeout;
            }
            set {
                pool.Timeout = value;
            }
        }

        private void InitHproseTcpClient() {
            m_lingerState = new LingerOption(false, 0);
            m_noDelay = false;
            m_receiveBufferSize = 8192;
            m_sendBufferSize = 8192;
#if !Smartphone
            m_receiveTimeout = 0;
            m_sendTimeout = 0;
#endif
        }

        private TcpClient CreateClient(string uri) {
            Uri u = new Uri(uri);
            int i = 0;
            TcpClient client;
            tryagain:
            try {
#if (dotNET10 || dotNETCF10)
                client = new TcpClient(u.Host, u.Port);
#else
                if (u.Scheme == "tcp") {
                    client = new TcpClient(u.Host, u.Port);
                }
                else {
                    client = new TcpClient((u.Scheme == "tcp6") ?
                                            AddressFamily.InterNetworkV6 :
                                            AddressFamily.InterNetwork);
                    client.Connect(u.Host, u.Port);
                }
#endif
            }
            catch (SocketException) {
                if (i < 5) {
                    i++;
                    goto tryagain;
                }
                throw;
            }
            client.LingerState = m_lingerState;
            client.NoDelay = m_noDelay;
            client.ReceiveBufferSize = m_receiveBufferSize;
            client.SendBufferSize = m_sendBufferSize;
#if !Smartphone
            client.ReceiveTimeout = m_receiveTimeout;
            client.SendTimeout = m_sendTimeout;
#endif
            return client;
        }

        private void Send(Stream stream, MemoryStream data) {
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
                stream.Write(buf, 0, n + 4);
            }
            else {
                data.Read(buf, 4, p);
                stream.Write(buf, 0, len);
                stream.Write(data.ToArray(), p, n - p);
            }
            stream.Flush();
        }

        private int ReadAtLeast(Stream stream, byte[] buf, int offset, int min) {
            if (min == 0) return 0;
            int size = buf.Length - offset;
            int n = offset;
            int p = min + offset;
            while (n < p) {
                int nn = stream.Read(buf, n, size);
                if (nn == 0) break;
                n += nn;
                size -= nn;
            }
            if (n < p) {
                throw new HproseException("Unexpected EOF");
            }
            return n - offset;
        }

        private MemoryStream Receive(Stream stream) {
            const int bufferlength = 2048;
            byte[] buf = new byte[bufferlength];
            int n = ReadAtLeast(stream, buf, 0, 4);
            int len = (int)buf[0] << 24 | (int)buf[1] << 16 | (int)buf[2] << 8 | (int)buf[3];
            int size = len - (n - 4);
            if (len <= bufferlength - 4) {
                ReadAtLeast(stream, buf, n, size);
                return new MemoryStream(buf, 4, len);
            }
            n -= 4;
            byte[] data = new byte[len];
            Buffer.BlockCopy(buf, 4, data, 0, n);
            ReadAtLeast(stream, data, n, size);
            return new MemoryStream(data, 0, len);
        }

        protected override MemoryStream SendAndReceive(MemoryStream data) {
            TcpConnEntry entry = pool.Get(uri);
            try {
                if (entry.conn.client == null) {
                    entry.Set(CreateClient(uri));
                }
                Stream stream = entry.conn.stream;
                Send(stream, data);
                data = Receive(stream);
            }
            catch {
                entry.Close();
                throw;
            }
            finally {
                pool.Free(entry);
            }
            return data;
        }

        // AsyncInvoke

        private class SendAndReceiveContext {
            public AsyncCallback callback;
            public AsyncCallback readCallback;
            public TcpConnEntry entry;
            public Exception e;
            public byte[] buf;
            public int offset;
            public int length;
        }

        protected override IAsyncResult BeginSendAndReceive(MemoryStream data, AsyncCallback callback) {
            TcpConnEntry entry = pool.Get(uri);
            try {
                if (entry.conn.client == null) {
                    entry.Set(CreateClient(uri));
                }
                return AsyncSend(entry, data, callback);
            }
            catch {
                entry.Close();
                pool.Free(entry);
                throw;
            }
        }

        protected override MemoryStream EndSendAndReceive(IAsyncResult asyncResult) {
            SendAndReceiveContext context = (SendAndReceiveContext)asyncResult.AsyncState;
            try {
                if (context.e == null) {
                    byte[] buf = context.buf;
                    return new MemoryStream(buf, 0, buf.Length);
                }
                else {
                    context.entry.Close();
                    throw context.e;
                }
            }
            finally {
                pool.Free(context.entry);
            }
        }

        private void AsyncReadCallback(IAsyncResult asyncResult) {
            SendAndReceiveContext context = (SendAndReceiveContext)asyncResult.AsyncState;
            try {
                NetworkStream stream = context.entry.conn.stream;
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
            NetworkStream stream = context.entry.conn.stream;
            byte[] buf = context.buf;
            int len = (int)buf[0] << 24 | (int)buf[1] << 16 | (int)buf[2] << 8 | (int)buf[3];
            context.readCallback = context.callback;
            context.buf = new byte[len];
            context.offset = 0;
            context.length = len;
            stream.BeginRead(context.buf, context.offset, context.length, new AsyncCallback(AsyncReadCallback), context);
        }

        private void WriteFullCallback(IAsyncResult asyncResult) {
            SendAndReceiveContext context = (SendAndReceiveContext)asyncResult.AsyncState;
            try {
                NetworkStream stream = context.entry.conn.stream;
                stream.EndWrite(asyncResult);
                context.readCallback = new AsyncCallback(ReadHeadCallback);
                context.buf = new byte[4];
                context.offset = 0;
                context.length = 4;
                stream.BeginRead(context.buf, context.offset, context.length, new AsyncCallback(AsyncReadCallback), context);
            }
            catch (Exception e) {
                context.e = e;
                context.callback(asyncResult);
            }
        }

        private void WritePartCallback(IAsyncResult asyncResult) {
            SendAndReceiveContext context = (SendAndReceiveContext)asyncResult.AsyncState;
            try {
                NetworkStream stream = context.entry.conn.stream;
                stream.EndWrite(asyncResult);
                stream.BeginWrite(context.buf, context.offset, context.length, new AsyncCallback(WriteFullCallback), context);
            }
            catch (Exception e) {
                context.e = e;
                context.callback(asyncResult);
            }
        }

        private IAsyncResult AsyncSend(TcpConnEntry entry, MemoryStream data, AsyncCallback callback) {
            int n = (int)data.Length;
            int len = n > 1020 ? 2048 : n > 508 ? 1024 : 512;
            byte[] buf = new byte[len];
            buf[0] = (byte)((n >> 24) & 0xff);
            buf[1] = (byte)((n >> 16) & 0xff);
            buf[2] = (byte)((n >> 8) & 0xff);
            buf[3] = (byte)(n & 0xff);
            int p = len - 4;
            SendAndReceiveContext context = new SendAndReceiveContext();
            context.callback = callback;
            context.entry = entry;
            NetworkStream stream = entry.conn.stream;
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
    }
}
#endif
