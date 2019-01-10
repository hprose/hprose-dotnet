
using System;
using System.Linq.Expressions;
using BenchmarkDotNet.Attributes;
using Hprose.IO.Deserializers;

namespace Hprose.Benchmark.IO.Serializers {
    [ClrJob, CoreJob, MonoJob]
    [RPlotExporter, RankColumn]
    public class BenchmarkNew {
        public T New<T>() where T : new() => new T();
        public T CreateInstance<T>() => (T)Activator.CreateInstance(typeof(T));
        public T CreateInstance2<T>() => (T)Activator.CreateInstance(typeof(T), true);
        public T ExpressionNew<T>() => Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile()();
        [Benchmark]
        public object BenchNew() => New<object>();
        [Benchmark]
        public object BenchCreateInstance() => CreateInstance<object>();
        [Benchmark]
        public object BenchCreateInstance2() => CreateInstance2<object>();
        [Benchmark]
        public object BenchFactoryNew() => Factory<object>.New();
        [Benchmark]
        public object BenchExpressionNew() => ExpressionNew<object>();
    }
}
