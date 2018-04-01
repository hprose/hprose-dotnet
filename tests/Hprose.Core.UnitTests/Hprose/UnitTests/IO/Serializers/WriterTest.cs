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
        public void TestSerializeBasic() {
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
        }
        [TestMethod]
        public void TestSerializeGuid() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(Guid.Empty);
                writer.Serialize(Guid.Empty);
                Assert.AreEqual("g{" + Guid.Empty.ToString() + "}r0;", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
        }
        [TestMethod]
        public void TestSerializeNullable() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                int? a = 1;
                writer.Serialize(a);
                a = null;
                writer.Serialize(a);
                Assert.AreEqual("1n", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
        }
        [TestMethod]
        public void TestSerializeNullableKey() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                NullableKey<int?> a = 1;
                writer.Serialize(a);
                a = null;
                writer.Serialize(a);
                Assert.AreEqual("1n", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
        }
        [TestMethod]
        public void TestSerializeString() {
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
                writer.Serialize("");
                writer.Serialize("A");
                writer.Serialize("hello");
                Assert.AreEqual("euAs5\"hello\"", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
        }
        [TestMethod]
        public void TestSerializeBytes() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var a = new byte[] {
                    1 + '0', 2 + '0', 3 + '0', 4 + '0', 5 + '0'
                };
                writer.Serialize(a);
                writer.Serialize(a);
                Assert.AreEqual("b5\"12345\"r0;", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
        }
        [TestMethod]
        public void TestSerializeChars() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var a = new char[] {
                    '1', '2', '3', '4', '5'
                };
                writer.Serialize(a);
                writer.Serialize(a);
                Assert.AreEqual("s5\"12345\"r0;", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
        }
        [TestMethod]
        public void TestSerializeArray() {
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
        }
        [TestMethod]
        public void TestSerializeList() {
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
        [TestMethod]
        public void TestSerializeCollection() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                ICollection<int?> a = new List<int?> {
                    null, 1,2,3,4,5
                };
                writer.Serialize(a);
                writer.Serialize(a);
                Assert.AreEqual("a6{n12345}r0;", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
        }
        [TestMethod]
        public void TestSerializeDictionary() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var dict = new Dictionary<NullableKey<int?>, int> {
                    { null, 1 },
                    { 2, 3 },
                    { 4, 5 }
                };
                writer.Serialize(dict);
                writer.Serialize(dict);
                Assert.AreEqual("m3{n12345}r0;", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                ICollection<KeyValuePair<NullableKey<int?>, int>> dict = new Dictionary<NullableKey<int?>, int> {
                    { null, 1 },
                    { 2, 3 },
                    { 4, 5 }
                };
                writer.Serialize(dict);
                writer.Serialize(dict);
                Assert.AreEqual("m3{n12345}r0;", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
        }
        [TestMethod]
        public void TestSerializeEnum() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(TypeCode.Boolean);
                writer.Serialize(TypeCode.Byte);
                Assert.AreEqual("36", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
        }
        [TestMethod]
        public void TestSerializeValueTuple() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(ValueTuple.Create());
                writer.Serialize(ValueTuple.Create(1, "hello"));
                writer.Serialize(ValueTuple.Create(1, 2, "hello"));
                writer.Serialize(ValueTuple.Create(1, 2, 3, "hello"));
                writer.Serialize(ValueTuple.Create(1, 2, 3, 4, "hello"));
                writer.Serialize(ValueTuple.Create(1, 2, 3, 4, 5, "hello"));
                writer.Serialize(ValueTuple.Create(1, 2, 3, 4, 5, 6, "hello"));
                var a = (1, 2, 3, 4, 5, 6, 7, "hello");
                writer.Serialize(a);
                writer.Serialize(a);
                var b = (1, 2, 3, 4, 5, 6, 7, 8, "hello");
                writer.Serialize(b);
                writer.Serialize(b);
                var c = (1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0);
                writer.Serialize(c);
                Assert.AreEqual("a{}a2{1s5\"hello\"}a3{12r2;}a4{123r2;}a5{1234r2;}a6{12345r2;}a7{123456r2;}a8{1234567r2;}r8;a9{12345678r2;}r9;a30{123456789012345678901234567890}", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(ValueTuple.Create(ValueTuple.Create(), ValueTuple.Create()));
                Assert.AreEqual("a2{a{}r1;}", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var e = ValueTuple.Create();
                var a = (1, 2, 3, 4, 5, 6, 7, e);
                writer.Serialize(a);
                Assert.AreEqual("a8{1234567a{}}", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
        }
        [TestMethod]
        public void TestSerializeTuple() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(Tuple.Create(1));
                writer.Serialize(Tuple.Create(1, "hello"));
                writer.Serialize(Tuple.Create(1, 2, "hello"));
                writer.Serialize(Tuple.Create(1, 2, 3, "hello"));
                writer.Serialize(Tuple.Create(1, 2, 3, 4, "hello"));
                writer.Serialize(Tuple.Create(1, 2, 3, 4, 5, "hello"));
                writer.Serialize(Tuple.Create(1, 2, 3, 4, 5, 6, "hello"));
                var a = Tuple.Create(1, 2, 3, 4, 5, 6, 7, "hello");
                writer.Serialize(a);
                writer.Serialize(a);
                var b = TupleExtensions.ToTuple((1, 2, 3, 4, 5, 6, 7, 8, "hello"));
                writer.Serialize(b);
                writer.Serialize(b);
                var c = TupleExtensions.ToTuple((1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0));
                writer.Serialize(c);
                Assert.AreEqual("a1{1}a2{1s5\"hello\"}a3{12r2;}a4{123r2;}a5{1234r2;}a6{12345r2;}a7{123456r2;}a8{1234567r2;}r8;a9{12345678r2;}r9;a20{12345678901234567890}", ValueWriter.UTF8.GetString(stream.ToArray()));
            }
        }
    }
}
