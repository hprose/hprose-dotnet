using Hprose.RPC;
using Hprose.RPC.Codec.JSONRPC;
using Hprose.RPC.Plugins.Limiter;
using Hprose.RPC.Plugins.Log;
using Hprose.RPC.Plugins.Push;
using Hprose.RPC.Plugins.Reverse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Hprose.RPC.Plugins.LoadBalance;
using System.Collections.Generic;

namespace Hprose.UnitTests.RPC {
    [TestClass]
    public class UdpTests {
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
        public async Task Test0() {
            var client = new Client("udp://127.0.0.1") {
                Timeout = TimeSpan.FromMilliseconds(100)
            };
            await Assert.ThrowsExceptionAsync<SocketException>(async () => {
                var result = await client.InvokeAsync<string>("hello", new object[] { "world" }).ConfigureAwait(false);
                Console.WriteLine(result);
            });
        }
        [TestMethod]
        public async Task Test1() {
            UdpClient server = new UdpClient(8412);
            var service = new Service();
            ServiceCodec.Instance.Debug = true;
            service.Use(Log.IOHandler)
                   .Use(Log.InvokeHandler)
                   .Add<string, Context, string>(Hello)
                   .Add<int, int, Task<int>>(Sum)
                   .Add(() => { return "good"; }, "Good")
                   .Bind(server);
            var client = new Client("udp://127.0.0.1");
            var result = await client.InvokeAsync<string>("hello", new object[] { "world" }).ConfigureAwait(false);
            Assert.AreEqual("Hello world", result);
            Assert.AreEqual(3, await client.InvokeAsync<int>("sum", new object[] { 1, 2 }).ConfigureAwait(false));
            Assert.AreEqual("good", await client.InvokeAsync<string>("good").ConfigureAwait(false));
            server.Close();
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
            UdpClient server = new UdpClient(8413);
            var service = new Service();
            service.AddMethod("Hello", this)
                   .AddMethod("Sum", this)
                   .Add<string>(OnewayCall)
                   .Bind(server);
            var client = new Client("udp4://127.0.0.1:8413");
            var log = new Log();
            client.Use(log.IOHandler).Use(log.InvokeHandler);
            var proxy = client.UseService<ITestInterface>();
            var result = await proxy.Hello("world").ConfigureAwait(false);
            Assert.AreEqual("Hello world", result);
            Assert.AreEqual(3, proxy.Sum(1, 2));
            proxy.OnewayCall("Oneway Sync");
            await proxy.OnewayCallAsync("Oneway Async").ConfigureAwait(false);
            server.Close();
        }
        [TestMethod]
        public async Task Test3() {
            UdpClient server = new UdpClient(8414);
            var service = new Broker(new Service()).Service;
            ServiceCodec.Instance.Debug = true;
            service.Use(Log.IOHandler)
                   .Use(Log.InvokeHandler)
                   .Bind(server);

            var client1 = new Client("udp://127.0.0.1:8414");
            var prosumer1 = new Prosumer(client1, "1");
            prosumer1.OnSubscribe += (topic) => {
                Console.WriteLine(topic + " is subscribed.");
            };
            prosumer1.OnUnsubscribe += (topic) => {
                Console.WriteLine(topic + " is unsubscribed.");
            };
            var client2 = new Client("udp://127.0.0.1:8414");
            var prosumer2 = new Prosumer(client2, "2");
            await prosumer1.Subscribe<string>("test", (data, from) => {
                System.Console.WriteLine(data);
                Assert.AreEqual("hello", data);
                System.Console.WriteLine(from);
                Assert.AreEqual("2", from);
            }).ConfigureAwait(false);
            await prosumer1.Subscribe<string>("test2", (data) => {
                System.Console.WriteLine(data);
                Assert.AreEqual("world", data);
            }).ConfigureAwait(false);
            await prosumer1.Subscribe<Exception>("test3", (data) => {
                System.Console.WriteLine(data);
                Assert.AreEqual("error", data.Message);
            }).ConfigureAwait(false);
            var r1 = prosumer2.Push("hello", "test", "1");
            var r2 = prosumer2.Push("hello", "test", "1");
            var r3 = prosumer2.Push("world", "test2", "1");
            var r4 = prosumer2.Push("world", "test2", "1");
            var r5 = prosumer2.Push(new Exception("error"), "test3", "1");
            var r6 = prosumer2.Push(new Exception("error"), "test3", "1");
            await Task.WhenAll(r1, r2, r3, r4, r5, r6).ConfigureAwait(false);
            await Task.Delay(10).ConfigureAwait(false);
            await prosumer1.Unsubscribe("test").ConfigureAwait(false);
            await prosumer1.Unsubscribe("test2").ConfigureAwait(false);
            await prosumer1.Unsubscribe("test3").ConfigureAwait(false);
            server.Close();
        }
        public object Missing(string name, object[] args, Context context) {
            return name + ":" + args.Length + ":" + (context as dynamic).RemoteEndPoint.Address;
        }
        [TestMethod]
        public void Test4() {
            UdpClient server = new UdpClient(8415);
            var service = new Service();
            service.AddMissingMethod(Missing).Bind(server);
            var client = new Client("udp://127.0.0.1:8415");
            var log = new Log();
            client.Use(log.IOHandler).Use(log.InvokeHandler);
            var result = client.Invoke<string>("hello", new object[] { "world" });
            Assert.AreEqual("hello:1:127.0.0.1", result);
            result = client.Invoke<string>("sum", new object[] { 1, 2 });
            Assert.AreEqual("sum:2:127.0.0.1", result);
            server.Close();
        }
        [TestMethod]
        public async Task Test5() {
            UdpClient server = new UdpClient(8416);
            var log = new Log();
            ServiceCodec.Instance.Debug = true;
            var service = new Service();
            service
                .Use(log.InvokeHandler)
                .Use(log.IOHandler);
            var caller = new Caller(service);
            service.Bind(server);

            var client = new Client("udp://127.0.0.1:8416");
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
            var results = await Task.WhenAll(result1, result2, result3).ConfigureAwait(false);
            Assert.AreEqual(results[0], "Hello world1");
            Assert.AreEqual(results[1], "Hello world2");
            Assert.AreEqual(results[2], "Hello world3");
            await provider.Close().ConfigureAwait(false);
            server.Close();
        }
        [TestMethod]
        public async Task Test6() {
            UdpClient server = new UdpClient(8417);
            var service = new Service {
                Codec = JsonRpcServiceCodec.Instance
            };
            service.AddMethod("Hello", this)
                   .AddMethod("Sum", this)
                   .Add<string>(OnewayCall)
                   .Bind(server);
            var client = new Client("udp://127.0.0.1:8417") {
                Codec = JsonRpcClientCodec.Instance
            };
            var log = new Log();
            client.Use(log.IOHandler).Use(log.InvokeHandler);
            var proxy = client.UseService<ITestInterface>();
            var result = await proxy.Hello("world").ConfigureAwait(false);
            Assert.AreEqual("Hello world", result);
            Assert.AreEqual(3, proxy.Sum(1, 2));
            proxy.OnewayCall("Oneway Sync");
            await proxy.OnewayCallAsync("Oneway Async").ConfigureAwait(false);
            server.Close();
        }
        [TestMethod]
        public async Task Test7() {
            UdpClient server1 = new UdpClient(8418);
            UdpClient server2 = new UdpClient(8419);
            UdpClient server3 = new UdpClient(8420);
            UdpClient server4 = new UdpClient(8421);
            var service = new Service();
            service.AddMethod("Hello", this)
                   .AddMethod("Sum", this)
                   .Add<string>(OnewayCall)
                   .Bind(server1)
                   .Bind(server2)
                   .Bind(server3)
                   .Bind(server4);
            var client = new Client(/* "udp4://127.0.0.1:8418" */);
            var lb = new NginxRoundRobinLoadBalance(new Dictionary<string, int>() {
                { "udp4://127.0.0.1:8418", 1 },
                { "udp4://127.0.0.1:8419", 2 },
                { "udp4://127.0.0.1:8420", 3 },
                { "udp4://127.0.0.1:8421", 4 }
            });
            client.Use(lb.Handler).Use(new ConcurrentLimiter(64).Handler).Use(new RateLimiter(50000).InvokeHandler);
            var proxy = client.UseService<ITestInterface>();
            var n = 1000;
            var tasks = new Task<string>[n];
            for (int i = 0; i < n; ++i) {
                tasks[i] = proxy.Hello("world" + i);
            }
            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
            for (int i = 0; i < n; ++i) {
                Assert.AreEqual(results[i], "Hello world" + i);
            }
            server1.Close();
            server2.Close();
            server3.Close();
            server4.Close();
        }
        [TestMethod]
        public async Task Test8() {
            UdpClient server = new UdpClient(8422);
            var service = new Service();
            service.Add(
                (ServiceContext context) => context.RemoteEndPoint.ToString(),
                "getAddress"
            );
            service.Bind(server);
            var client = new Client("udp4://127.0.0.1:8422");
            var log = new Log();
            client.Use(log.IOHandler).Use(log.InvokeHandler);
            Console.WriteLine(await client.InvokeAsync<string>("getAddress").ConfigureAwait(false));
            Console.WriteLine(await client.InvokeAsync<string>("getAddress").ConfigureAwait(false));
            await client.Abort().ConfigureAwait(false);
            Console.WriteLine(await client.InvokeAsync<string>("getAddress").ConfigureAwait(false));
            Console.WriteLine(await client.InvokeAsync<string>("getAddress").ConfigureAwait(false));
            server.Close();
        }
    }
}
