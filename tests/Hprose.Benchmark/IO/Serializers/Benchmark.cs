using System;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;

using Hprose.IO.Serializers;

namespace Hprose.Benchmark.IO.Serializers {
    [ClrJob(isBaseline: true), CoreJob, MonoJob]
    [RPlotExporter, RankColumn]
    public class Benchmark {
        [Benchmark]
        public Serializer<string> GetSerializerFromDictionary() => Serializer.GetInstance(typeof(string)) as Serializer<string>;
        [Benchmark]
        public Serializer<string> GetSerializerFromStaticProperty() => Serializer<string>.Instance;
        static void Main(string[] args) {
            var summary = BenchmarkRunner.Run<Benchmark>();
            Console.ReadKey();
        }
    }
}
