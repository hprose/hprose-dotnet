using Hprose.IO;
using Hprose.RPC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace Hprose.UnitTests.IO {
    [TestClass]
    public class FormatterTests {
        [TestMethod]
        public void TestSerializeBasic() {
            using (MemoryStream stream = new MemoryStream()) {
                Formatter.Serialize(null, stream);
                Formatter.Serialize(true, stream);
                Formatter.Serialize(false, stream);
                Formatter.Serialize('0', stream);
                Formatter.Serialize('A', stream);
                Formatter.Serialize('人', stream);
                Formatter.Serialize((byte)123, stream);
                Formatter.Serialize((sbyte)-123, stream);
                var data = stream.GetArraySegment();
                Assert.AreEqual("ntfu0uAu人i123;i-123;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
        }
    }
}
