using Hprose.RPC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading.Tasks;

namespace Hprose.UnitTests.RPC {
    [TestClass]
    public class HttpTest {
        public Task<int> Sum(int x, int y) {
            return Task<int>.Factory.StartNew(() => x + y);
        }
        public string Hello(string name) {
            return "Hello " + name;
        }
        [TestMethod]
        public async Task Test1() {
            HttpListener server = new HttpListener();
            server.Prefixes.Add("http://127.0.0.1:8080/");
            server.Start();
            var service = new Service();
            service.AddMethod("Hello", this);
            service.AddMethod("Sum", this);
            service.Bind(server);
            var client = new Client("http://127.0.0.1:8080/");
            var result = await client.Invoke<string>("hello", new object[] { "world" });
            Assert.AreEqual("Hello world", result);
            Assert.AreEqual(3, await client.Invoke<int>("sum", new object[] { 1, 2 }));
            server.Stop();
        }
        public interface ITestInterface {
            int Sum(int x, int y);
            Task<string> Hello(string name);
        }
        [TestMethod]
        public async Task Test2() {
            HttpListener server = new HttpListener();
            server.Prefixes.Add("http://127.0.0.1:8081/");
            server.Start();
            var service = new Service();
            service.AddMethod("Hello", this);
            service.AddMethod("Sum", this);
            service.Bind(server);
            var client = new Client("http://127.0.0.1:8081/");
            var proxy = client.UseService<ITestInterface>();
            var result = await proxy.Hello("world");
            Assert.AreEqual("Hello world", result);
            Assert.AreEqual(3, proxy.Sum(1, 2));
            server.Stop();
        }
    }
}
