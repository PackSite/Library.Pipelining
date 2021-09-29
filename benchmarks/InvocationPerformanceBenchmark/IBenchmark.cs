namespace InvocationPerformanceBenchmark
{
    using System;
    using System.Threading.Tasks;

    public interface IBenchmark : IAsyncDisposable
    {
        ValueTask SetupAsync();

        ValueTask BenchmarkAsync();
    }
}