namespace WeirdExample.Pipelines.Simple
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using PackSite.Library.Pipelining;

    public class SimpleStep2 : IStep<SimpleArgs>
    {
        private readonly IInvokablePipelineFactory _invokablePipelineFactory;
        private readonly ILogger _logger;

        public SimpleStep2(IInvokablePipelineFactory invokablePipelineFactory, ILogger<SimpleStep2> logger)
        {
            _invokablePipelineFactory = invokablePipelineFactory;
            _logger = logger;
        }

        public async ValueTask ExecuteAsync(SimpleArgs args, StepDelegate next, IInvokablePipeline<SimpleArgs> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.Value += GetType().Name + " > ";

            IInvokablePipeline<SimpleArgs> invokableSubpipeline = _invokablePipelineFactory.GetRequiredPipeline<SimpleArgs>("dynamic-subpipeline-demo");
            await invokableSubpipeline.InvokeAsync(args, next, cancellationToken);
            _logger.LogInformation("\n[SUBPIPELINE]\nIPC: {@IPCounters}\nPC: {@PCounters}", invokableSubpipeline.Counters, invokableSubpipeline.Pipeline.Counters);

            args.Value += GetType().Name + " < ";
        }
    }
}
