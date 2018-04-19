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
using System.Text;
using System.Collections.Specialized;
using System.Linq;

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
        [TestMethod]
        public void TestDeserializeChar() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize("");
                writer.Serialize('0');
                writer.Serialize('1');
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                writer.Serialize(3.14);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual((char)0, reader.Deserialize<char>());
                Assert.AreEqual((char)0, reader.Deserialize<char>());
                Assert.AreEqual('0', reader.Deserialize<char>());
                Assert.AreEqual('1', reader.Deserialize<char>());
                Assert.AreEqual((char)123, reader.Deserialize<char>());
                Assert.AreEqual(char.MaxValue - 123 + 1, reader.Deserialize<char>());
                Assert.AreEqual((char)3, reader.Deserialize<char>());
            }
        }
        [TestMethod]
        public void TestDeserializeTimeSpan() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize("");
                writer.Serialize('0');
                writer.Serialize('1');
                writer.Serialize((byte)123);
                writer.Serialize((sbyte)-123);
                writer.Serialize(3.14);
                writer.Serialize(new DateTime(123456789));
                writer.Serialize(new TimeSpan(123456789));
                writer.Serialize("1:30:40");
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(TimeSpan.Zero, reader.Deserialize<TimeSpan>());
                Assert.AreEqual(TimeSpan.Zero, reader.Deserialize<TimeSpan>());
                Assert.AreEqual(TimeSpan.Zero, reader.Deserialize<TimeSpan>());
                Assert.AreEqual(new TimeSpan(1, 0, 0, 0), reader.Deserialize<TimeSpan>());
                Assert.AreEqual(new TimeSpan(123), reader.Deserialize<TimeSpan>());
                Assert.AreEqual(new TimeSpan(-123), reader.Deserialize<TimeSpan>());
                Assert.AreEqual(new TimeSpan(3), reader.Deserialize<TimeSpan>());
                Assert.AreEqual(new TimeSpan(123456789), reader.Deserialize<TimeSpan>());
                Assert.AreEqual(new TimeSpan(123456789), reader.Deserialize<TimeSpan>());
                Assert.AreEqual(new TimeSpan(1, 30, 40), reader.Deserialize<TimeSpan>());
            }
        }
        [TestMethod]
        public void TestDeserializeDateTime() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize("");
                writer.Serialize((byte)123);
                writer.Serialize(3.14);
                writer.Serialize(new DateTime(123456789));
                writer.Serialize(new TimeSpan(123456789));
                writer.Serialize("2018-04-14 12:39:40");
                writer.Serialize("2018-04-14 12:39:40");
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(new DateTime(0), reader.Deserialize<DateTime>());
                Assert.AreEqual(new DateTime(0), reader.Deserialize<DateTime>());
                Assert.AreEqual(new DateTime(123), reader.Deserialize<DateTime>());
                Assert.AreEqual(new DateTime(3), reader.Deserialize<DateTime>());
                Assert.AreEqual(new DateTime(123456789), reader.Deserialize<DateTime>());
                Assert.AreEqual(new DateTime(123456789), reader.Deserialize<DateTime>());
                Assert.AreEqual(new DateTime(2018, 04, 14, 12, 39, 40), reader.Deserialize<DateTime>());
                Assert.AreEqual(new DateTime(2018, 04, 14, 12, 39, 40), reader.Deserialize<DateTime>());
            }
        }
        [TestMethod]
        public void TestDeserializeGuid() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(Guid.Empty);
                writer.Serialize(Guid.Empty);
                writer.Serialize(Guid.Empty.ToByteArray());
                writer.Serialize(Guid.Empty.ToString());
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(Guid.Empty, reader.Deserialize<Guid>());
                Assert.AreEqual(Guid.Empty, reader.Deserialize<Guid>());
                Assert.AreEqual(Guid.Empty, reader.Deserialize<Guid>());
                Assert.AreEqual(Guid.Empty, reader.Deserialize<Guid>());
            }
        }
        [TestMethod]
        public void TestDeserializeString() {
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
                writer.Serialize(new DateTime(2018, 4, 14));
                writer.Serialize(Guid.Empty);
                writer.Serialize(new byte[] { (byte)'0', (byte)'1', (byte)'2' });
                writer.Serialize(new byte[] { (byte)'0', (byte)'1', (byte)'2' });
                writer.Serialize(new List<char> { '0', '1', '2' });
                writer.Serialize(new List<char> { '0', '1', '2' });
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<string>());
                Assert.AreEqual(bool.TrueString, reader.Deserialize<string>());
                Assert.AreEqual(bool.FalseString, reader.Deserialize<string>());
                Assert.AreEqual("", reader.Deserialize<string>());
                Assert.AreEqual("0", reader.Deserialize<string>());
                Assert.AreEqual("1", reader.Deserialize<string>());
                Assert.AreEqual("5e2", reader.Deserialize<string>());
                Assert.AreEqual("5e2", reader.Deserialize<string>());
                Assert.AreEqual("123", reader.Deserialize<string>());
                Assert.AreEqual("-123", reader.Deserialize<string>());
                Assert.AreEqual("3.14", reader.Deserialize<string>());
                Assert.AreEqual(double.NaN.ToString(), reader.Deserialize<string>());
                Assert.AreEqual(double.NegativeInfinity.ToString(), reader.Deserialize<string>());
                Assert.AreEqual(double.PositiveInfinity.ToString(), reader.Deserialize<string>());
                Assert.AreEqual(new DateTime(2018, 4, 14).ToString(), reader.Deserialize<string>());
                Assert.AreEqual(Guid.Empty.ToString(), reader.Deserialize<string>());
                Assert.AreEqual("012", reader.Deserialize<string>());
                Assert.AreEqual("012", reader.Deserialize<string>());
                Assert.AreEqual("012", reader.Deserialize<string>());
                Assert.AreEqual("012", reader.Deserialize<string>());
            }
        }
        [TestMethod]
        public void TestDeserializeChars() {
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
                writer.Serialize(new DateTime(2018, 4, 14));
                writer.Serialize(Guid.Empty);
                writer.Serialize(new byte[] { (byte)'0', (byte)'1', (byte)'2' });
                writer.Serialize(new byte[] { (byte)'0', (byte)'1', (byte)'2' });
                writer.Serialize(new List<char> { '0', '1', '2' });
                writer.Serialize(new List<char> { '0', '1', '2' });
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<char[]>());
                Assert.AreEqual(bool.TrueString, new String(reader.Deserialize<char[]>()));
                Assert.AreEqual(bool.FalseString, new String(reader.Deserialize<char[]>()));
                Assert.AreEqual("", new String(reader.Deserialize<char[]>()));
                Assert.AreEqual("0", new String(reader.Deserialize<char[]>()));
                Assert.AreEqual("1", new String(reader.Deserialize<char[]>()));
                Assert.AreEqual("5e2", new String(reader.Deserialize<char[]>()));
                Assert.AreEqual("5e2", new String(reader.Deserialize<char[]>()));
                Assert.AreEqual("123", new String(reader.Deserialize<char[]>()));
                Assert.AreEqual("-123", new String(reader.Deserialize<char[]>()));
                Assert.AreEqual("3.14", new String(reader.Deserialize<char[]>()));
                Assert.AreEqual(double.NaN.ToString(), new String(reader.Deserialize<char[]>()));
                Assert.AreEqual(double.NegativeInfinity.ToString(), new String(reader.Deserialize<char[]>()));
                Assert.AreEqual(double.PositiveInfinity.ToString(), new String(reader.Deserialize<char[]>()));
                Assert.AreEqual(new DateTime(2018, 4, 14).ToString(), new String(reader.Deserialize<char[]>()));
                Assert.AreEqual(Guid.Empty.ToString(), new String(reader.Deserialize<char[]>()));
                Assert.AreEqual("012", new String(reader.Deserialize<char[]>()));
                Assert.AreEqual("012", new String(reader.Deserialize<char[]>()));
                Assert.AreEqual("012", new String(reader.Deserialize<char[]>()));
                Assert.AreEqual("012", new String(reader.Deserialize<char[]>()));
            }
        }
        [TestMethod]
        public void TestDeserializeStringBuilder() {
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
                writer.Serialize(new DateTime(2018, 4, 14));
                writer.Serialize(Guid.Empty);
                writer.Serialize(new byte[] { (byte)'0', (byte)'1', (byte)'2' });
                writer.Serialize(new byte[] { (byte)'0', (byte)'1', (byte)'2' });
                writer.Serialize(new List<char> { '0', '1', '2' });
                writer.Serialize(new List<char> { '0', '1', '2' });
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<StringBuilder>());
                Assert.AreEqual(bool.TrueString, reader.Deserialize<StringBuilder>().ToString());
                Assert.AreEqual(bool.FalseString, reader.Deserialize<StringBuilder>().ToString());
                Assert.AreEqual("", reader.Deserialize<StringBuilder>().ToString());
                Assert.AreEqual("0", reader.Deserialize<StringBuilder>().ToString());
                Assert.AreEqual("1", reader.Deserialize<StringBuilder>().ToString());
                Assert.AreEqual("5e2", reader.Deserialize<StringBuilder>().ToString());
                Assert.AreEqual("5e2", reader.Deserialize<StringBuilder>().ToString());
                Assert.AreEqual("123", reader.Deserialize<StringBuilder>().ToString());
                Assert.AreEqual("-123", reader.Deserialize<StringBuilder>().ToString());
                Assert.AreEqual("3.14", reader.Deserialize<StringBuilder>().ToString());
                Assert.AreEqual(double.NaN.ToString(), reader.Deserialize<StringBuilder>().ToString());
                Assert.AreEqual(double.NegativeInfinity.ToString(), reader.Deserialize<StringBuilder>().ToString());
                Assert.AreEqual(double.PositiveInfinity.ToString(), reader.Deserialize<StringBuilder>().ToString());
                Assert.AreEqual(new DateTime(2018, 4, 14).ToString(), reader.Deserialize<StringBuilder>().ToString());
                Assert.AreEqual(Guid.Empty.ToString(), reader.Deserialize<StringBuilder>().ToString());
                Assert.AreEqual("012", reader.Deserialize<StringBuilder>().ToString());
                Assert.AreEqual("012", reader.Deserialize<StringBuilder>().ToString());
                Assert.AreEqual("012", reader.Deserialize<StringBuilder>().ToString());
                Assert.AreEqual("012", reader.Deserialize<StringBuilder>().ToString());
            }
        }
        [TestMethod]
        public void TestDeserializeBytes() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize("1234567890");
                writer.Serialize(Guid.Empty);
                writer.Serialize(new byte[] { (byte)'0', (byte)'1', (byte)'2' });
                writer.Serialize(new byte[] { (byte)'0', (byte)'1', (byte)'2' });
                writer.Serialize(new List<int> { '0', '1', '2' });
                writer.Serialize(new List<int> { '0', '1', '2' });
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<byte[]>());
                Assert.AreEqual("1234567890", Encoding.UTF8.GetString(reader.Deserialize<byte[]>()));
                Assert.AreEqual(Encoding.UTF8.GetString(Guid.Empty.ToByteArray()), Encoding.UTF8.GetString(reader.Deserialize<byte[]>()));
                Assert.AreEqual("012", Encoding.UTF8.GetString(reader.Deserialize<byte[]>()));
                Assert.AreEqual("012", Encoding.UTF8.GetString(reader.Deserialize<byte[]>()));
                Assert.AreEqual("012", Encoding.UTF8.GetString(reader.Deserialize<byte[]>()));
                Assert.AreEqual("012", Encoding.UTF8.GetString(reader.Deserialize<byte[]>()));
            }
        }
        private void AreEqual<T>(T[] array1, T[] array2) {
            Assert.AreEqual(array1.Length, array2.Length);
            for (int i = 0; i < array1.Length; i++) {
                Assert.AreEqual(array1[i], array2[i]);
            }
        }
        private void AreEqual<T>(T[,] array1, T[,] array2) {
            Assert.AreEqual(array1.GetLength(0), array2.GetLength(0));
            Assert.AreEqual(array1.GetLength(1), array2.GetLength(1));
            for (int i = 0; i < array1.GetLength(0); i++) {
                for (int j = 0; j < array1.GetLength(1); j++) {
                    Assert.AreEqual(array1[i, j], array2[i, j]);
                }
            }
        }
        private void AreEqual<T>(T[,,] array1, T[,,] array2) {
            Assert.AreEqual(array1.GetLength(0), array2.GetLength(0));
            Assert.AreEqual(array1.GetLength(1), array2.GetLength(1));
            Assert.AreEqual(array1.GetLength(2), array2.GetLength(2));
            for (int i = 0; i < array1.GetLength(0); i++) {
                for (int j = 0; j < array1.GetLength(1); j++) {
                    for (int k = 0; k < array1.GetLength(2); k++) {
                        Assert.AreEqual(array1[i, j, k], array2[i, j, k]);
                    }
                }
            }
        }
        [TestMethod]
        public void TestDeserializeArray() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var array = new int[] { '0', '1', '2', '3', '4', '5' };
                var array2 = new int[,] { { '0', '1', '2' }, { '3', '4', '5' } };
                var array3 = new int[,,] { { { '0', '1' }, { '2', '3' } }, { { '4', '5' }, { '6', '7' } } };
                writer.Serialize(null);
                writer.Serialize(array3);
                writer.Serialize(array2);
                writer.Serialize(array);
                writer.Serialize(null);
                writer.Serialize(null);
                writer.Serialize(array2);
                writer.Serialize(array3);
                writer.Serialize(array);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<int[]>());
                AreEqual(array3, reader.Deserialize<int[,,]>());
                AreEqual(array2, reader.Deserialize<int[,]>());
                AreEqual(array, reader.Deserialize<int[]>());
                Assert.AreEqual(null, reader.Deserialize<int[,]>());
                Assert.AreEqual(null, reader.Deserialize<int[,,]>());
                AreEqual(array2, reader.Deserialize<int[,]>());
                AreEqual(array3, reader.Deserialize<int[,,]>());
                AreEqual(array, reader.Deserialize<int[]>());
            }
        }
        private void AreEqual<T>(ICollection<T> c1, ICollection<T> c2) {
            Assert.AreEqual(c1.Count, c2.Count);
            foreach (var e in c1) {
                Assert.IsTrue(c2.Contains(e));
            }
        }
        private void AreEqual<T>(IReadOnlyCollection<T> c1, IReadOnlyCollection<T> c2) {
            Assert.AreEqual(c1.Count, c2.Count);
            foreach (var e in c1) {
                Assert.IsTrue(c2.Contains(e));
            }
        }
        [TestMethod]
        public void TestDeserializeCollection() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var list = new List<int> { '0', '1', '2', '3', '4', '5' };
                writer.Serialize(null);
                writer.Serialize(list);
                writer.Serialize(list);
                writer.Serialize(list);
                writer.Serialize(list);
                var set = new HashSet<int> { '0', '1', '2' };
                writer.Serialize(set);
                writer.Serialize(set);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<List<int>>());
                AreEqual(list, reader.Deserialize<IList<int>>());
                AreEqual(list, reader.Deserialize<IList<int>>());
                AreEqual(list, reader.Deserialize<ICollection<int>>());
                AreEqual(list, (ICollection<int>)reader.Deserialize<IEnumerable<int>>());
                AreEqual(set, reader.Deserialize<ISet<int>>());
                AreEqual(set, reader.Deserialize<ISet<int>>());
            }
        }
        [TestMethod]
        public void TestDeserializeQueue() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var queue = new Queue<int>(new List<int> { '0', '1', '2', '3', '4', '5' });
                writer.Serialize(null);
                writer.Serialize(queue);
                writer.Serialize(queue);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<Queue<int>>());
                AreEqual(queue.ToArray(), reader.Deserialize<Queue<int>>().ToArray());
                AreEqual(queue.ToArray(), reader.Deserialize<Queue<int>>().ToArray());
            }
        }
        [TestMethod]
        public void TestDeserializeStack() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var stack = new Stack<int>(new List<int> { '0', '1', '2', '3', '4', '5' });
                writer.Serialize(null);
                writer.Serialize(stack);
                writer.Serialize(stack);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<Stack<int>>());
                AreEqual(stack.ToArray(), reader.Deserialize<Stack<int>>().ToArray());
                AreEqual(stack.ToArray(), reader.Deserialize<Stack<int>>().ToArray());
            }
        }
        [TestMethod]
        public void TestDeserializeBlockingCollection() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var c = new BlockingCollection<int> { '0', '1', '2', '3', '4', '5' };
                writer.Serialize(null);
                writer.Serialize(c);
                writer.Serialize(c);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<BlockingCollection<int>>());
                AreEqual(c.ToArray(), reader.Deserialize<BlockingCollection<int>>().ToArray());
                AreEqual(c.ToArray(), reader.Deserialize<BlockingCollection<int>>().ToArray());
            }
        }
        [TestMethod]
        public void TestDeserializeNullable() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var c = new BlockingCollection<int?> { null, '0', '1', null, '2', '3', '4', '5' };
                writer.Serialize(null);
                writer.Serialize(null);
                writer.Serialize(c);
                writer.Serialize(c);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<int?>());
                Assert.AreEqual(0, reader.Deserialize<int>());
                AreEqual(c.ToArray(), reader.Deserialize<BlockingCollection<int?>>().ToArray());
                AreEqual(c.ToArray(), reader.Deserialize<BlockingCollection<int?>>().ToArray());
            }
        }
        [TestMethod]
        public void TestDeserializeNullableKey() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(null);
                writer.Serialize(null);
                writer.Serialize(1);
                writer.Serialize("string");
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<NullableKey<int?>>());
                Assert.AreEqual(null, reader.Deserialize<NullableKey<string>>());
                Assert.AreEqual<int?>(1, reader.Deserialize<NullableKey<int?>>());
                Assert.AreEqual<string>("string", reader.Deserialize<NullableKey<string>>());
            }
        }
        [TestMethod]
        public void TestDeserializeEnum() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(TypeCode.Boolean);
                writer.Serialize(TypeCode.Empty);
                writer.Serialize(TypeCode.Double);
                writer.Serialize(true);
                writer.Serialize((char)(9));
                writer.Serialize("String");
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(TypeCode.Boolean, reader.Deserialize<TypeCode>());
                Assert.AreEqual(TypeCode.Empty, reader.Deserialize<TypeCode>());
                Assert.AreEqual(TypeCode.Double, reader.Deserialize<TypeCode>());
                Assert.AreEqual(TypeCode.Object, reader.Deserialize<TypeCode>());
                Assert.AreEqual(TypeCode.Int32, reader.Deserialize<TypeCode>());
                Assert.AreEqual(TypeCode.String, reader.Deserialize<TypeCode>());
            }
        }
        [TestMethod]
        public void TestDeserializeProduceConsumerCollection() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var s = new ConcurrentStack<int>(new List<int> { '0', '1', '2', '3', '4', '5' });
                var q = new ConcurrentQueue<int>(new List<int> { '0', '1', '2', '3', '4', '5' });
                writer.Serialize(null);
                writer.Serialize(s);
                writer.Serialize(q);
                writer.Serialize(null);
                writer.Serialize(s);
                writer.Serialize(q);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<ConcurrentStack<int>>());
                AreEqual(s.ToArray(), reader.Deserialize<ConcurrentStack<int>>().ToArray());
                AreEqual(q.ToArray(), reader.Deserialize<ConcurrentQueue<int>>().ToArray());
                Assert.AreEqual(null, reader.Deserialize<ConcurrentQueue<int>>());
                AreEqual(s.ToArray(), reader.Deserialize<ConcurrentStack<int>>().ToArray());
                AreEqual(q.ToArray(), reader.Deserialize<ConcurrentQueue<int>>().ToArray());
            }
        }
        private void AreEqual(StringCollection c1, StringCollection c2) {
            Assert.AreEqual(c1.Count, c2.Count);
            for (var i = 0; i < c1.Count; ++i) {
                Assert.AreEqual(c1[i], c2[i]);
            }
        }
        [TestMethod]
        public void TestDeserializeStringCollection() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var s = new StringCollection { "Hello", "World" };
                writer.Serialize(null);
                writer.Serialize(s);
                writer.Serialize(s);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<StringCollection>());
                AreEqual(s, reader.Deserialize<StringCollection>());
                AreEqual(s, reader.Deserialize<StringCollection>());
            }
        }
        [TestMethod]
        public void TestDeserializeDictionary() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var dict = new Dictionary<string, string> {
                    { "Item1", "XXXXX" },
                    { "Item2", "YYYYY" },
                    { "Item3", "ZZZZZ" },
                };
                writer.Serialize(null);
                writer.Serialize(dict);
                writer.Serialize(dict);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<IDictionary<string, string>>());
                AreEqual(dict, reader.Deserialize<IDictionary<string, string>>());
                AreEqual(dict, reader.Deserialize<IDictionary<string, string>>());
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var dict = new Dictionary<string, string> {
                    { "Item1", "XXXXX" },
                    { "Item2", "YYYYY" },
                    { "Item3", "ZZZZZ" },
                };
                writer.Serialize(null);
                writer.Serialize(dict);
                writer.Serialize(dict);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<ISet<KeyValuePair<string, string>>>());
                AreEqual(dict, reader.Deserialize<ISet<KeyValuePair<string, string>>>());
                AreEqual(dict, reader.Deserialize<ISet<KeyValuePair<string, string>>>());
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var dict = new Dictionary<string, string> {
                    { "Item1", "XXXXX" },
                    { "Item2", "YYYYY" },
                    { "Item3", "ZZZZZ" },
                };
                writer.Serialize(null);
                writer.Serialize(dict);
                writer.Serialize(dict);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<IList<KeyValuePair<string, string>>>());
                AreEqual(dict, reader.Deserialize<IList<KeyValuePair<string, string>>>());
                AreEqual(dict, reader.Deserialize<IList<KeyValuePair<string, string>>>());
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var dict = new Dictionary<string, string> {
                    { "Item1", "XXXXX" },
                    { "Item2", "YYYYY" },
                    { "Item3", "ZZZZZ" },
                };
                writer.Serialize(null);
                writer.Serialize(dict);
                writer.Serialize(dict);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<IReadOnlyList<KeyValuePair<string, string>>>());
                AreEqual(dict, reader.Deserialize<IReadOnlyList<KeyValuePair<string, string>>>());
                AreEqual(dict, reader.Deserialize<IReadOnlyList<KeyValuePair<string, string>>>());
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var dict = new Dictionary<string, string> {
                    { "Item1", "XXXXX" },
                    { "Item2", "YYYYY" },
                    { "Item3", "ZZZZZ" },
                };
                writer.Serialize(null);
                writer.Serialize(dict);
                writer.Serialize(dict);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<ICollection<KeyValuePair<string, string>>>());
                AreEqual(dict, reader.Deserialize<ICollection<KeyValuePair<string, string>>>());
                AreEqual(dict, reader.Deserialize<ICollection<KeyValuePair<string, string>>>());
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var dict = new Dictionary<string, string> {
                    { "Item1", "XXXXX" },
                    { "Item2", "YYYYY" },
                    { "Item3", "ZZZZZ" },
                };
                writer.Serialize(null);
                writer.Serialize(dict);
                writer.Serialize(dict);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<IReadOnlyCollection<KeyValuePair<string, string>>>());
                AreEqual(dict, reader.Deserialize<IReadOnlyCollection<KeyValuePair<string, string>>>());
                AreEqual(dict, reader.Deserialize<IReadOnlyCollection<KeyValuePair<string, string>>>());
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var dict = new Dictionary<string, string> {
                    { "Item1", "XXXXX" },
                    { "Item2", "YYYYY" },
                    { "Item3", "ZZZZZ" },
                };
                writer.Serialize(null);
                writer.Serialize(dict);
                writer.Serialize(dict);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<ConcurrentDictionary<string, string>>());
                AreEqual((ICollection<KeyValuePair<string, string>>)dict, reader.Deserialize<ConcurrentDictionary<string, string>>());
                AreEqual((ICollection<KeyValuePair<string, string>>)dict, reader.Deserialize<ConcurrentDictionary<string, string>>());
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var dict = new NullableKeyDictionary<object, object> {
                    { null, null },
                    { "Item1", "XXXXX" },
                    { "Item2", "YYYYY" },
                    { "Item3", "ZZZZZ" },
                };
                writer.Serialize(null);
                writer.Serialize(dict);
                writer.Serialize(dict);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize());
                AreEqual(dict, (ICollection<KeyValuePair<object, object>>)reader.Deserialize());
                AreEqual(dict, (ICollection<KeyValuePair<object, object>>)reader.Deserialize());
            }
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                dynamic o = new ExpandoObject();
                o.Id = 1;
                o.Name = "Test";
                writer.Serialize(null);
                writer.Serialize(o);
                writer.Serialize(o);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize());
                AreEqual(o, reader.Deserialize<ExpandoObject>());
                AreEqual(o, reader.Deserialize<ExpandoObject>());
            }
        }
        [TestMethod]
        public void TestDeserializeArrayList() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var c = new ArrayList { '0', '1', '2', '3', '4', '5' };
                writer.Serialize(null);
                writer.Serialize(c);
                writer.Serialize(c);
                stream.Position = 0;
                Reader reader = new Reader(stream) {
                    DefaultCharType = CharType.Char
                };
                Assert.AreEqual(null, reader.Deserialize<ArrayList>());
                AreEqual(c.ToArray(), reader.Deserialize<ArrayList>().ToArray());
                AreEqual(c.ToArray(), reader.Deserialize<ArrayList>().ToArray());
            }
        }
        private void AreEqual(IDictionary d1, IDictionary d2) {
            Assert.AreEqual(d1.Count, d2.Count);
            foreach (DictionaryEntry e in d1) {
                Assert.AreEqual(d2[e.Key], e.Value);
            }
        }
        [TestMethod]
        public void TestDeserializeHashtable() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var h = new Hashtable {
                    { "Item1", "XXXXX" },
                    { "Item2", "YYYYY" },
                    { "Item3", "ZZZZZ" },
                };
                writer.Serialize(null);
                writer.Serialize(h);
                writer.Serialize(h);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize<Hashtable>());
                AreEqual(h, reader.Deserialize<Hashtable>());
                AreEqual(h, reader.Deserialize<Hashtable>());
            }
        }
        public class Test {
            public int Id;
            public string Name;
        }
        [TestMethod]
        public void TestDeserializeObjectAsExpandoObject() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                var o = new Test {
                    Id = 1,
                    Name = "Test",
                };
                writer.Serialize(null);
                writer.Serialize(o);
                writer.Serialize(o);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                Assert.AreEqual(null, reader.Deserialize());
                dynamic o2 = reader.Deserialize<ExpandoObject>();
                Assert.AreEqual(o.Id, o2.Id);
                Assert.AreEqual(o.Name, o2.Name);
                dynamic o3 = reader.Deserialize<ExpandoObject>();
                Assert.AreEqual(o2, o3);
            }
        }
    }
}