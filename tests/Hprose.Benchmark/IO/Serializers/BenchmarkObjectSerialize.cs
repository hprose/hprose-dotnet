using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;

using Hprose.IO.Serializers;
using Hprose.IO.Deserializers;

using Newtonsoft.Json;

namespace Hprose.Benchmark.IO.Serializers {
    [ClrJob(isBaseline: true), CoreJob, MonoJob]
    [RPlotExporter, RankColumn]
    public class BenchmarkObjectSerialize {
        [DataContract(Name = "Person")]
        public class Person {
            [DataMember(Order = 0)]
            public int Id;
            [DataMember(Order = 1)]
            public string Name;
            [DataMember(Order = 2)]
            public int Age;
        }
        private static Person o = new Person {
            Id = 0,
            Name = "Tom",
            Age = 48
        };
        private static byte[] hproseData;
        private static byte[] dcData;
        private static string newtonData;

        static BenchmarkObjectSerialize() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(o);
                stream.Position = 0;
                Reader reader = new Reader(stream);
                reader.Deserialize<Person>();
                hproseData = stream.ToArray();
            }
            using (MemoryStream stream = new MemoryStream()) {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Person));
                js.WriteObject(stream, o);
                stream.Position = 0;
                dcData = stream.ToArray();
            }
            newtonData = JsonConvert.SerializeObject(o);
        }

        [Benchmark]
        public void HproseSerializeObject() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(o);
            }
        }
        [Benchmark]
        public void HproseDeserializeObject() {
            using (MemoryStream stream = new MemoryStream(hproseData)) {
                Reader reader = new Reader(stream);
                reader.Deserialize<Person>();
            }
        }
        [Benchmark]
        public void DataContractSerializeObject() {
            using (MemoryStream stream = new MemoryStream()) {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Person));
                js.WriteObject(stream, o);
                stream.Position = 0;
                js.ReadObject(stream);
            }
        }
        [Benchmark]
        public void DataContractDeserializeObject() {
            using (MemoryStream stream = new MemoryStream(dcData)) {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Person));
                js.ReadObject(stream);
            }
        }
        [Benchmark]
        public void NewtonJsonSerializeObject() => JsonConvert.SerializeObject(o);
        [Benchmark]
        public void NewtonJsonDeserializeObject() => JsonConvert.DeserializeObject(newtonData, typeof(Person));
    }
}
