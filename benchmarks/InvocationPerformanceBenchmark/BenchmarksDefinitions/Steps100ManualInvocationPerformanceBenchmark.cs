namespace InvocationPerformanceBenchmark.BenchmarksDefinitions
{
    using System.Threading.Tasks;
    using InvocationPerformanceBenchmark.Steps;

    public sealed class Steps100ManualInvocationPerformanceBenchmark : IBenchmark
    {
        private readonly NopStep Step = new();

        public Steps100ManualInvocationPerformanceBenchmark()
        {

        }

        public Task SetupAsync()
        {
            return Task.CompletedTask;
        }

        public Task BenchmarkAsync()
        {
            var args = new ProcessingArgs();

            for (int i = 0; i < 100; i++)
            {
                ++args.Value;
                --args.Value;
            }

            return Task.CompletedTask;
        }

        public ValueTask DisposeAsync()
        {
            return default;
        }
    }
}
