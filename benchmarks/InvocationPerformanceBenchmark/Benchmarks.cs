[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
namespace InvocationPerformanceBenchmark
{
    using System;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Order;
    using BenchmarkDotNet.Running;
    using InvocationPerformanceBenchmark.BenchmarksDefinitions;

    [SimpleJob]
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class Benchmarks
    {
        private readonly IBenchmark InvocationPerformance1 = new Steps1InvocationPerformanceBenchmark();
        private readonly IBenchmark InvocationPerformance2 = new Steps2InvocationPerformanceBenchmark();
        private readonly IBenchmark InvocationPerformance5 = new Steps5InvocationPerformanceBenchmark();
        private readonly IBenchmark InvocationPerformance10 = new Steps10InvocationPerformanceBenchmark();
        private readonly IBenchmark InvocationPerformance100 = new Steps100InvocationPerformanceBenchmark();
        private readonly IBenchmark ManualInvocationPerformance100 = new Steps100ManualInvocationPerformanceBenchmark();

        [GlobalSetup]
        public void Setup()
        {
            try
            {
                InvocationPerformance1.SetupAsync().GetAwaiter().GetResult();
                InvocationPerformance2.SetupAsync().GetAwaiter().GetResult();
                InvocationPerformance5.SetupAsync().GetAwaiter().GetResult();
                InvocationPerformance10.SetupAsync().GetAwaiter().GetResult();
                InvocationPerformance100.SetupAsync().GetAwaiter().GetResult();
                ManualInvocationPerformance100.SetupAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                throw;
            }
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            try
            {
                InvocationPerformance1.DisposeAsync().AsTask().GetAwaiter().GetResult();
                InvocationPerformance2.DisposeAsync().AsTask().GetAwaiter().GetResult();
                InvocationPerformance5.DisposeAsync().AsTask().GetAwaiter().GetResult();
                InvocationPerformance10.DisposeAsync().AsTask().GetAwaiter().GetResult();
                InvocationPerformance100.DisposeAsync().AsTask().GetAwaiter().GetResult();
                ManualInvocationPerformance100.DisposeAsync().AsTask().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        [Benchmark(Description = "Steps.Count == 1")]
        public void Steps1Benchmark()
        {
            InvocationPerformance1.BenchmarkAsync().GetAwaiter().GetResult();
        }

        [Benchmark(Description = "Steps.Count == 2")]
        public void Steps2Benchmark()
        {
            InvocationPerformance2.BenchmarkAsync().GetAwaiter().GetResult();
        }

        [Benchmark(Description = "Steps.Count == 5")]
        public void Steps5Benchmark()
        {
            InvocationPerformance5.BenchmarkAsync().GetAwaiter().GetResult();
        }

        [Benchmark(Description = "Steps.Count == 10")]
        public void Steps10Benchmark()
        {
            InvocationPerformance5.BenchmarkAsync().GetAwaiter().GetResult();
        }

        [Benchmark(Description = "Steps.Count == 100")]
        public void Steps50Benchmark()
        {
            InvocationPerformance5.BenchmarkAsync().GetAwaiter().GetResult();
        }

        [Benchmark(Description = "Loop simulation", Baseline = true)]
        public void LoopSimulationBenchmark()
        {
            ManualInvocationPerformance100.BenchmarkAsync().GetAwaiter().GetResult();
        }

        public static void Main()
        {
            BenchmarkRunner.Run<Benchmarks>(DefaultConfig.Instance.WithOptions(ConfigOptions.StopOnFirstError));
        }
    }
}
