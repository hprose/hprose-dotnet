using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using Hprose.Collections.Generic;
using Hprose.IO.Serializers;

namespace Hprose.UnitTests.IO.Serializers {
    [TestClass]
    public class WriterTest {
        [TestMethod]
        public void TestSerialize() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(true);
                writer.Serialize(false);
                writer.Serialize('0');
                writer.Serialize('A');
                writer.Serialize('人');
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                Assert.AreEqual("ntfu0uAu人i123;i-123;", ValueWriter.UTF8.GetString(stream.ToArray()));
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
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var a = new byte[] {
                    1 + '0', 2 + '0', 3 + '0', 4 + '0', 5 + '0'
                };
                writer.Serialize(a);
                writer.Serialize(a);
                Assert.AreEqual("b5\"12345\"r0;", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var a = new char[] {
                    '1', '2', '3', '4', '5'
                };
                writer.Serialize(a);
                writer.Serialize(a);
                Assert.AreEqual("s5\"12345\"r0;", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize("");
                writer.Serialize("A");
                writer.Serialize("hello");
                Assert.AreEqual("euAs5\"hello\"", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var a = new NullableKey<int?>[] {
                    null,1,2,3,4,5
                };
                writer.Serialize(a);
                writer.Serialize(a);
                Assert.AreEqual("a6{n12345}r0;", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var a = new object[] {
                    null, 1,2,3,4,5, "hello"
                };
                writer.Serialize(a);
                writer.Serialize(a);
                Assert.AreEqual("a7{n12345s5\"hello\"}r0;", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var a = new List<int?> {
                    null, 1,2,3,4,5
                };
                writer.Serialize(a);
                writer.Serialize(a);
                Assert.AreEqual("a6{n12345}r0;", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
        }
    }
}
