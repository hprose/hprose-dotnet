using System;

using BenchmarkDotNet.Running;

namespace Hprose.Benchmark.IO.Serializers {
    public class Benchmark {
        static void Main(string[] args) {
            BenchmarkRunner.Run<BenchmarkFunc>();
            //BenchmarkRunner.Run<BenchmarkGetSerializer>();
            //BenchmarkRunner.Run<BenchmarkObjectSerialize>();
            //BenchmarkRunner.Run<BenchmarkDataSetSerialize>();
            Console.ReadKey();
        }
    }
}
