using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Reflection;
using System;
using Hprose.RPC;

namespace Hprose.UnitTests.RPC {
    [TestClass]
    public class TaskTests {
        public Task<int> Sum(int x, int y) {
            return Task<int>.Factory.StartNew(() => x + y);
        }
        [TestMethod]
        public async Task TestTask1() {
            Assert.AreEqual(3, await Sum(1, 2));
        }
        [TestMethod]
        public async Task TestTask2() {
            MethodInfo sum = GetType().GetMethod("Sum");
            Task result = (Task)sum.Invoke(this, new object[] { 1, 2 });
            Assert.AreEqual(3, (int)await TaskResult.Get(result));
        }
    }
}
