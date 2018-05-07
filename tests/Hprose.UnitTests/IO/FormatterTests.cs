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
                Hprose.IO.Formatter.Serialize(null, stream);
                Hprose.IO.Formatter.Serialize(true, stream);
                Hprose.IO.Formatter.Serialize(false, stream);
                Hprose.IO.Formatter.Serialize('0', stream);
                Hprose.IO.Formatter.Serialize('A', stream);
                Hprose.IO.Formatter.Serialize('人', stream);
                Hprose.IO.Formatter.Serialize((byte)123, stream);
                Hprose.IO.Formatter.Serialize((sbyte)-123, stream);
                Assert.AreEqual("ntfu0uAu人i123;i-123;", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
        }
    }
}
