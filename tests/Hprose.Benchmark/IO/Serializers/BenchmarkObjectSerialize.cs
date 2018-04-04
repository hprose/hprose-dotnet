using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;

using Hprose.IO.Serializers;

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
        private Person o = new Person {
            Id = 0,
            Name = "Tom",
            Age = 48
        };
        [Benchmark]
        public void HproseSerializeObject() {
            using (MemoryStream stream = new MemoryStream()) {
                Writer writer = new Writer(stream);
                writer.Serialize(o);
            }
        }
        [Benchmark]
        public void DataContractSerializeObject() {
            using (MemoryStream stream = new MemoryStream()) {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Person));
                js.WriteObject(stream, o);
            }
        }
        [Benchmark]
        public void NewtonJsonSerializeObject() => JsonConvert.SerializeObject(o);
    }
}
