
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;

using Hprose.IO.Serializers;

namespace Hprose.Benchmark.IO.Serializers {
    [ClrJob(isBaseline: true), CoreJob, MonoJob]
    [RPlotExporter, RankColumn]
    public class BenchmarkGetSerializer {
        [Benchmark]
        public Serializer<string> GetSerializerFromDictionary() => Hprose.IO.Serializers.Serializers.GetInstance(typeof(string)) as Serializer<string>;
        [Benchmark]
        public Serializer<string> GetSerializerFromStaticProperty() => Serializer<string>.Instance;
    }
}
