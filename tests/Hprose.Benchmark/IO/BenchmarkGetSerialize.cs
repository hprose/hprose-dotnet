
using BenchmarkDotNet.Attributes;

using Hprose.IO;

namespace Hprose.Benchmark.IO {
    [ClrJob, CoreJob, MonoJob]
    [RPlotExporter, RankColumn]
    public class BenchmarkGetSerializer {
        [Benchmark]
        public Serializer<string> GetSerializerFromDictionary() => Serializer.GetInstance(typeof(string)) as Serializer<string>;
        [Benchmark]
        public Serializer<string> GetSerializerFromStaticProperty() => Serializer<string>.Instance;
    }
}
