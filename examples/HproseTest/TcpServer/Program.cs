using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hprose.Server;

namespace HproseTcpServerTest {
    class Program {
        public static string Hello(string name) {
            return "Hello " + name + "!";
        }
        static void Main(string[] args) {
            HproseTcpListenerServer tcpserver = new HproseTcpListenerServer("tcp4://127.0.0.1:4321/");
            tcpserver.Add("Hello", typeof(Program));
            tcpserver.Start();
            HproseHttpListenerServer httpserver = new HproseHttpListenerServer("http://localhost:8888/");
            httpserver.Add("Hello", typeof(Program));
            httpserver.Start();
            Console.ReadKey();
            tcpserver.Stop();
            httpserver.Stop();
        }
    }
}
