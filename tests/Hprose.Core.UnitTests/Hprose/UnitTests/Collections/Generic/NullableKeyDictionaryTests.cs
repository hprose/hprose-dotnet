using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hprose.Collections.Generic;
using System.Linq;

namespace Hprose.Core.UnitTests.Hprose.UnitTests.Collections.Generic {
    [TestClass]
    public class NullableKeyDictionaryTests {
        [TestMethod]
        public void TestNullableKeyDictionaryString() {
            var dict = new NullableKeyDictionary<string, string> {
                { "Hello", "World" },
                { null, "Null" }
            };
            Assert.AreEqual(dict.Count, 2);
        }
        [TestMethod]
        public void TestNullableKeyDictionaryInt() {
            var dict = new NullableKeyDictionary<int?, string> {
                { 1, "Hello" },
                { null, "Null" }
            };
            Assert.AreEqual(dict.Count, 2);
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
            Assert.AreEqual(dict["hello"], dict["Hello"]);
            Assert.IsTrue(dict.ContainsKey("hello"));
            Assert.IsTrue(dict.ContainsKey(null));
        }
    }
}
