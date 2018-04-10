using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization;

using Hprose.Collections.Generic;
using Hprose.IO.Serializers;
using Hprose.IO.Deserializers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hprose.UnitTests.IO.Deserializers {
    [TestClass]
    public class ReaderTests {
        [TestMethod]
        public void TestDeserializeByte() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(true);
                writer.Serialize(false);
                writer.Serialize("");
                writer.Serialize('0');
                writer.Serialize('1');
                writer.Serialize("0x23");
                writer.Serialize("0x23");
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                writer.Serialize(3.14);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(reader.Deserialize<byte>(), 0);
                Assert.AreEqual(reader.Deserialize<byte>(), 1);
                Assert.AreEqual(reader.Deserialize<byte>(), 0);
                Assert.AreEqual(reader.Deserialize<byte>(), 0);
                Assert.AreEqual(reader.Deserialize<byte>(), 0);
                Assert.AreEqual(reader.Deserialize<byte>(), 1);
                Assert.AreEqual(reader.Deserialize<byte>(), 0x23);
                Assert.AreEqual(reader.Deserialize<byte>(), 0x23);
                Assert.AreEqual(reader.Deserialize<byte>(), 123);
                Assert.AreEqual(reader.Deserialize<byte>(), byte.MaxValue - 123 + 1);
                Assert.AreEqual(reader.Deserialize<byte>(), 3);
            }
        }
        [TestMethod]
        public void TestDeserializeSByte() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(true);
                writer.Serialize(false);
                writer.Serialize("");
                writer.Serialize('0');
                writer.Serialize('1');
                writer.Serialize("0x23");
                writer.Serialize("0x23");
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                writer.Serialize(3.14);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(reader.Deserialize<sbyte>(), 0);
                Assert.AreEqual(reader.Deserialize<sbyte>(), 1);
                Assert.AreEqual(reader.Deserialize<sbyte>(), 0);
                Assert.AreEqual(reader.Deserialize<sbyte>(), 0);
                Assert.AreEqual(reader.Deserialize<sbyte>(), 0);
                Assert.AreEqual(reader.Deserialize<sbyte>(), 1);
                Assert.AreEqual(reader.Deserialize<sbyte>(), 0x23);
                Assert.AreEqual(reader.Deserialize<sbyte>(), 0x23);
                Assert.AreEqual(reader.Deserialize<sbyte>(), 123);
                Assert.AreEqual(reader.Deserialize<sbyte>(), -123);
                Assert.AreEqual(reader.Deserialize<sbyte>(), 3);
            }
        }
        [TestMethod]
        public void TestDeserializeInt16() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(true);
                writer.Serialize(false);
                writer.Serialize("");
                writer.Serialize('0');
                writer.Serialize('1');
                writer.Serialize("0x23");
                writer.Serialize("0x23");
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                writer.Serialize(3.14);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(reader.Deserialize<short>(), 0);
                Assert.AreEqual(reader.Deserialize<short>(), 1);
                Assert.AreEqual(reader.Deserialize<short>(), 0);
                Assert.AreEqual(reader.Deserialize<short>(), 0);
                Assert.AreEqual(reader.Deserialize<short>(), 0);
                Assert.AreEqual(reader.Deserialize<short>(), 1);
                Assert.AreEqual(reader.Deserialize<short>(), 0x23);
                Assert.AreEqual(reader.Deserialize<short>(), 0x23);
                Assert.AreEqual(reader.Deserialize<short>(), 123);
                Assert.AreEqual(reader.Deserialize<short>(), -123);
                Assert.AreEqual(reader.Deserialize<short>(), 3);
            }
        }
        [TestMethod]
        public void TestDeserializeUInt16() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(true);
                writer.Serialize(false);
                writer.Serialize("");
                writer.Serialize('0');
                writer.Serialize('1');
                writer.Serialize("0x23");
                writer.Serialize("0x23");
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                writer.Serialize(3.14);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(reader.Deserialize<ushort>(), 0);
                Assert.AreEqual(reader.Deserialize<ushort>(), 1);
                Assert.AreEqual(reader.Deserialize<ushort>(), 0);
                Assert.AreEqual(reader.Deserialize<ushort>(), 0);
                Assert.AreEqual(reader.Deserialize<ushort>(), 0);
                Assert.AreEqual(reader.Deserialize<ushort>(), 1);
                Assert.AreEqual(reader.Deserialize<ushort>(), 0x23);
                Assert.AreEqual(reader.Deserialize<ushort>(), 0x23);
                Assert.AreEqual(reader.Deserialize<ushort>(), 123);
                Assert.AreEqual(reader.Deserialize<ushort>(), ushort.MaxValue - 123 + 1);
                Assert.AreEqual(reader.Deserialize<ushort>(), 3);
            }
        }
        [TestMethod]
        public void TestDeserializeInt32() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(true);
                writer.Serialize(false);
                writer.Serialize("");
                writer.Serialize('0');
                writer.Serialize('1');
                writer.Serialize("0x23");
                writer.Serialize("0x23");
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                writer.Serialize(3.14);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(reader.Deserialize<int>(), 0);
                Assert.AreEqual(reader.Deserialize<int>(), 1);
                Assert.AreEqual(reader.Deserialize<int>(), 0);
                Assert.AreEqual(reader.Deserialize<int>(), 0);
                Assert.AreEqual(reader.Deserialize<int>(), 0);
                Assert.AreEqual(reader.Deserialize<int>(), 1);
                Assert.AreEqual(reader.Deserialize<int>(), 0x23);
                Assert.AreEqual(reader.Deserialize<int>(), 0x23);
                Assert.AreEqual(reader.Deserialize<int>(), 123);
                Assert.AreEqual(reader.Deserialize<int>(), -123);
                Assert.AreEqual(reader.Deserialize<int>(), 3);
            }
        }
        [TestMethod]
        public void TestDeserializeUInt32() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(true);
                writer.Serialize(false);
                writer.Serialize("");
                writer.Serialize('0');
                writer.Serialize('1');
                writer.Serialize("0x23");
                writer.Serialize("0x23");
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                writer.Serialize(3.14);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(reader.Deserialize<uint>(), 0u);
                Assert.AreEqual(reader.Deserialize<uint>(), 1u);
                Assert.AreEqual(reader.Deserialize<uint>(), 0u);
                Assert.AreEqual(reader.Deserialize<uint>(), 0u);
                Assert.AreEqual(reader.Deserialize<uint>(), 0u);
                Assert.AreEqual(reader.Deserialize<uint>(), 1u);
                Assert.AreEqual(reader.Deserialize<uint>(), 0x23u);
                Assert.AreEqual(reader.Deserialize<uint>(), 0x23u);
                Assert.AreEqual(reader.Deserialize<uint>(), 123u);
                Assert.AreEqual(reader.Deserialize<uint>(), uint.MaxValue - 123 + 1);
                Assert.AreEqual(reader.Deserialize<uint>(), 3u);
            }
        }
        [TestMethod]
        public void TestDeserializeInt64() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(true);
                writer.Serialize(false);
                writer.Serialize("");
                writer.Serialize('0');
                writer.Serialize('1');
                writer.Serialize("0x23");
                writer.Serialize("0x23");
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                writer.Serialize(3.14);
                writer.Serialize(new DateTime(1234567890123456789));
                writer.Serialize(new DateTime(1234567890123456789));
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(reader.Deserialize<long>(), 0L);
                Assert.AreEqual(reader.Deserialize<long>(), 1L);
                Assert.AreEqual(reader.Deserialize<long>(), 0L);
                Assert.AreEqual(reader.Deserialize<long>(), 0L);
                Assert.AreEqual(reader.Deserialize<long>(), 0L);
                Assert.AreEqual(reader.Deserialize<long>(), 1L);
                Assert.AreEqual(reader.Deserialize<long>(), 0x23L);
                Assert.AreEqual(reader.Deserialize<long>(), 0x23L);
                Assert.AreEqual(reader.Deserialize<long>(), 123L);
                Assert.AreEqual(reader.Deserialize<long>(), -123L);
                Assert.AreEqual(reader.Deserialize<long>(), 3L);
                Assert.AreEqual(reader.Deserialize<long>(), 1234567890123456789);
                Assert.AreEqual(reader.Deserialize<long>(), 1234567890123456789);
            }
        }
        [TestMethod]
        public void TestDeserializeUInt64() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(true);
                writer.Serialize(false);
                writer.Serialize("");
                writer.Serialize('0');
                writer.Serialize('1');
                writer.Serialize("0x23");
                writer.Serialize("0x23");
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                writer.Serialize(3.14);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(reader.Deserialize<ulong>(), 0uL);
                Assert.AreEqual(reader.Deserialize<ulong>(), 1uL);
                Assert.AreEqual(reader.Deserialize<ulong>(), 0uL);
                Assert.AreEqual(reader.Deserialize<ulong>(), 0uL);
                Assert.AreEqual(reader.Deserialize<ulong>(), 0uL);
                Assert.AreEqual(reader.Deserialize<ulong>(), 1uL);
                Assert.AreEqual(reader.Deserialize<ulong>(), 0x23uL);
                Assert.AreEqual(reader.Deserialize<ulong>(), 0x23uL);
                Assert.AreEqual(reader.Deserialize<ulong>(), 123uL);
                Assert.AreEqual(reader.Deserialize<ulong>(), ulong.MaxValue - 123 + 1);
                Assert.AreEqual(reader.Deserialize<ulong>(), 3uL);
            }
        }
        [TestMethod]
        public void TestDeserializeBoolean() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(true);
                writer.Serialize(false);
                writer.Serialize("");
                writer.Serialize('\0');
                writer.Serialize('0');
                writer.Serialize('1');
                writer.Serialize("true");
                writer.Serialize("false");
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                writer.Serialize(3.14);
                writer.Serialize(double.NaN);
                writer.Serialize(double.NegativeInfinity);
                writer.Serialize(double.PositiveInfinity);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(reader.Deserialize<bool>(), false);
                Assert.AreEqual(reader.Deserialize<bool>(), true);
                Assert.AreEqual(reader.Deserialize<bool>(), false);
                Assert.AreEqual(reader.Deserialize<bool>(), false);
                Assert.AreEqual(reader.Deserialize<bool>(), false);
                Assert.AreEqual(reader.Deserialize<bool>(), false);
                Assert.AreEqual(reader.Deserialize<bool>(), true);
                Assert.AreEqual(reader.Deserialize<bool>(), true);
                Assert.AreEqual(reader.Deserialize<bool>(), false);
                Assert.AreEqual(reader.Deserialize<bool>(), true);
                Assert.AreEqual(reader.Deserialize<bool>(), true);
                Assert.AreEqual(reader.Deserialize<bool>(), true);
                Assert.AreEqual(reader.Deserialize<bool>(), true);
                Assert.AreEqual(reader.Deserialize<bool>(), true);
                Assert.AreEqual(reader.Deserialize<bool>(), true);
            }
        }
    }
}