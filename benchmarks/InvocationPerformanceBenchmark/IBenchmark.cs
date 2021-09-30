namespace InvocationPerformanceBenchmark
{
    using System;
    using System.Threading.Tasks;

    public interface IBenchmark : IAsyncDisposable
    {
        Task SetupAsync();

        Task BenchmarkAsync();
    }
}