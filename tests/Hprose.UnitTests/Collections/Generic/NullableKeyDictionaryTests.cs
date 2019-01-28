using System;
using System.Linq;

using Hprose.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hprose.UnitTests.Collections.Generic {
    [TestClass]
    public class NullableKeyDictionaryTests {
        [TestMethod]
        public void TestNullableKeyDictionaryString() {
            var dict = new NullableKeyDictionary<string, string> {
                { "Hello", "World" },
                { null, "Null" }
            };
            Assert.AreEqual(2, dict.Count);
        }
        [TestMethod]
        public void TestNullableKeyDictionaryInt() {
            var dict = new NullableKeyDictionary<int?, string> {
                { 1, "Hello" },
                { null, "Null" }
            };
            Assert.AreEqual(2, dict.Count);
        }
        [TestMethod]
        public void TestNullableKeyDictionaryKeys() {
            var dict = new NullableKeyDictionary<string, string> {
                { "Hello", "World" },
                { null, "Null" }
            };
            var keys = dict.Keys;
            var keys1 = new String[2];
            keys.CopyTo(keys1, 0);
            var keys2 = new String[] { "Hello", null };
            Assert.IsTrue(keys1.SequenceEqual(keys2));
        }
        [TestMethod]
        public void TestNullableKeyDictionaryContainsKey() {
            var dict = new NullableKeyDictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
                { "Hello", "World" },
                { null, "Null" }
            };
            Assert.IsTrue(dict["hello"] == dict["Hello"]);
            Assert.IsTrue(dict.ContainsKey("hello"));
            Assert.IsTrue(dict.ContainsKey(null));
        }
    }
}
