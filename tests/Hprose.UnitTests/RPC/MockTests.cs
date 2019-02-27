using Hprose.RPC;
using Hprose.RPC.Plugins.Limiter;
using Hprose.RPC.Plugins.Log;
using Hprose.RPC.Plugins.Push;
using Hprose.RPC.Plugins.Reverse;
using Hprose.RPC.Codec.JSONRPC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Threading.Tasks;
using Hprose.RPC.Plugins.LoadBalance;
using System.Collections.Generic;

namespace Hprose.UnitTests.RPC {
    [TestClass]
    public class MockTest {
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
            MockServer server = new MockServer("test1");
            var service = new Service();
            ServiceCodec.Instance.Debug = true;
            service.Use(Log.IOHandler)
                   .Use(Log.InvokeHandler)
                   .Add<string, Context, string>(Hello)
                   .Add<int, int, Task<int>>(Sum)
                   .Add(() => { return "good"; }, "Good")
                   .Bind(server);
            var client = new Client("mock://test1");
            var result = await client.InvokeAsync<string>("hello", new object[] { "world" });
            Assert.AreEqual("Hello world", result);
            Assert.AreEqual(3, await client.InvokeAsync<int>("sum", new object[] { 1, 2 }));
            Assert.AreEqual("good", await client.InvokeAsync<string>("good"));
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
            MockServer server = new MockServer("test2");
            var service = new Service();
            service.AddMethod("Hello", this)
                   .AddMethod("Sum", this)
                   .Add<string>(OnewayCall)
                   .Bind(server);
            var client = new Client("mock://test2");
            var log = new Log();
            client.Use(log.IOHandler).Use(log.InvokeHandler);
            var proxy = client.UseService<ITestInterface>();
            var result = await proxy.Hello("world");
            Assert.AreEqual("Hello world", result);
            Assert.AreEqual(3, proxy.Sum(1, 2));
            proxy.OnewayCall("Oneway Sync");
            await proxy.OnewayCallAsync("Oneway Async");
            server.Close();
        }
        [TestMethod]
        public async Task Test3() {
            MockServer server = new MockServer("test3");
            var service = new Broker(new Service()).Service;
            ServiceCodec.Instance.Debug = true;
            service.Use(Log.IOHandler)
                   .Use(Log.InvokeHandler)
                   .Bind(server);
            var client1 = new Client("mock://test3");
            var prosumer1 = new Prosumer(client1, "1") {
                OnSubscribe = (topic) => {
                    Console.WriteLine(topic + " is subscribed.");
                },
                OnUnsubscribe = (topic) => {
                    Console.WriteLine(topic + " is unsubscribed.");
                }
            };
            var client2 = new Client("mock://test3");
            var prosumer2 = new Prosumer(client2, "2");
            await prosumer1.Subscribe<string>("test", (data, from) => {
                Assert.AreEqual("hello", data);
                Assert.AreEqual("2", from);
            });
            await prosumer1.Subscribe<string>("test2", (data) => {
                Assert.AreEqual("world", data);
            });
            var r1 = prosumer2.Push("hello", "test", "1");
            var r2 = prosumer2.Push("hello", "test", "1");
            var r3 = prosumer2.Push("world", "test2", "1");
            var r4 = prosumer2.Push("world", "test2", "1");
            await Task.WhenAll(r1, r2, r3, r4);
            await Task.Delay(10);
            await prosumer1.Unsubscribe("test");
            await prosumer1.Unsubscribe("test2");
            server.Close();
        }
        public object Missing(string name, object[] args, Context context) {
            return name + ":" + args.Length + ":" + (context as dynamic).RemoteEndPoint.Address;
        }
        [TestMethod]
        public async Task Test4() {
            MockServer server = new MockServer("127.0.0.1");
            var service = new Service();
            service.AddMissingMethod(Missing).Bind(server);
            var client = new Client("mock://127.0.0.1");
            var log = new Log();
            client.Use(log.IOHandler).Use(log.InvokeHandler);
            var result = await client.InvokeAsync<string>("hello", new object[] { "world" });
            Assert.AreEqual("hello:1:127.0.0.1", result);
            result = await client.InvokeAsync<string>("sum", new object[] { 1, 2 });
            Assert.AreEqual("sum:2:127.0.0.1", result);
            server.Close();
        }
        [TestMethod]
        public async Task Test5() {
            MockServer server = new MockServer("test5");
            var log = new Log();
            ServiceCodec.Instance.Debug = true;
            var service = new Service();
            service
                .Use(log.InvokeHandler)
                .Use(log.IOHandler);
            var caller = new Caller(service);
            service.Bind(server);
            var client = new Client("mock://test5");
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
            server.Close();
        }
        [TestMethod]
        public async Task Test6() {
            MockServer server = new MockServer("test6");
            var service = new Service {
                Codec = JsonRpcServiceCodec.Instance
            };
            service.AddMethod("Hello", this)
                   .AddMethod("Sum", this)
                   .Add<string>(OnewayCall)
                   .Bind(server);
            var client = new Client("mock://test6") {
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
            server.Close();
        }
        [TestMethod]
        public async Task Test7() {
            MockServer server1 = new MockServer("testlb1");
            MockServer server2 = new MockServer("testlb2");
            MockServer server3 = new MockServer("testlb3");
            MockServer server4 = new MockServer("testlb4");
            var service = new Service();
            service.AddMethod("Hello", this)
                   .AddMethod("Sum", this)
                   .Add<string>(OnewayCall)
                   .Bind(server1)
                   .Bind(server2)
                   .Bind(server3)
                   .Bind(server4);

            var client = new Client(/* "mock://testlb1" */);
            var lb = new WeightedRoundRobinLoadBalance(new Dictionary<string, int>() {
                { "mock://testlb1", 1 },
                { "mock://testlb2", 2 },
                { "mock://testlb3", 3 },
                { "mock://testlb4", 4 }
            });
            client.Use(lb.Handler).Use(new ConcurrentLimiter(64).Handler);
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
        public async Task Test8() {
            MockServer server = new MockServer("test8");
            var service = new Service();
            service.Add(
                (ServiceContext context) => context.RemoteEndPoint.ToString(),
                "getAddress"
            );
            service.Bind(server);
            var client = new Client("mock://test8");
            var log = new Log();
            client.Use(log.IOHandler).Use(log.InvokeHandler);
            Console.WriteLine(await client.InvokeAsync<string>("getAddress"));
            Console.WriteLine(await client.InvokeAsync<string>("getAddress"));
            await client.Abort();
            Console.WriteLine(await client.InvokeAsync<string>("getAddress"));
            Console.WriteLine(await client.InvokeAsync<string>("getAddress"));
            server.Close();
        }
    }
}
