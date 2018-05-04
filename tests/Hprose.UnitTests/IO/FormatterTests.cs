using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization;

using Hprose.Collections.Generic;
using Hprose.IO;
using Hprose.IO.Serializers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hprose.UnitTests.IO {
    [TestClass]
    public class FormatterTests {
        [TestMethod]
        public void TestSerializeBasic() {
            using (MemoryStream stream = new MemoryStream()) {
                HproseFormatter.Serialize(null, stream);
                HproseFormatter.Serialize(true, stream);
                HproseFormatter.Serialize(false, stream);
                HproseFormatter.Serialize('0', stream);
                HproseFormatter.Serialize('A', stream);
                HproseFormatter.Serialize('人', stream);
                HproseFormatter.Serialize((byte)123, stream);
                HproseFormatter.Serialize((sbyte)-123, stream);
                Assert.AreEqual("ntfu0uAu人i123;i-123;", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
        }
    }
}
