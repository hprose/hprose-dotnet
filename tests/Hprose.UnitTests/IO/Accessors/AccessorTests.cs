using System;
using System.IO;
using System.Runtime.Serialization;

using Hprose.IO.Accessors;
using Hprose.IO.Serializers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hprose.UnitTests.IO.Serializers {
    [TestClass]
    public class AccessorTests {
        [DataContract(Name = "Hello")]
        public class TestClass {
            private string _name = "World";
            [DataMember(Order = 2)]
            public string Name {
                get => _name;
                set => _name = value;
            }
            [DataMember(Name = "id", Order = 1)]
            public int ID = 1;
            public int Age = 10;
            [IgnoreDataMember]
            public bool OOXX = true;
            [NonSerialized]
            public bool XXOO = false;
            public string ReadOnlyProp => "readonly";
        }
        [Serializable]
        public class TestClass2 {
            private string _name = "World";
            [DataMember(Order = 2)]
            public string Name {
                get => _name;
                set => _name = value;
            }
            [DataMember(Name = "id", Order = 1)]
            public int ID = 2;
            public int Age = 10;
            [IgnoreDataMember]
            public bool OOXX = true;
            [NonSerialized]
            public bool XXOO = false;
            public string ReadOnlyProp => "readonly";
        }
        public class TestClass3 {
            private string _name = "World";
            [DataMember(Order = 2)]
            public string Name {
                get => _name;
                set => _name = value;
            }
            [DataMember(Name = "id", Order = 1)]
            public int ID = 2;
            public int Age = 10;
            [IgnoreDataMember]
            public bool OOXX = true;
            [NonSerialized]
            public bool XXOO = false;
            public string ReadOnlyProp => "readonly";
        }
        [DataContract]
        public class TestClass4 {
            private string _name = "World";
            [DataMember(Order = 2)]
            private string Name {
                get => _name;
                set => _name = value;
            }
            [DataMember(Name = "id", Order = 1)]
            private int ID = 1;
            private int Age = 10;
            [IgnoreDataMember]
            private bool OOXX = true;
            [NonSerialized]
            private bool XXOO = false;
            private string ReadOnlyProp => "readonly";
        }
        [Serializable]
        public class TestClass5 {
            private string _name = "World";
            [DataMember(Order = 2)]
            private string Name {
                get => _name;
                set => _name = value;
            }
            [DataMember(Name = "id", Order = 1)]
            private int ID = 2;
            private int Age = 10;
            [IgnoreDataMember]
            private bool OOXX = true;
            [NonSerialized]
            private bool XXOO = false;
            private string ReadOnlyProp => "readonly";
        }
        public class TestClass6 {
            private string _name = "World";
            [DataMember(Order = 2)]
            private string Name {
                get => _name;
                set => _name = value;
            }
            [DataMember(Name = "id", Order = 1)]
            private int ID = 2;
            private int Age = 10;
            [IgnoreDataMember]
            private bool OOXX = true;
            [NonSerialized]
            private bool XXOO = false;
            private string ReadOnlyProp => "readonly";
        }
        public static string Serialize<T>(T obj) {
            using(var stream = new MemoryStream()) {
                var writer = new Writer(stream);
                writer.Serialize(obj);
                return ValueWriter.UTF8.GetString(stream.ToArray());
            }
        }
        [TestMethod]
        public void TestGetMembers() {
            Assert.AreEqual("a2{s2\"id\"s4\"name\"}", Serialize(MembersAccessor<TestClass>.Members.Keys));
            Assert.AreEqual("a3{s3\"age\"s2\"id\"s4\"name\"}", Serialize(MembersAccessor<TestClass2>.Members.Keys));
            Assert.AreEqual("a3{s3\"age\"s2\"id\"s4\"name\"}", Serialize(MembersAccessor<TestClass3>.Members.Keys));
            Assert.AreEqual("a2{s2\"id\"s4\"name\"}", Serialize(MembersAccessor<TestClass4>.Members.Keys));
            Assert.AreEqual("a{}", Serialize(MembersAccessor<TestClass5>.Members.Keys));
            Assert.AreEqual("a{}", Serialize(MembersAccessor<TestClass6>.Members.Keys));
        }
        [TestMethod]
        public void TestGetFields() {
            Assert.AreEqual("a{}", Serialize(FieldsAccessor<TestClass>.Fields.Keys));
            Assert.AreEqual("a3{s5\"_name\"s3\"age\"s2\"id\"}", Serialize(FieldsAccessor<TestClass2>.Fields.Keys));
            Assert.AreEqual("a{}", Serialize(FieldsAccessor<TestClass3>.Fields.Keys));
            Assert.AreEqual("a{}", Serialize(FieldsAccessor<TestClass4>.Fields.Keys));
            Assert.AreEqual("a3{s5\"_name\"s3\"age\"s2\"id\"}", Serialize(FieldsAccessor<TestClass5>.Fields.Keys));
            Assert.AreEqual("a{}", Serialize(FieldsAccessor<TestClass6>.Fields.Keys));
        }
        [TestMethod]
        public void TestGetProperties() {
            Assert.AreEqual("a{}", Serialize(PropertiesAccessor<TestClass>.Properties.Keys));
            Assert.AreEqual("a1{s4\"name\"}", Serialize(PropertiesAccessor<TestClass2>.Properties.Keys));
            Assert.AreEqual("a{}", Serialize(PropertiesAccessor<TestClass3>.Properties.Keys));
            Assert.AreEqual("a{}", Serialize(PropertiesAccessor<TestClass4>.Properties.Keys));
            Assert.AreEqual("a{}", Serialize(PropertiesAccessor<TestClass5>.Properties.Keys));
            Assert.AreEqual("a{}", Serialize(PropertiesAccessor<TestClass6>.Properties.Keys));
        }
    }
}
