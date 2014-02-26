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
 * HproseTcpClient.cs                                     *
 *                                                        *
 * hprose tcp client class for C#.                        *
 *                                                        *
 * LastModified: Feb 26, 2014                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/
#if !(SILVERLIGHT || WINDOWS_PHONE || Core)
using System;
#if (dotNET10 || dotNET11 || dotNETCF10)
using System.Collections;
#else
using System.Collections.Generic;
#endif
using System.IO;
using System.Net.Sockets;
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
                throw new HproseException("This client desn't support " + u.Scheme + " scheme.");
            }
            return new HproseTcpClient(uri, mode);
        }

        public override void UseService(string uri) {
            if (this.uri != null) {
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
#if !dotNETCF10
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
            public Stream stream;
        }
        internal class TcpConnEntry {
            public string uri;
            public TcpConn conn;
            public bool free;
            public bool valid;
            public TcpConnEntry(string uri) {
                this.uri = uri;
                this.conn.client = null;
                this.conn.stream = null;
                this.free = false;
                this.valid = false;
            }
            public void Set(TcpClient client) {
                if (client != null) {
                    this.conn.client = client;
                    this.conn.stream = client.GetStream();
                }
            }
            public void Close() {
                this.free = true;
                this.valid = false;
            }
        }
        internal class TcpConnPool {
#if (dotNET10 || dotNET11 || dotNETCF10)
            private readonly ArrayList pool = new ArrayList();
#else
            private readonly List<TcpConnEntry> pool = new List<TcpConnEntry>();
#endif
            private readonly object syncRoot = new object();
            public TcpConnEntry Get(string uri) {
                lock(syncRoot) {
                    foreach (TcpConnEntry entry in pool) {
                        if (entry.free && entry.valid) {
                            if (entry.uri == uri) {
                                entry.free = false;
                                return entry;
                            }
                            else if (entry.uri == null) {
                                entry.free = false;
                                entry.valid = false;
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
            private void FreeConns(
#if (dotNET10 || dotNET11 || dotNETCF10)
                ArrayList conns
#else
                List<TcpConn> conns
#endif
            ) {
                foreach (TcpConn conn in conns) {
                    if (conn.stream != null) {
                        conn.stream.Close();
                    }
                    if (conn.client != null) {
                        conn.client.Close();
                    }
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
                            if (entry.free && entry.valid) {
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
                if (entry.free && !entry.valid) {
                    if (entry.conn.client != null) {
                        entry.conn.stream.Close();
                        entry.conn.stream = null;
                        entry.conn.client.Close();
                        entry.conn.client = null;
                    }
                    entry.uri = null;
                }
                lock(syncRoot) {
                    entry.free = true;
                    entry.valid = true;
                }
            }
        }

        private readonly TcpConnPool pool = new TcpConnPool();

        private delegate MemoryStream SendAndReceiveDelegate(MemoryStream data);
        private SendAndReceiveDelegate sendAndReceive;

        private void InitHproseTcpClient() {
            sendAndReceive = new SendAndReceiveDelegate(SendAndReceive);
            m_lingerState = new LingerOption(false, 0);
            m_noDelay = false;
            m_receiveBufferSize = 8192;
            m_sendBufferSize = 8192;
#if !dotNETCF10
            m_receiveTimeout = 500;
            m_sendTimeout = 500;
#endif
        }

        private TcpClient CreateClient(string uri) {
            Uri u = new Uri(uri);
#if (dotNET10 || dotNETCF10)
            TcpClient client = new TcpClient(u.Host, u.Port);
#else
            TcpClient client;
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
            client.LingerState = m_lingerState;
            client.NoDelay = m_noDelay;
            client.ReceiveBufferSize = m_receiveBufferSize;
            client.SendBufferSize = m_sendBufferSize;
#if !dotNETCF10
            client.ReceiveTimeout = m_receiveTimeout;
            client.SendTimeout = m_sendTimeout;
#endif
            return client;
        }

        protected override MemoryStream SendAndReceive(MemoryStream data) {
            TcpConnEntry entry = pool.Get(uri);
            try {
                if (entry.conn.client == null) {
                    entry.Set(CreateClient(uri));
                }
                Stream stream = entry.conn.stream;
                HproseHelper.SendDataOverTcp(stream, data);
                data = HproseHelper.ReceiveDataOverTcp(stream);
            }
            catch {
                entry.Close();
                pool.Free(entry);
                throw;
            }
            pool.Free(entry);
            return data;
        }

        // AsyncInvoke
        protected override IAsyncResult BeginSendAndReceive(MemoryStream data, AsyncCallback callback) {
            return sendAndReceive.BeginInvoke(data, callback, null);
        }

        protected override MemoryStream EndSendAndReceive(IAsyncResult asyncResult) {
            return sendAndReceive.EndInvoke(asyncResult);
        }
    }
}
#endif
