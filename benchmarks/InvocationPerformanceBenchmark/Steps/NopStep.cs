namespace InvocationPerformanceBenchmark.Steps
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    public sealed class NopStep : IStep<ProcessingArgs>
    {
        public async Task ExecuteAsync(ProcessingArgs args, StepDelegate next, IInvokablePipeline<ProcessingArgs> invokablePipeline, CancellationToken cancellationToken = default)
        {
            ++args.Value;

            await next();

            --args.Value;
        }
    }
}
