using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hprose.Common;
using Hprose.Server;

namespace TcpSessionServer {
    class Session {
        internal static HashMap<object, int> sidMap = new HashMap<object, int>();
        internal static List<Dictionary<string, dynamic>> sessions = new List<Dictionary<string, dynamic>>();
        public static Dictionary<string, dynamic> GetSession(object context) {
            return sessions[sidMap[context]];
        }
    }

    class MyServerFilter : IHproseFilter {
        public MemoryStream InputFilter(MemoryStream inStream, object context) {
            int len = (int)inStream.Length - 7;
            if (len > 0 &&
                inStream.ReadByte() == 's' &&
                inStream.ReadByte() == 'i' &&
                inStream.ReadByte() == 'd') {
                int sid = inStream.ReadByte() << 24 |
                          inStream.ReadByte() << 16 |
                          inStream.ReadByte() << 8 |
                          inStream.ReadByte();
                Session.sidMap[context] = sid;
                byte[] buf = new byte[len];
                inStream.Read(buf, 0, len);
                return new MemoryStream(buf);
            }
            else {
                int sid = Session.sessions.Count;
                Session.sidMap[context] = sid;
                Session.sessions.Add(new Dictionary<string, dynamic>());
            }
            return inStream;
        }

        public MemoryStream OutputFilter(MemoryStream outStream, object context) {
            int sid = Session.sidMap[context];
            byte[] buf = new byte[outStream.Length + 7];
            buf[0] = (byte)'s';
            buf[1] = (byte)'i';
            buf[2] = (byte)'d';
            buf[3] = (byte)(sid >> 24 & 0xff);
            buf[4] = (byte)(sid >> 16 & 0xff);
            buf[5] = (byte)(sid >> 8 & 0xff);
            buf[6] = (byte)(sid & 0xff);
            outStream.Read(buf, 7, (int)outStream.Length);
            return new MemoryStream(buf);
        }
    }

    class Program {
        public static int Inc() {
            var session = Session.GetSession(HproseService.CurrentContext);
            if (!session.ContainsKey("n")) {
                session["n"] = 0;
                return 0;
            }
            int i = session["n"] + 1;
            session["n"] = i;
            return i;
        }
        static void Main(string[] args) {
            HproseTcpListenerServer tcpserver = new HproseTcpListenerServer("tcp4://127.0.0.1:4321/");
            tcpserver.Filter = new MyServerFilter();
            tcpserver.Add("Inc", typeof(Program));
            tcpserver.Start();
            Console.ReadKey();
            tcpserver.Stop();

        }
    }
}
