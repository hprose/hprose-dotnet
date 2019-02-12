using Hprose.RPC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace Hprose.UnitTests.IO {
    [TestClass]
    public class MemoryStreamTests {
        [TestMethod]
        public void TestGetArraySegment() {
            var bytes = Encoding.UTF8.GetBytes("Hello World!");
            var stream = new MemoryStream(bytes, 6, 5, false, true);
            var result = stream.GetArraySegment();
            for (int i = 0; i < 100000; ++i) {
                result = stream.GetArraySegment();
            }
            Assert.AreEqual("World", Encoding.UTF8.GetString(result.Array, result.Offset, result.Count));
        }
        [TestMethod]
        public void TestToArray() {
            var bytes = Encoding.UTF8.GetBytes("Hello World!");
            var stream = new MemoryStream(bytes, 6, 5, false, true);
            var result = stream.ToArray();
            for (int i = 0; i < 100000; ++i) {
                result = stream.ToArray();
            }
            Assert.AreEqual("World", Encoding.UTF8.GetString(result));
        }
    }
}
