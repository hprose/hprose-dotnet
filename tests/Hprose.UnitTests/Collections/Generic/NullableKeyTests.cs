using System;
using System.Collections.Generic;

using Hprose.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hprose.UnitTests.Collections.Generic {
    [TestClass]
    public class NullableKeyTests {
        [TestMethod]
        public void TestNullableKeyString() {
            var dict = new Dictionary<NullableKey<string>, string> {
                { "Hello", "World" },
                { null, "Null" }
            };
            Assert.AreEqual<int>(2, dict.Count);
        }
        [TestMethod]
        public void TestNullableKeyInt() {
            var dict = new Dictionary<NullableKey<int?>, string> {
                { 1, "Hello" },
                { null, "Null" }
            };
            Assert.AreEqual<int>(2, dict.Count);
        }
        [TestMethod]
        public void TestEquals() {
            NullableKey<String> s = "hello";
            Assert.IsTrue(s == "hello");
            s = null;
            Assert.IsTrue(s == null);
            Assert.IsFalse(s.Equals(0));
        }
        [TestMethod]
        public void TestGetHashCode() {
            NullableKey<String> s = "hello";
            Assert.IsTrue(s.GetHashCode() == "hello".GetHashCode());
            s = null;
            Assert.IsTrue(s.GetHashCode() == 0);
        }
        [TestMethod]
        public void TestCompareTo() {
            NullableKey<String> s = "hello";
            Assert.IsTrue(s.CompareTo("hello") == 0);
            s = null;
            Assert.IsTrue(s.CompareTo("hello") < 0);
        }
    }
}
