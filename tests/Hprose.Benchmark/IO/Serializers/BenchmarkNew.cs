
using System;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;
using Hprose.IO.Deserializers;

namespace Hprose.Benchmark.IO.Serializers {
    [ClrJob(isBaseline: true), CoreJob, MonoJob]
    [RPlotExporter, RankColumn]
    public class BenchmarkNew {
        public T New<T>() where T : new() => new T();
        public T CreateInstance<T>() => (T)Activator.CreateInstance(typeof(T));
        public T CreateInstance2<T>() => (T)Activator.CreateInstance(typeof(T), true);
        [Benchmark]
        public object BenchNew() => New<object>();
        [Benchmark]
        public object BenchCreateInstance() => CreateInstance<object>();
        [Benchmark]
        public object BenchCreateInstance2() => CreateInstance2<object>();
        [Benchmark]
        public object BenchFactoryNew() => Factory<object>.New();
    }
}
