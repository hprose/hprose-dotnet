using System;

using BenchmarkDotNet.Running;

namespace Hprose.Benchmark.IO {
    public class Benchmark {
        static void Main(string[] args) {
            BenchmarkRunner.Run<BenchmarkNew>();
            BenchmarkRunner.Run<BenchmarkFunc>();
            BenchmarkRunner.Run<BenchmarkGetSerializer>();
            BenchmarkRunner.Run<BenchmarkObjectSerialize>();
            BenchmarkRunner.Run<BenchmarkDataSetSerialize>();
            Console.ReadKey();
        }
    }
}
