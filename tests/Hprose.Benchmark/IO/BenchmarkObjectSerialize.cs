using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using BenchmarkDotNet.Attributes;

using Newtonsoft.Json;

namespace Hprose.Benchmark.IO {
    [ClrJob, CoreJob, MonoJob]
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
        private static readonly Person[] persons = new Person[] {
           new Person {
               Id = 0,
               Name = "Tom",
               Age = 48
           },
           new Person {
               Id = 1,
               Name = "Jerry",
               Age = 36
           },
           new Person {
               Id = 2,
               Name = "Spike",
               Age = 53
           }
        };
        private static readonly byte[] hproseData;
        private static readonly byte[] dcData;
        private static readonly string newtonData;
        private static readonly string sstextData;

        static BenchmarkObjectSerialize() {
            hproseData = Hprose.IO.Formatter.Serialize(persons);
            newtonData = JsonConvert.SerializeObject(persons);
            sstextData = ServiceStack.Text.JsonSerializer.SerializeToString(persons);
            using (MemoryStream stream = new MemoryStream()) {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Person[]));
                js.WriteObject(stream, persons);
                stream.Position = 0;
                dcData = stream.ToArray();
            }
        }

        [Benchmark]
        public void HproseSerializeObject() => Hprose.IO.Formatter.Serialize(persons);
        [Benchmark]
        public void NewtonJsonSerializeObject() => JsonConvert.SerializeObject(persons);
        [Benchmark]
        public void ServiceStackSerializeObject() => ServiceStack.Text.JsonSerializer.SerializeToString(persons);
        [Benchmark]
        public void DataContractSerializeObject() {
            using (MemoryStream stream = new MemoryStream()) {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Person[]));
                js.WriteObject(stream, persons);
            }
        }
        [Benchmark]
        public void HproseDeserializeObject() => Hprose.IO.Formatter.Deserialize<Person[]>(hproseData);
        [Benchmark]
        public void NewtonJsonDeserializeObject() => JsonConvert.DeserializeObject<Person[]>(newtonData);
        [Benchmark]
        public void ServiceStackDeserializeObject() => ServiceStack.Text.JsonSerializer.DeserializeFromString<Person[]>(sstextData);
        [Benchmark]
        public void DataContractDeserializeObject() {
            using (MemoryStream stream = new MemoryStream(dcData)) {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Person[]));
                js.ReadObject(stream);
            }
        }
    }
}
