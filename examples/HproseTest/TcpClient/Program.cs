using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hprose.Client;
using System.Diagnostics;

namespace HproseTcpClientTest
{
    public interface IHello
    {
        string Hello(string name);
    }
    class Program
    {
        static IHello hello;

        static void Test(string data) {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 1000; i++) {
                hello.Hello(data);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }
        static void Main(string[] args)
        {

            HproseClient client = HproseClient.Create("tcp4://127.0.0.1:4321/");
            hello = client.UseService<IHello>();
            Console.WriteLine("TCP");
            Console.WriteLine(hello.Hello("World"));
            Test("World");
            Test("".PadRight(512, '$'));
            Test("".PadRight(1024, '$'));
            Test("".PadRight(2 * 1024, '$'));
            Test("".PadRight(4 * 1024, '$'));
            Test("".PadRight(8 * 1024, '$'));
            Test("".PadRight(16 * 1024, '$'));
            Test("".PadRight(32 * 1024, '$'));
            Test("".PadRight(64 * 1024, '$'));

            client = HproseClient.Create("http://localhost:8888/");
            hello = client.UseService<IHello>();
            Console.WriteLine("HTTP");
            Console.WriteLine(hello.Hello("World"));
            Test("World");
            Test("".PadRight(512, '$'));
            Test("".PadRight(1024, '$'));
            Test("".PadRight(2 * 1024, '$'));
            Test("".PadRight(4 * 1024, '$'));
            Test("".PadRight(8 * 1024, '$'));
            Test("".PadRight(16 * 1024, '$'));
            Test("".PadRight(32 * 1024, '$'));
            Test("".PadRight(64 * 1024, '$'));
            Console.ReadKey();

//            client.Invoke<string>("hello", new Object[] { "Async World" }, result => Console.WriteLine(result));

        }
    }
}
