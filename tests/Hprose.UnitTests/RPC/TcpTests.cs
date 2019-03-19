using Hprose.RPC;
using Hprose.RPC.Codec.JSONRPC;
using Hprose.RPC.Plugins.Limiter;
using Hprose.RPC.Plugins.LoadBalance;
using Hprose.RPC.Plugins.Log;
using Hprose.RPC.Plugins.Push;
using Hprose.RPC.Plugins.Reverse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Hprose.UnitTests.RPC {
    [TestClass]
    public class TcpTest {
        [TestMethod]
        public void TestCRC32() {
            var data = Encoding.ASCII.GetBytes("abcdefg");
            Assert.AreEqual((uint)0x312A6AA6, CRC32.Compute(data));
            data = Encoding.ASCII.GetBytes("abcdefg12345678");
            Assert.AreEqual(0x9AE0DAAF, CRC32.Compute(data, 7, 8));
        }
        public Task<int> Sum(int x, int y) {
            return Task<int>.Factory.StartNew(() => x + y);
        }
        public string Hello(string name, Context context) {
            return "Hello " + name;
        }
        public void OnewayCall(string name) {
            Console.WriteLine(name);
        }
        [TestMethod]
        public async Task Test1() {
            IPAddress iPAddress = (await Dns.GetHostAddressesAsync("127.0.0.1"))[0];
            TcpListener server = new TcpListener(iPAddress, 8412);
            server.Start();
            var service = new Service();
            ServiceCodec.Instance.Debug = true;
            service.Use(Log.IOHandler)
                   .Use(Log.InvokeHandler)
                   .Add<string, Context, string>(Hello)
                   .Add<int, int, Task<int>>(Sum)
                   .Add(() => { return "good"; }, "Good")
                   .Bind(server);
            var client = new Client("tcp://127.0.0.1");
            var result = await client.InvokeAsync<string>("hello", new object[] { "world" });
            Assert.AreEqual("Hello world", result);
            Assert.AreEqual(3, await client.InvokeAsync<int>("sum", new object[] { 1, 2 }));
            Assert.AreEqual("good", await client.InvokeAsync<string>("good"));
            server.Stop();
        }

        public interface ITestInterface {
            int Sum(int x, int y);
            [Log(false)]
            Task<string> Hello(string name);
            [Name("OnewayCall")]
            Task OnewayCallAsync(string name);
            void OnewayCall(string name);
        }
        [TestMethod]
        public async Task Test2() {
            IPAddress iPAddress = (await Dns.GetHostAddressesAsync("127.0.0.1"))[0];
            TcpListener server = new TcpListener(iPAddress, 8413);
            server.Start();
            var service = new Service();
            service.AddMethod("Hello", this)
                   .AddMethod("Sum", this)
                   .Add<string>(OnewayCall)
                   .Bind(server);
            var client = new Client("tcp4://127.0.0.1:8413");
            var log = new Log();
            client.Use(log.IOHandler).Use(log.InvokeHandler);
            var proxy = client.UseService<ITestInterface>();
            var result = await proxy.Hello("world");
            Assert.AreEqual("Hello world", result);
            Assert.AreEqual(3, proxy.Sum(1, 2));
            proxy.OnewayCall("Oneway Sync");
            await proxy.OnewayCallAsync("Oneway Async");
            server.Stop();
        }
        [TestMethod]
        public async Task Test3() {
            IPAddress iPAddress = (await Dns.GetHostAddressesAsync("127.0.0.1"))[0];
            TcpListener server = new TcpListener(iPAddress, 8414);
            server.Start();
            var service = new Broker(new Service()).Service;
            ServiceCodec.Instance.Debug = true;
            service.Use(Log.IOHandler)
                   .Use(Log.InvokeHandler)
                   .Bind(server);

            var client1 = new Client("tcp4://127.0.0.1:8414");
            var prosumer1 = new Prosumer(client1, "1");
            prosumer1.OnSubscribe += (topic) => {
                Console.WriteLine(topic + " is subscribed.");
            };
            prosumer1.OnUnsubscribe += (topic) => {
                Console.WriteLine(topic + " is unsubscribed.");
            };
            var client2 = new Client("tcp4://127.0.0.1:8414");
            var prosumer2 = new Prosumer(client2, "2");
            await prosumer1.Subscribe<string>("test", (data, from) => {
                System.Console.WriteLine(data);
                Assert.AreEqual("hello", data);
                System.Console.WriteLine(from);
                Assert.AreEqual("2", from);
            });
            await prosumer1.Subscribe<string>("test2", (data) => {
                System.Console.WriteLine(data);
                Assert.AreEqual("world", data);
            });
            await prosumer1.Subscribe<Exception>("test3", (data) => {
                System.Console.WriteLine(data);
                Assert.AreEqual("error", data.Message);
            });
            var r1 = prosumer2.Push("hello", "test", "1");
            var r2 = prosumer2.Push("hello", "test", "1");
            var r3 = prosumer2.Push("world", "test2", "1");
            var r4 = prosumer2.Push("world", "test2", "1");
            var r5 = prosumer2.Push(new Exception("error"), "test3", "1");
            var r6 = prosumer2.Push(new Exception("error"), "test3", "1");
            await Task.WhenAll(r1, r2, r3, r4, r5, r6);
            await Task.Delay(10);
            await prosumer1.Unsubscribe("test");
            await prosumer1.Unsubscribe("test2");
            await prosumer1.Unsubscribe("test3");
            server.Stop();
        }
        public object Missing(string name, object[] args, Context context) {
            return name + ":" + args.Length + ":" + (context as dynamic).RemoteEndPoint.Address;
        }
        [TestMethod]
        public async Task Test4() {
            IPAddress iPAddress = (await Dns.GetHostAddressesAsync("127.0.0.1"))[0];
            TcpListener server = new TcpListener(iPAddress, 8415);
            server.Start();
            var service = new Service();
            service.AddMissingMethod(Missing).Bind(server);
            var client = new Client("tcp://127.0.0.1:8415");
            var log = new Log();
            client.Use(log.IOHandler).Use(log.InvokeHandler);
            var result = client.Invoke<string>("hello", new object[] { "world" });
            Assert.AreEqual("hello:1:127.0.0.1", result);
            result = client.Invoke<string>("sum", new object[] { 1, 2 });
            Assert.AreEqual("sum:2:127.0.0.1", result);
            server.Stop();
        }
        [TestMethod]
        public async Task Test5() {
            IPAddress iPAddress = (await Dns.GetHostAddressesAsync("127.0.0.1"))[0];
            TcpListener server = new TcpListener(iPAddress, 8416);
            server.Start();
            var log = new Log();
            ServiceCodec.Instance.Debug = true;
            var service = new Service();
            service
                .Use(log.InvokeHandler)
                .Use(log.IOHandler);
            var caller = new Caller(service);
            service.Bind(server);

            var client = new Client("tcp://127.0.0.1:8416");
            var provider = new Provider(client, "1") {
                Debug = true
            };
            var listening = provider.Add<string, Context, string>(Hello)
                   .Add<int, int, Task<int>>(Sum)
                   .Use(log.InvokeHandler)
                   .Listen();

            var proxy = caller.UseService<ITestInterface>("1");
            var result1 = proxy.Hello("world1");
            var result2 = proxy.Hello("world2");
            var result3 = proxy.Hello("world3");
            var r1 = caller.Invoke<string>("1", "hello", new object[] { "world1" });
            var r2 = caller.Invoke<string>("1", "hello", new object[] { "world2" });
            var r3 = caller.Invoke<string>("1", "hello", new object[] { "world3" });
            Assert.AreEqual(r1, "Hello world1");
            Assert.AreEqual(r2, "Hello world2");
            Assert.AreEqual(r3, "Hello world3");
            var results = await Task.WhenAll(result1, result2, result3);
            Assert.AreEqual(results[0], "Hello world1");
            Assert.AreEqual(results[1], "Hello world2");
            Assert.AreEqual(results[2], "Hello world3");
            await provider.Close();
            server.Stop();
        }
        [TestMethod]
        public async Task Test6() {
            IPAddress iPAddress = (await Dns.GetHostAddressesAsync("127.0.0.1"))[0];
            TcpListener server = new TcpListener(iPAddress, 8417);
            server.Start();
            var service = new Service {
                Codec = JsonRpcServiceCodec.Instance
            };
            service.AddMethod("Hello", this)
                   .AddMethod("Sum", this)
                   .Add<string>(OnewayCall)
                   .Bind(server);
            var client = new Client("tcp://127.0.0.1:8417") {
                Codec = JsonRpcClientCodec.Instance
            };
            var log = new Log();
            client.Use(log.IOHandler).Use(log.InvokeHandler);
            var proxy = client.UseService<ITestInterface>();
            var result = await proxy.Hello("world");
            Assert.AreEqual("Hello world", result);
            Assert.AreEqual(3, proxy.Sum(1, 2));
            proxy.OnewayCall("Oneway Sync");
            await proxy.OnewayCallAsync("Oneway Async");
            server.Stop();
        }
        [TestMethod]
        public async Task Test7() {
            IPAddress iPAddress = (await Dns.GetHostAddressesAsync("127.0.0.1"))[0];
            TcpListener server1 = new TcpListener(iPAddress, 8418);
            server1.Start();
            TcpListener server2 = new TcpListener(iPAddress, 8419);
            server2.Start();
            TcpListener server3 = new TcpListener(iPAddress, 8420);
            server3.Start();
            TcpListener server4 = new TcpListener(iPAddress, 8421);
            server4.Start();
            var service = new Service();
            service.AddMethod("Hello", this)
                   .AddMethod("Sum", this)
                   .Add<string>(OnewayCall)
                   .Bind(server1)
                   .Bind(server2)
                   .Bind(server3)
                   .Bind(server4);
            var client = new Client(/* "tcp4://127.0.0.1:8418" */);
            var lb = new NginxRoundRobinLoadBalance(new Dictionary<string, int>() {
                { "tcp4://127.0.0.1:8418", 1 },
                { "tcp4://127.0.0.1:8419", 2 },
                { "tcp4://127.0.0.1:8420", 3 },
                { "tcp4://127.0.0.1:8421", 4 }
            });
            client.Use(lb.Handler).Use(new ConcurrentLimiter(64).Handler).Use(new RateLimiter(50000).InvokeHandler);
            var proxy = client.UseService<ITestInterface>();
            var n = 1000;
            var tasks = new Task<string>[n];
            for (int i = 0; i < n; ++i) {
                tasks[i] = proxy.Hello("world" + i);
            }
            var results = await Task.WhenAll(tasks);
            for (int i = 0; i < n; ++i) {
                Assert.AreEqual(results[i], "Hello world" + i);
            }
            server1.Stop();
            server2.Stop();
            server3.Stop();
            server4.Stop();
        }
        [TestMethod]
        public async Task Test8() {
            IPAddress iPAddress = (await Dns.GetHostAddressesAsync("127.0.0.1"))[0];
            Socket server1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server1.Bind(new IPEndPoint(iPAddress, 8422));
            server1.Listen(128);
            Socket server2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server2.Bind(new IPEndPoint(iPAddress, 8423));
            server2.Listen(128);
            Socket server3 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server3.Bind(new IPEndPoint(iPAddress, 8424));
            server3.Listen(128);
            Socket server4 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server4.Bind(new IPEndPoint(iPAddress, 8425));
            server4.Listen(128);
            var service = new Service();
            service.AddMethod("Hello", this)
                   .AddMethod("Sum", this)
                   .Add<string>(OnewayCall)
                   .Bind(server1)
                   .Bind(server2)
                   .Bind(server3)
                   .Bind(server4);
            var client = new Client(/* "tcp4://127.0.0.1:8422" */);
            var lb = new NginxRoundRobinLoadBalance(new Dictionary<string, int>() {
                { "tcp4://127.0.0.1:8422", 1 },
                { "tcp4://127.0.0.1:8423", 2 },
                { "tcp4://127.0.0.1:8424", 3 },
                { "tcp4://127.0.0.1:8425", 4 }
            });
            client.Use(lb.Handler).Use(new ConcurrentLimiter(64).Handler).Use(new RateLimiter(50000).InvokeHandler);
            var proxy = client.UseService<ITestInterface>();
            var n = 1000;
            var tasks = new Task<string>[n];
            for (int i = 0; i < n; ++i) {
                tasks[i] = proxy.Hello("world" + i);
            }
            var results = await Task.WhenAll(tasks);
            for (int i = 0; i < n; ++i) {
                Assert.AreEqual(results[i], "Hello world" + i);
            }
            server1.Close();
            server2.Close();
            server3.Close();
            server4.Close();
        }
        [TestMethod]
        public async Task Test9() {
            IPAddress iPAddress = (await Dns.GetHostAddressesAsync("127.0.0.1"))[0];
            TcpListener server = new TcpListener(iPAddress, 8426);
            server.Start();
            var service = new Service();
            service.Add(
                (ServiceContext context) => context.RemoteEndPoint.ToString(),
                "getAddress"
            );
            service.Bind(server);
            var client = new Client("tcp4://127.0.0.1:8426");
            var log = new Log();
            client.Use(log.IOHandler).Use(log.InvokeHandler);
            Console.WriteLine(await client.InvokeAsync<string>("getAddress"));
            Console.WriteLine(await client.InvokeAsync<string>("getAddress"));
            await client.Abort();
            Console.WriteLine(await client.InvokeAsync<string>("getAddress"));
            Console.WriteLine(await client.InvokeAsync<string>("getAddress"));
            server.Stop();
        }
#if NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0
        [TestMethod]
        public async Task Test10() {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                Socket server = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
                server.Bind(new UnixDomainSocketEndPoint("/tmp/test"));
                server.Listen(128);
                var service = new Service();
                service.AddMethod("Hello", this)
                       .AddMethod("Sum", this)
                       .Add<string>(OnewayCall)
                       .Bind(server);
                var client = new Client("unix:///tmp/test");
                var proxy = client.UseService<ITestInterface>();
                var n = 100000;
                var tasks = new Task<string>[n];
                for (int i = 0; i < n; ++i) {
                    tasks[i] = proxy.Hello("world" + i);
                }
                var results = await Task.WhenAll(tasks);
                for (int i = 0; i < n; ++i) {
                    Assert.AreEqual(results[i], "Hello world" + i);
                }
                server.Close();
            }
        }
#endif
    }
}
