﻿[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
namespace InvocationPerformanceBenchmark
{
    using System;
    using System.Globalization;
    using System.Threading;
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
        private readonly IBenchmark InvocationPerformance50 = new Steps50InvocationPerformanceBenchmark();
        private readonly IBenchmark ManualInvocationPerformance50 = new Steps50ManualInvocationPerformanceBenchmark();

        [GlobalSetup]
        public void Setup()
        {
            try
            {
                InvocationPerformance1.SetupAsync().GetAwaiter().GetResult();
                InvocationPerformance2.SetupAsync().GetAwaiter().GetResult();
                InvocationPerformance5.SetupAsync().GetAwaiter().GetResult();
                InvocationPerformance10.SetupAsync().GetAwaiter().GetResult();
                InvocationPerformance50.SetupAsync().GetAwaiter().GetResult();
                ManualInvocationPerformance50.SetupAsync().GetAwaiter().GetResult();
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
                InvocationPerformance50.DisposeAsync().AsTask().GetAwaiter().GetResult();
                ManualInvocationPerformance50.DisposeAsync().AsTask().GetAwaiter().GetResult();
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
            InvocationPerformance10.BenchmarkAsync().GetAwaiter().GetResult();
        }

        [Benchmark(Description = "Steps.Count == 50")]
        public void Steps50Benchmark()
        {
            InvocationPerformance50.BenchmarkAsync().GetAwaiter().GetResult();
        }

        [Benchmark(Description = "Loop simulation (i == 50)", Baseline = true)]
        public void LoopSimulationBenchmark()
        {
            ManualInvocationPerformance50.BenchmarkAsync().GetAwaiter().GetResult();
        }

        public static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            BenchmarkRunner.Run<Benchmarks>(DefaultConfig.Instance.WithOptions(ConfigOptions.StopOnFirstError));
        }
    }
}
