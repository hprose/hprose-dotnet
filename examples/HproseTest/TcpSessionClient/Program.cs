using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hprose.Common;
using Hprose.Client;

namespace TcpSessionClient {
    class MyClientFilter : IHproseFilter {
        private readonly HashMap<object, int> sessionIdMap = new HashMap<object, int>();
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
                sessionIdMap[context] = sid;
                byte[] buf = new byte[len];
                inStream.Read(buf, 0, len);
                return new MemoryStream(buf);
            }
            return inStream;
        }

        public MemoryStream OutputFilter(MemoryStream outStream, object context) {
            if (sessionIdMap.ContainsKey(context)) {
                int sid = sessionIdMap[context];
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
            return outStream;
        }
    }

    public interface IStub {
        [SimpleMode(true)]
        int Inc();
    }
    class Program {
        static void Main(string[] args) {
            HproseClient client = HproseClient.Create("tcp://127.0.0.1:4321");
            client.Filter = new MyClientFilter();
            IStub stub = client.UseService<IStub>();
            Console.WriteLine(stub.Inc());
            Console.WriteLine(stub.Inc());
            Console.WriteLine(stub.Inc());
            Console.WriteLine(stub.Inc());
            Console.ReadKey();
        }
    }
}
