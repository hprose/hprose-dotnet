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
 * LastModified: Feb 23, 2014                             *
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
        private delegate Stream GetStream(object context);
        private GetStream getOutputStream = null;
        private GetStream getInputStream = null;

        public HproseTcpClient()
            : base() {
            InitGetStream();
        }

        public HproseTcpClient(string uri)
            : base(uri) {
            InitGetStream();
        }

        public HproseTcpClient(HproseMode mode)
            : base(mode) {
            InitGetStream();
        }

        public HproseTcpClient(string uri, HproseMode mode)
            : base(uri, mode) {
            InitGetStream();
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

        private void InitGetStream() {
            getOutputStream = new GetStream(this.GetOutputStream);
            getInputStream = new GetStream(this.GetInputStream);
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

        protected override object GetInvokeContext() {
            return pool.Get(this.uri);
        }

        protected override void SendData(Stream ostream, object context, bool success) {
            TcpConnEntry entry = context as TcpConnEntry;
            if (success) {
                try {
                    ostream.Flush();
                }
                catch {
                    entry.Close();
                    pool.Free(entry);
                    throw;
                }
            }
            else {
                entry.Close();
                pool.Free(entry);
            }
        }

        protected override void EndInvoke(Stream istream, object context, bool success) {
            TcpConnEntry entry = context as TcpConnEntry;
            if (!success) {
                entry.Close();
            }
            pool.Free(entry);
        }

        protected override Stream GetOutputStream(object context) {
            TcpConnEntry entry = context as TcpConnEntry;
            if (entry.conn.client == null) {
                Uri uri = new Uri(entry.uri);
#if (dotNET10 || dotNETCF10)
                TcpClient client = new TcpClient(uri.Host, uri.Port);
#else
                TcpClient client;
                if (uri.Scheme == "tcp") {
                    client = new TcpClient(uri.Host, uri.Port);
                }
                else {
                    client = new TcpClient((uri.Scheme == "tcp6") ?
                                            AddressFamily.InterNetworkV6 :
                                            AddressFamily.InterNetwork);
                    client.Connect(uri.Host, uri.Port);
                }
#endif
                entry.Set(client);
            }
            return entry.conn.stream;
        }

        protected override Stream GetInputStream(object context) {
            return (context as TcpConnEntry).conn.stream;
        }

        protected override IAsyncResult BeginGetOutputStream(AsyncCallback callback, object context) {
            return getOutputStream.BeginInvoke(context, callback, context);
        }

        protected override Stream EndGetOutputStream(IAsyncResult asyncResult) {
            return getOutputStream.EndInvoke(asyncResult);
        }

        protected override IAsyncResult BeginGetInputStream(AsyncCallback callback, object context) {
            return getInputStream.BeginInvoke(context, callback, context);
        }

        protected override Stream EndGetInputStream(IAsyncResult asyncResult) {
            return getInputStream.EndInvoke(asyncResult);
        }
    }
}
#endif
