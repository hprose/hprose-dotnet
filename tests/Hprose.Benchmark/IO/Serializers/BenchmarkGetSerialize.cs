
using BenchmarkDotNet.Attributes;

using Hprose.IO.Serializers;

namespace Hprose.Benchmark.IO.Serializers {
    [ClrJob, CoreJob, MonoJob]
    [RPlotExporter, RankColumn]
    public class BenchmarkGetSerializer {
        [Benchmark]
        public Serializer<string> GetSerializerFromDictionary() => Hprose.IO.Serializers.Serializers.GetInstance(typeof(string)) as Serializer<string>;
        [Benchmark]
        public Serializer<string> GetSerializerFromStaticProperty() => Serializer<string>.Instance;
    }
}
