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
using System.Numerics;

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
                Assert.AreEqual(0, reader.Deserialize<byte>());
                Assert.AreEqual(1, reader.Deserialize<byte>());
                Assert.AreEqual(0, reader.Deserialize<byte>());
                Assert.AreEqual(0, reader.Deserialize<byte>());
                Assert.AreEqual(0, reader.Deserialize<byte>());
                Assert.AreEqual(1, reader.Deserialize<byte>());
                Assert.AreEqual(0x23, reader.Deserialize<byte>());
                Assert.AreEqual(0x23, reader.Deserialize<byte>());
                Assert.AreEqual(123, reader.Deserialize<byte>());
                Assert.AreEqual(byte.MaxValue - 123 + 1, reader.Deserialize<byte>());
                Assert.AreEqual(3, reader.Deserialize<byte>());
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
                Assert.AreEqual(0, reader.Deserialize<sbyte>());
                Assert.AreEqual(1, reader.Deserialize<sbyte>());
                Assert.AreEqual(0, reader.Deserialize<sbyte>());
                Assert.AreEqual(0, reader.Deserialize<sbyte>());
                Assert.AreEqual(0, reader.Deserialize<sbyte>());
                Assert.AreEqual(1, reader.Deserialize<sbyte>());
                Assert.AreEqual(0x23, reader.Deserialize<sbyte>());
                Assert.AreEqual(0x23, reader.Deserialize<sbyte>());
                Assert.AreEqual(123, reader.Deserialize<sbyte>());
                Assert.AreEqual(-123, reader.Deserialize<sbyte>());
                Assert.AreEqual(3, reader.Deserialize<sbyte>());
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
                Assert.AreEqual(0, reader.Deserialize<short>());
                Assert.AreEqual(1, reader.Deserialize<short>());
                Assert.AreEqual(0, reader.Deserialize<short>());
                Assert.AreEqual(0, reader.Deserialize<short>());
                Assert.AreEqual(0, reader.Deserialize<short>());
                Assert.AreEqual(1, reader.Deserialize<short>());
                Assert.AreEqual(0x23, reader.Deserialize<short>());
                Assert.AreEqual(0x23, reader.Deserialize<short>());
                Assert.AreEqual(123, reader.Deserialize<short>());
                Assert.AreEqual(-123, reader.Deserialize<short>());
                Assert.AreEqual(3, reader.Deserialize<short>());
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
                Assert.AreEqual(0, reader.Deserialize<ushort>());
                Assert.AreEqual(1, reader.Deserialize<ushort>());
                Assert.AreEqual(0, reader.Deserialize<ushort>());
                Assert.AreEqual(0, reader.Deserialize<ushort>());
                Assert.AreEqual(0, reader.Deserialize<ushort>());
                Assert.AreEqual(1, reader.Deserialize<ushort>());
                Assert.AreEqual(0x23, reader.Deserialize<ushort>());
                Assert.AreEqual(0x23, reader.Deserialize<ushort>());
                Assert.AreEqual(123, reader.Deserialize<ushort>());
                Assert.AreEqual(ushort.MaxValue - 123 + 1, reader.Deserialize<ushort>());
                Assert.AreEqual(3, reader.Deserialize<ushort>());
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
                Assert.AreEqual(0, reader.Deserialize<int>());
                Assert.AreEqual(1, reader.Deserialize<int>());
                Assert.AreEqual(0, reader.Deserialize<int>());
                Assert.AreEqual(0, reader.Deserialize<int>());
                Assert.AreEqual(0, reader.Deserialize<int>());
                Assert.AreEqual(1, reader.Deserialize<int>());
                Assert.AreEqual(0x23, reader.Deserialize<int>());
                Assert.AreEqual(0x23, reader.Deserialize<int>());
                Assert.AreEqual(123, reader.Deserialize<int>());
                Assert.AreEqual(-123, reader.Deserialize<int>());
                Assert.AreEqual(3, reader.Deserialize<int>());
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
                Assert.AreEqual(0u, reader.Deserialize<uint>());
                Assert.AreEqual(1u, reader.Deserialize<uint>());
                Assert.AreEqual(0u, reader.Deserialize<uint>());
                Assert.AreEqual(0u, reader.Deserialize<uint>());
                Assert.AreEqual(0u, reader.Deserialize<uint>());
                Assert.AreEqual(1u, reader.Deserialize<uint>());
                Assert.AreEqual(0x23u, reader.Deserialize<uint>());
                Assert.AreEqual(0x23u, reader.Deserialize<uint>());
                Assert.AreEqual(123u, reader.Deserialize<uint>());
                Assert.AreEqual(uint.MaxValue - 123 + 1, reader.Deserialize<uint>());
                Assert.AreEqual(3u, reader.Deserialize<uint>());
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
                Assert.AreEqual(0L, reader.Deserialize<long>());
                Assert.AreEqual(1L, reader.Deserialize<long>());
                Assert.AreEqual(0L, reader.Deserialize<long>());
                Assert.AreEqual(0L, reader.Deserialize<long>());
                Assert.AreEqual(0L, reader.Deserialize<long>());
                Assert.AreEqual(1L, reader.Deserialize<long>());
                Assert.AreEqual(0x23L, reader.Deserialize<long>());
                Assert.AreEqual(0x23L, reader.Deserialize<long>());
                Assert.AreEqual(123L, reader.Deserialize<long>());
                Assert.AreEqual(-123L, reader.Deserialize<long>());
                Assert.AreEqual(3L, reader.Deserialize<long>());
                Assert.AreEqual(1234567890123456789, reader.Deserialize<long>());
                Assert.AreEqual(1234567890123456789, reader.Deserialize<long>());
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
                Assert.AreEqual(0uL, reader.Deserialize<ulong>());
                Assert.AreEqual(1uL, reader.Deserialize<ulong>());
                Assert.AreEqual(0uL, reader.Deserialize<ulong>());
                Assert.AreEqual(0uL, reader.Deserialize<ulong>());
                Assert.AreEqual(0uL, reader.Deserialize<ulong>());
                Assert.AreEqual(1uL, reader.Deserialize<ulong>());
                Assert.AreEqual(0x23uL, reader.Deserialize<ulong>());
                Assert.AreEqual(0x23uL, reader.Deserialize<ulong>());
                Assert.AreEqual(123uL, reader.Deserialize<ulong>());
                Assert.AreEqual(ulong.MaxValue - 123 + 1, reader.Deserialize<ulong>());
                Assert.AreEqual(3uL, reader.Deserialize<ulong>());
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
                Assert.IsFalse(reader.Deserialize<bool>());
                Assert.IsTrue(reader.Deserialize<bool>());
                Assert.IsFalse(reader.Deserialize<bool>());
                Assert.IsFalse(reader.Deserialize<bool>());
                Assert.IsFalse(reader.Deserialize<bool>());
                Assert.IsFalse(reader.Deserialize<bool>());
                Assert.IsTrue(reader.Deserialize<bool>());
                Assert.IsTrue(reader.Deserialize<bool>());
                Assert.IsFalse(reader.Deserialize<bool>());
                Assert.IsTrue(reader.Deserialize<bool>());
                Assert.IsTrue(reader.Deserialize<bool>());
                Assert.IsTrue(reader.Deserialize<bool>());
                Assert.IsTrue(reader.Deserialize<bool>());
                Assert.IsTrue(reader.Deserialize<bool>());
                Assert.IsTrue(reader.Deserialize<bool>());
            }
        }
        [TestMethod]
        public void TestDeserializeSingle() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(true);
                writer.Serialize(false);
                writer.Serialize("");
                writer.Serialize('0');
                writer.Serialize('1');
                writer.Serialize("5e2");
                writer.Serialize("5e2");
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                writer.Serialize(3.14);
                writer.Serialize(double.NaN);
                writer.Serialize(double.NegativeInfinity);
                writer.Serialize(double.PositiveInfinity);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(0f, reader.Deserialize<float>());
                Assert.AreEqual(1f, reader.Deserialize<float>());
                Assert.AreEqual(0f, reader.Deserialize<float>());
                Assert.AreEqual(0f, reader.Deserialize<float>());
                Assert.AreEqual(0f, reader.Deserialize<float>());
                Assert.AreEqual(1f, reader.Deserialize<float>());
                Assert.AreEqual(5e2f, reader.Deserialize<float>());
                Assert.AreEqual(5e2f, reader.Deserialize<float>());
                Assert.AreEqual(123f, reader.Deserialize<float>());
                Assert.AreEqual(-123f, reader.Deserialize<float>());
                Assert.AreEqual(3.14f, reader.Deserialize<float>());
                Assert.IsTrue(float.IsNaN(reader.Deserialize<float>()));
                Assert.IsTrue(float.IsNegativeInfinity(reader.Deserialize<float>()));
                Assert.IsTrue(float.IsPositiveInfinity(reader.Deserialize<float>()));
            }
        }
        [TestMethod]
        public void TestDeserializeDouble() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(true);
                writer.Serialize(false);
                writer.Serialize("");
                writer.Serialize('0');
                writer.Serialize('1');
                writer.Serialize("5e2");
                writer.Serialize("5e2");
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                writer.Serialize(3.14);
                writer.Serialize(double.NaN);
                writer.Serialize(double.NegativeInfinity);
                writer.Serialize(double.PositiveInfinity);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(0, reader.Deserialize<double>());
                Assert.AreEqual(1, reader.Deserialize<double>());
                Assert.AreEqual(0, reader.Deserialize<double>());
                Assert.AreEqual(0, reader.Deserialize<double>());
                Assert.AreEqual(0, reader.Deserialize<double>());
                Assert.AreEqual(1, reader.Deserialize<double>());
                Assert.AreEqual(5e2, reader.Deserialize<double>());
                Assert.AreEqual(5e2, reader.Deserialize<double>());
                Assert.AreEqual(123, reader.Deserialize<double>());
                Assert.AreEqual(-123, reader.Deserialize<double>());
                Assert.AreEqual(3.14, reader.Deserialize<double>());
                Assert.IsTrue(double.IsNaN(reader.Deserialize<double>()));
                Assert.IsTrue(double.IsNegativeInfinity(reader.Deserialize<double>()));
                Assert.IsTrue(double.IsPositiveInfinity(reader.Deserialize<double>()));
            }
        }
        [TestMethod]
        public void TestDeserializeDecimal() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(true);
                writer.Serialize(false);
                writer.Serialize("");
                writer.Serialize('0');
                writer.Serialize('1');
                writer.Serialize("-7.9e28");
                writer.Serialize("-7.9e28");
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                writer.Serialize(3.14);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(0m, reader.Deserialize<decimal>());
                Assert.AreEqual(1m, reader.Deserialize<decimal>());
                Assert.AreEqual(0m, reader.Deserialize<decimal>());
                Assert.AreEqual(0m, reader.Deserialize<decimal>());
                Assert.AreEqual(0m, reader.Deserialize<decimal>());
                Assert.AreEqual(1m, reader.Deserialize<decimal>());
                Assert.AreEqual(-7.9e28m, reader.Deserialize<decimal>());
                Assert.AreEqual(-7.9e28m, reader.Deserialize<decimal>());
                Assert.AreEqual(123m, reader.Deserialize<decimal>());
                Assert.AreEqual(-123m, reader.Deserialize<decimal>());
                Assert.AreEqual(3.14m, reader.Deserialize<decimal>());
            }
        }
        [TestMethod]
        public void TestDeserializeIntPtr() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(true);
                writer.Serialize(false);
                writer.Serialize("");
                writer.Serialize('0');
                writer.Serialize('1');
                writer.Serialize("123456789");
                writer.Serialize("123456789");
                writer.Serialize((IntPtr)(123456789));
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                writer.Serialize(3.14);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual((IntPtr)0, reader.Deserialize<IntPtr>());
                Assert.AreEqual((IntPtr)1, reader.Deserialize<IntPtr>());
                Assert.AreEqual((IntPtr)0, reader.Deserialize<IntPtr>());
                Assert.AreEqual((IntPtr)0, reader.Deserialize<IntPtr>());
                Assert.AreEqual((IntPtr)0, reader.Deserialize<IntPtr>());
                Assert.AreEqual((IntPtr)1, reader.Deserialize<IntPtr>());
                Assert.AreEqual((IntPtr)123456789, reader.Deserialize<IntPtr>());
                Assert.AreEqual((IntPtr)123456789, reader.Deserialize<IntPtr>());
                Assert.AreEqual((IntPtr)123456789, reader.Deserialize<IntPtr>());
                Assert.AreEqual((IntPtr)123, reader.Deserialize<IntPtr>());
                Assert.AreEqual((IntPtr)(-123), reader.Deserialize<IntPtr>());
                Assert.AreEqual((IntPtr)3, reader.Deserialize<IntPtr>());
            }
        }
        [TestMethod]
        public void TestDeserializeUIntPtr() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(true);
                writer.Serialize(false);
                writer.Serialize("");
                writer.Serialize('0');
                writer.Serialize('1');
                writer.Serialize("123456789");
                writer.Serialize("123456789");
                writer.Serialize((UIntPtr)(123456789));
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                writer.Serialize(3.14);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual((UIntPtr)0, reader.Deserialize<UIntPtr>());
                Assert.AreEqual((UIntPtr)1, reader.Deserialize<UIntPtr>());
                Assert.AreEqual((UIntPtr)0, reader.Deserialize<UIntPtr>());
                Assert.AreEqual((UIntPtr)0, reader.Deserialize<UIntPtr>());
                Assert.AreEqual((UIntPtr)0, reader.Deserialize<UIntPtr>());
                Assert.AreEqual((UIntPtr)1, reader.Deserialize<UIntPtr>());
                Assert.AreEqual((UIntPtr)123456789, reader.Deserialize<UIntPtr>());
                Assert.AreEqual((UIntPtr)123456789, reader.Deserialize<UIntPtr>());
                Assert.AreEqual((UIntPtr)123456789, reader.Deserialize<UIntPtr>());
                Assert.AreEqual((UIntPtr)123, reader.Deserialize<UIntPtr>());
                Assert.AreEqual((UIntPtr)(-123), reader.Deserialize<UIntPtr>());
                Assert.AreEqual((UIntPtr)3, reader.Deserialize<UIntPtr>());
            }
        }
        [TestMethod]
        public void TestDeserializeBigInteger() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(true);
                writer.Serialize(false);
                writer.Serialize("");
                writer.Serialize('0');
                writer.Serialize('1');
                writer.Serialize("123456789");
                writer.Serialize("123456789");
                writer.Serialize(BigInteger.Parse("123456789123456789123456789123456789"));
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                writer.Serialize(3.14);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(0, reader.Deserialize<BigInteger>());
                Assert.AreEqual(1, reader.Deserialize<BigInteger>());
                Assert.AreEqual(0, reader.Deserialize<BigInteger>());
                Assert.AreEqual(0, reader.Deserialize<BigInteger>());
                Assert.AreEqual(0, reader.Deserialize<BigInteger>());
                Assert.AreEqual(1, reader.Deserialize<BigInteger>());
                Assert.AreEqual(123456789, reader.Deserialize<BigInteger>());
                Assert.AreEqual(123456789, reader.Deserialize<BigInteger>());
                Assert.AreEqual(BigInteger.Parse("123456789123456789123456789123456789"), reader.Deserialize<BigInteger>());
                Assert.AreEqual(123, reader.Deserialize<BigInteger>());
                Assert.AreEqual(-123, reader.Deserialize<BigInteger>());
                Assert.AreEqual(3, reader.Deserialize<BigInteger>());
            }
        }
        [TestMethod]
        public void TestDeserializeDBNull() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(DBNull.Value);
                writer.Serialize("");
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(DBNull.Value, reader.Deserialize<DBNull>());
                Assert.AreEqual(DBNull.Value, reader.Deserialize<DBNull>());
                Assert.AreEqual(DBNull.Value, reader.Deserialize<DBNull>());
            }
        }
    }
}