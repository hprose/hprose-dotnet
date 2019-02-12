
using System;

using BenchmarkDotNet.Attributes;

namespace Hprose.Benchmark.IO {
    [ClrJob, CoreJob, MonoJob]
    [RPlotExporter, RankColumn]
    public class BenchmarkFunc {
        private Func<int, bool> ToBoolean = Convert.ToBoolean;
        [Benchmark]
        public bool FuncBench0() => ((Func<int, bool>)((int value) => Convert.ToBoolean(value)))(1);
        [Benchmark]
        public bool FuncBench1() => ((Func<int, bool>)(Convert.ToBoolean))(1);
        [Benchmark]
        public bool FuncBench2() => ((Func<int, bool>)((int value) => value != 0))(1);
        [Benchmark]
        public bool FuncBench3() => Convert.ToBoolean(1);
        [Benchmark]
        public bool FuncBench4() => ToBoolean(1);
        [Benchmark]
        public bool FuncBench5() => Convert.ToBoolean((object)1);
    }
}
