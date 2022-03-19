namespace SubPipelineExample.Pipelines
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    public class Step2 : IStep<DemoArgs>
    {
        private readonly IInvokablePipelineFactory _invokablePipelineFactory;

        public Step2(IInvokablePipelineFactory invokablePipelineFactory)
        {
            _invokablePipelineFactory = invokablePipelineFactory;
        }

        public async ValueTask ExecuteAsync(DemoArgs args, StepDelegate next, IInvokablePipeline<DemoArgs> invokablePipeline, CancellationToken cancellationToken = default)
        {
            args.Value += GetType().Name + " > ";

            IInvokablePipeline<DemoArgs> invokableSubpipeline = _invokablePipelineFactory.GetRequiredPipeline<DemoArgs>("dynamic-sub-pipeline-demo");
            await invokableSubpipeline.InvokeAsync(args, next, cancellationToken);

            args.Value += GetType().Name + " < ";
        }
    }
}
