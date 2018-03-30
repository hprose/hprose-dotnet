using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Hprose.Collections.Generic;
using Hprose.IO.Serializers;

namespace Hprose.UnitTests.IO.Serializers {
    [TestClass]
    public class WriterTest {
        [TestMethod]
        public void TestSerialize() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                //writer.Serialize(null);
                writer.Serialize(true);
                writer.Serialize(false);
                writer.Serialize('0');
                writer.Serialize('A');
                writer.Serialize('人');
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                Assert.AreEqual("tfu0uAu人i123;i-123;", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(Guid.Empty);
                writer.Serialize(Guid.Empty);
                Assert.AreEqual("g{" + Guid.Empty.ToString() + "}r0;", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                int? a = 1;
                writer.Serialize(a);
                a = null;
                writer.Serialize(a);
                Assert.AreEqual("1n", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                NullableKey<int?> a = 1;
                writer.Serialize(a);
                a = null;
                writer.Serialize(a);
                Assert.AreEqual("1n", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                string s = null;
                writer.Serialize(s);
                s = "hello";
                writer.Serialize(s);
                writer.Serialize(s);
                Assert.AreEqual("ns5\"hello\"r0;", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
        }
    }
}
