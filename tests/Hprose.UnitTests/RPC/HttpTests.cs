using Hprose.IO;
using Hprose.RPC;
using Hprose.RPC.Plugins.Log;
using Hprose.RPC.Plugins.Push;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Hprose.UnitTests.RPC {
    [TestClass]
    public class HttpTest {
        public Task<int> Sum(int x, int y) {
            return Task<int>.Factory.StartNew(() => x + y);
        }
        public string Hello(string name, Context context) {
            System.Console.WriteLine((context as ServiceContext).Method.Fullname);
            System.Console.WriteLine((context as dynamic).Request.RemoteEndPoint.Address);
            return "Hello " + name;
        }
        [TestMethod]
        public async Task Test1() {
            HttpListener server = new HttpListener();
            server.Prefixes.Add("http://127.0.0.1:8080/");
            server.Start();
            var service = new Service();
            ServiceCodec.Instance.Debug = true;
            service.Use(Log.IOHandler)
                   .Use(Log.InvokeHandler)
                   .Add<string, Context, string>(Hello)
                   .Add<int, int, Task<int>>(Sum)
                   .Add(() => { return "good"; }, "Good")
                   .Bind(server);
            var client = new Client("http://127.0.0.1:8080/");
            var result = await client.Invoke<string>("hello", new object[] { "world" });
            Assert.AreEqual("Hello world", result);
            Assert.AreEqual(3, await client.Invoke<int>("sum", new object[] { 1, 2 }));
            Assert.AreEqual("good", await client.Invoke<string>("good"));
            server.Stop();
        }
        public interface ITestInterface {
            int Sum(int x, int y);
            [Log(false)]
            Task<string> Hello(string name);
        }
        [TestMethod]
        public async Task Test2() {
            HttpListener server = new HttpListener();
            server.Prefixes.Add("http://127.0.0.1:8081/");
            server.Start();
            var service = new Service();
            service.AddMethod("Hello", this)
                   .AddMethod("Sum", this)
                   .Bind(server, "http");
            var client = new Client("http://127.0.0.1:8081/");
            ((ClientCodec)(client.Codec)).Simple = true;
            var log = new Log();
            client.Use(log.IOHandler).Use(log.InvokeHandler);
            var proxy = client.UseService<ITestInterface>();
            var result = await proxy.Hello("world");
            Assert.AreEqual("Hello world", result);
            Assert.AreEqual(3, proxy.Sum(1, 2));
            server.Stop();
        }
        [TestMethod]
        public async Task Test3() {
            HttpListener server = new HttpListener();
            server.Prefixes.Add("http://127.0.0.1:8082/");
            server.Start();
            var service = new Broker(new Service()).Service;
            ServiceCodec.Instance.Debug = true;
            service.Use(Log.IOHandler)
                   .Use(Log.InvokeHandler)
                   .Bind(server);

            var client1 = new Client("http://127.0.0.1:8082/");
            var prosumer1 = new Prosumer(client1, "1");
            prosumer1.OnSubscribe = (topic) => {
                Console.WriteLine(topic + " is subscribed.");
            };
            prosumer1.OnUnsubscribe = (topic) => {
                Console.WriteLine(topic + " is unsubscribed.");
            };
            var client2 = new Client("http://127.0.0.1:8082/");
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
            await Task.Delay(100);
            await prosumer1.Unsubscribe("test");
            await prosumer1.Unsubscribe("test2");
            server.Stop();
        }
    }
}
