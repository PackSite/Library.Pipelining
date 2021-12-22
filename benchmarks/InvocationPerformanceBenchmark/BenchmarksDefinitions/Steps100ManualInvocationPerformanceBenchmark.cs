namespace InvocationPerformanceBenchmark.BenchmarksDefinitions
{
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using InvocationPerformanceBenchmark.Steps;

    public sealed class Steps100ManualInvocationPerformanceBenchmark : IBenchmark
    {
        public Steps100ManualInvocationPerformanceBenchmark()
        {

        }

        public Task SetupAsync()
        {
            return Task.CompletedTask;
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        public Task BenchmarkAsync()
        {
            ProcessingArgs args = new();

            for (int m = 0; m < 100; m++)
            {
                for (int i = 0; i < 100; i++)
                {
                    ++args.Value;

                    //await Task.Yield();

                    --args.Value;
                }
            }

            return Task.CompletedTask;
        }

        public ValueTask DisposeAsync()
        {
            return default;
        }
    }
}
