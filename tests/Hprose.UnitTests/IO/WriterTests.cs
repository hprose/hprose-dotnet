using Hprose.Collections.Generic;
using Hprose.IO;
using Hprose.RPC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace Hprose.UnitTests.IO {
    [TestClass]
    public class WriterTests {
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
                var data = stream.GetArraySegment();
                Assert.AreEqual("ntfu0uAu人i123;i-123;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
        }
        [TestMethod]
        public void TestSerializeDateTime() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                DateTime date = new DateTime(2018, 4, 10);
                writer.Serialize(date);
                writer.Serialize(date);
                var data = stream.GetArraySegment();
                Assert.AreEqual("D20180410;r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                DateTime date = new DateTime(2018, 4, 10, 13, 31, 34, 567);
                writer.Serialize(date);
                writer.Serialize(date);
                var data = stream.GetArraySegment();
                Assert.AreEqual("D20180410T133134.567;r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                DateTime date = new DateTime(1970, 1, 1, 13, 31, 34, 567);
                writer.Serialize(date);
                writer.Serialize(date);
                var data = stream.GetArraySegment();
                Assert.AreEqual("T133134.567;r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                DateTime date = new DateTime(123456789);
                writer.Serialize(date);
                writer.Serialize(date);
                var data = stream.GetArraySegment();
                Assert.AreEqual("D00010101T000012.345678900;r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
        }
        [TestMethod]
        public void TestSerializeGuid() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(Guid.Empty);
                writer.Serialize(Guid.Empty);
                var data = stream.GetArraySegment();
                Assert.AreEqual("g{" + Guid.Empty.ToString() + "}r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
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
                var data = stream.GetArraySegment();
                Assert.AreEqual("1n", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
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
                var data = stream.GetArraySegment();
                Assert.AreEqual("1n", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
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
                writer.Serialize("🥁🐎🍄");
                var data = stream.GetArraySegment();
                Assert.AreEqual("ns5\"hello\"r0;s6\"🥁🐎🍄\"", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize("");
                writer.Serialize("A");
                writer.Serialize("hello");
                var data = stream.GetArraySegment();
                Assert.AreEqual("euAs5\"hello\"", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
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
                var data = stream.GetArraySegment();
                Assert.AreEqual("b5\"12345\"r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
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
                var data = stream.GetArraySegment();
                Assert.AreEqual("s5\"12345\"r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
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
                var data = stream.GetArraySegment();
                Assert.AreEqual("a6{n12345}r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var a = new object[] {
                    null, 1,2,3,4,5, "hello"
                };
                writer.Serialize(a);
                writer.Serialize(a);
                var data = stream.GetArraySegment();
                Assert.AreEqual("a7{n12345s5\"hello\"}r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
        }
        [TestMethod]
        public void TestSerializeArraySegment() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var a = new ArraySegment<int>(new int[] { 1,2,3,4,5 });
                writer.Serialize(a);
                writer.Serialize(a);
                writer.Serialize(new ArraySegment<int>(a.Array, 3, 2));
                var data = stream.GetArraySegment();
                Assert.AreEqual("a5{12345}r0;a2{45}", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
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
                var data = stream.GetArraySegment();
                Assert.AreEqual("a6{n12345}r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
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
                var data = stream.GetArraySegment();
                Assert.AreEqual("a6{n12345}r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
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
                var data = stream.GetArraySegment();
                Assert.AreEqual("m3{n12345}r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
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
                var data = stream.GetArraySegment();
                Assert.AreEqual("m3{n12345}r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
        }
        [TestMethod]
        public void TestSerializeEnum() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(TypeCode.Boolean);
                writer.Serialize(TypeCode.Byte);
                var data = stream.GetArraySegment();
                Assert.AreEqual("36", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
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
                var data = stream.GetArraySegment();
                Assert.AreEqual("a{}a2{1s5\"hello\"}a3{12r2;}a4{123r2;}a5{1234r2;}a6{12345r2;}a7{123456r2;}a8{1234567r2;}r8;a9{12345678r2;}r9;a30{123456789012345678901234567890}", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(ValueTuple.Create(ValueTuple.Create(), ValueTuple.Create()));
                var data = stream.GetArraySegment();
                Assert.AreEqual("a2{a{}r1;}", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var e = ValueTuple.Create();
                var a = (1, 2, 3, 4, 5, 6, 7, e);
                writer.Serialize(a);
                var data = stream.GetArraySegment();
                Assert.AreEqual("a8{1234567a{}}", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
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
                var data = stream.GetArraySegment();
                Assert.AreEqual("a1{1}a2{1s5\"hello\"}a3{12r2;}a4{123r2;}a5{1234r2;}a6{12345r2;}a7{123456r2;}a8{1234567r2;}r8;a9{12345678r2;}r9;a20{12345678901234567890}", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
        }
        [TestMethod]
        public void TestSerializeLinkedList() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var a = new LinkedList<int?>();
                a.AddLast((int?)null);
                a.AddLast(1);
                a.AddLast(2);
                a.AddLast(3);
                a.AddLast(4);
                a.AddLast(5);
                writer.Serialize(a);
                writer.Serialize(a);
                var data = stream.GetArraySegment();
                Assert.AreEqual("a6{n12345}r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
        }
        [TestMethod]
        public void TestSerializeBlockingCollection() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var a = new BlockingCollection<int?>() {
                    null, 1,2,3,4,5
                };
                writer.Serialize(a);
                writer.Serialize(a);
                var data = stream.GetArraySegment();
                Assert.AreEqual("a6{n12345}r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
        }
        [TestMethod]
        public void TestSerializeArrayList() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var a = new ArrayList() {
                    null, 1,2,3,4,5
                };
                writer.Serialize(a);
                writer.Serialize(a);
                var data = stream.GetArraySegment();
                Assert.AreEqual("a6{n12345}r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
        }
        [TestMethod]
        public void TestSerializeHashtable() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var dict = new Hashtable {
                    { new NullableKey<object>(null), 1 },
                    { 2, 3 },
                    { 4, 5 }
                };
                writer.Serialize(dict);
                writer.Serialize(dict);
                var data = stream.GetArraySegment();
                Assert.AreEqual("m3{4523n1}r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
        }
        [TestMethod]
        public void TestSerializeBitArray() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var a = new BitArray(new bool[] { true, false, true, false, true, false });
                writer.Serialize(a);
                writer.Serialize(a);
                var data = stream.GetArraySegment();
                Assert.AreEqual("a6{tftftf}r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
        }
        [TestMethod]
        public void TestSerializeStream() {
            using (MemoryStream stream = new MemoryStream()) {
                using (MemoryStream s = new MemoryStream(new byte[] { (byte)'0', (byte)'1', (byte)'2' })) {
                    Writer writer = new Writer(stream);
                    writer.Serialize(s);
                    writer.Serialize(s);
                    var data = stream.GetArraySegment();
                    Assert.AreEqual("b3\"012\"r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
                }
            }
        }
        [TestMethod]
        public void TestSerializeMDArray() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var a = new int[2, 2, 2] {
                    { { 1, 2 }, { 3, 4 } },
                    { { 5, 6 }, { 7, 8 } }
                };
                writer.Serialize(a);
                writer.Serialize(a);
                var b = new int[2, 2] {
                    { 1, 2 }, { 3, 4 }
                };
                writer.Serialize(b);
                writer.Serialize(b);
                var data = stream.GetArraySegment();
                Assert.AreEqual("a2{a2{a2{12}a2{34}}a2{a2{56}a2{78}}}r0;a2{a2{12}a2{34}}r7;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
        }
        [TestMethod]
        public void TestSerializeAnonymousType() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var v = new { Amount = 108, Message = "Hello" };
                writer.Serialize(v);
                writer.Serialize(v);
                var data = stream.GetArraySegment();
                Assert.AreEqual("m2{s6\"amount\"i108;s7\"message\"s5\"Hello\"}r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
        }
        [DataContract(Name = "Person")]
        public class Person {
            [DataMember(Order = 0)]
            public int Id;
            [DataMember(Order = 1)]
            public string Name;
            [DataMember(Order = 2)]
            public int Age;
        }
        [TestMethod]
        public void TestSerializeObject() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var o = new Person {
                    Id = 0,
                    Name = "Tom",
                    Age = 48
                };
                var o2 = new Person {
                    Id = 1,
                    Name = "Jerry",
                    Age = 36
                };
                writer.Serialize(o);
                writer.Serialize(o2);
                writer.Serialize(o);
                var data = stream.GetArraySegment();
                Assert.AreEqual("c6\"Person\"3{s2\"id\"s4\"name\"s3\"age\"}o0{0s3\"Tom\"i48;}o0{1s5\"Jerry\"i36;}r3;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
        }
        [TestMethod]
        public void TestSerializeExpandoObject() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                dynamic o = new ExpandoObject();
                o.Id = 1;
                o.Name = "Test";
                Assert.AreEqual((o as IDictionary<string, object>).ContainsKey("Id"), true);
                writer.Serialize(o);
                writer.Serialize(o);
                var data = stream.GetArraySegment();
                Assert.AreEqual("m2{s2\"id\"1s4\"name\"s4\"Test\"}r0;", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
        }
        [TestMethod]
        public void TestSerializeException() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                Exception e = new Exception("error");
                writer.Serialize(e);
                writer.Serialize(e);
                var data = stream.GetArraySegment();
                Assert.AreEqual("Es5\"error\"Es5\"error\"", Encoding.UTF8.GetString(data.Array, data.Offset, data.Count));
            }
        }
    }
}
